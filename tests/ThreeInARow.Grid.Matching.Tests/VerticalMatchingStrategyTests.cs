using ThreeInARow.Grid.Implementations;
using ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;
using ThreeInARow.Grid.ValueObjects;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Grid.Matching.Tests;

public class VerticalMatchingStrategyTests : MGridTestUtility
{
    private readonly VerticalMatchingStrategy<string> _strategy = new(minMatchLength: 3);

    #region Find Matches Tests

    #region Scenarios Where No Vertical Matches Are Found

    [Test]
    public void GivenAnEmptyGrid_WhenPlayerLooksForVerticalMatches_ThenNoMatchesAreFound()
    {
        // Given a completely empty grid with no elements
        var emptyCells = Enumerable.Empty<ElementCell<string>>();

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(emptyCells);

        // Then no matches are found
        Assert.That(matches, Is.Empty, "Empty grid should contain no vertical matches");
    }

    [Test]
    public void GivenAGridWithNoConsecutiveVerticalElements_WhenPlayerLooksForVerticalMatches_ThenNoMatchesAreFound()
    {
        // Given a grid where no three consecutive elements are the same vertically
        var grid = new[,]
        {
            { "A", "B", "C" },
            { "B", "A", "B" },
            { "A", "B", "A" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then no matches are found because no column has three consecutive identical elements
        Assert.That(matches, Is.Empty, "Grid with no consecutive identical vertical elements should have no vertical matches");
    }

    [Test]
    public void GivenAGridWithEmptyCellsBreakingPotentialMatches_WhenPlayerLooksForVerticalMatches_ThenNoMatchesAreFound()
    {
        // Given a grid where empty cells break what would otherwise be valid vertical matches
        var grid = new[,]
        {
            { "A", "B", "C", "D" },
            { "A", "C", "D", "E" }, // AA-AA broken by empty cell
            { null, "D", "E", "F" }, // Empty cell breaks vertical continuity
            { "A", "E", "F", "G" },
            { "A", "F", "G", "H" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then no matches are found because empty cells break the continuity
        Assert.That(matches, Is.Empty, "Empty cells should break potential vertical matches");
    }

    #endregion

    #region Scenarios Where Single Vertical Matches Are Found

    [Test]
    public void GivenAGridWithOneMinimalVerticalMatch_WhenPlayerLooksForVerticalMatches_ThenOneMatchIsFound()
    {
        // Given a grid with exactly three consecutive identical elements in one column
        var grid = new[,]
        {
            { "A", "B", "C", "D" },
            { "A", "C", "D", "E" }, // AAA vertical match in column 0
            { "A", "D", "E", "F" },
            { "B", "E", "F", "G" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then exactly one match is found with the correct positions
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one vertical match");
        Assert.That(matches[0].Count, Is.EqualTo(3), "Minimal vertical match should contain exactly three elements");
    }

    [Test]
    public void GivenAGridWithOneExtendedVerticalMatch_WhenPlayerLooksForVerticalMatches_ThenOneLongerMatchIsFound()
    {
        // Given a grid with more than three consecutive identical elements in one column
        var grid = new[,]
        {
            { "A", "B", "C", "D" },
            { "A", "C", "D", "E" }, // AAAAA vertical match in column 0 (5 elements)
            { "A", "D", "E", "F" },
            { "A", "E", "F", "G" },
            { "A", "F", "G", "H" },
            { "B", "G", "H", "I" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then exactly one extended match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one extended vertical match");
        Assert.That(matches[0].Count, Is.EqualTo(5), "Extended match should include all five consecutive elements");
    }

    [Test]
    public void GivenAGridWithAMatchAfterEmptySpaces_WhenPlayerLooksForVerticalMatches_ThenTheMatchIsFound()
    {
        // Given a grid where a valid vertical match occurs after some empty cells
        var grid = new[,]
        {
            { null, "B", "C", "D" },
            { null, "C", "D", "E" },
            { "A", "D", "E", "F" }, // AAA vertical match after empty cells
            { "A", "E", "F", "G" },
            { "A", "F", "G", "H" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then the match is found despite the preceding empty cells
        Assert.That(matches, Has.Count.EqualTo(1), "Should find vertical match even after empty cells");
        Assert.That(matches[0].Count, Is.EqualTo(3), "Match should include exactly three elements");
    }

    [Test]
    public void GivenAGridWithAMatchBeforeEmptySpaces_WhenPlayerLooksForVerticalMatches_ThenTheMatchIsFound()
    {
        // Given a grid where a valid vertical match occurs before some empty cells
        var grid = new[,]
        {
            { "A", "B", "C", "D" }, // AAA vertical match before empty cells
            { "A", "C", "D", "E" },
            { "A", "D", "E", "F" },
            { null, "E", "F", "G" },
            { null, "F", "G", "H" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then the match is found despite the following empty cells
        Assert.That(matches, Has.Count.EqualTo(1), "Should find vertical match even before empty cells");
        Assert.That(matches[0].Count, Is.EqualTo(3), "Match should include exactly three elements");
    }

    #endregion

    #region Scenarios Where Multiple Vertical Matches Are Found

    [Test]
    public void GivenAGridWithMultipleMatchesInTheSameColumn_WhenPlayerLooksForVerticalMatches_ThenAllMatchesAreFound()
    {
        // Given a grid with two separate vertical matches in the same column
        var grid = new[,]
        {
            { "A", "B", "C", "D" },
            { "A", "C", "D", "E" }, // AAA match at top of column 0
            { "A", "D", "E", "F" },
            { "B", "E", "F", "G" }, // Separator element
            { "C", "F", "G", "H" }, // CCC match at bottom of column 0
            { "C", "G", "H", "I" },
            { "C", "H", "I", "J" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then both matches are found
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both vertical matches in the same column");
    }

    [Test]
    public void GivenAGridWithMatchesInDifferentColumns_WhenPlayerLooksForVerticalMatches_ThenAllMatchesAreFound()
    {
        // Given a grid with vertical matches in different columns
        var grid = new[,]
        {
            { "A", "B", "C", "D" }, // AAA match in column 0, BBB match in column 1
            { "A", "B", "D", "E" },
            { "A", "B", "E", "F" },
            { "B", "C", "F", "G" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then both matches are found across different columns
        Assert.That(matches, Has.Count.EqualTo(2), "Should find vertical matches in different columns");
    }

    [Test]
    public void GivenAGridWithMatchesSeparatedByEmptySpaces_WhenPlayerLooksForVerticalMatches_ThenBothMatchesAreFound()
    {
        // Given a grid with two vertical matches in the same column separated by empty cells
        var grid = new[,]
        {
            { "A", "B", "C", "D" }, // AAA at top and AAA at bottom, separated by empty cell
            { "A", "C", "D", "E" },
            { "A", "D", "E", "F" },
            { null, "E", "F", "G" }, // Empty cell separates the matches
            { "A", "F", "G", "H" },
            { "A", "G", "H", "I" },
            { "A", "H", "I", "J" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then both matches are found despite the empty cell between them
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both matches separated by empty cells");
    }

    [Test]
    public void GivenAGridWithMultipleMatchesOfDifferentElements_WhenPlayerLooksForVerticalMatches_ThenAllDifferentMatchesAreFound()
    {
        // Given a grid with vertical matches of different element types
        var grid = new[,]
        {
            { "A", "B", "C", "D", "E", "F" },
            { "A", "B", "X", "D", "Y", "F" }, // AAA and BBB matches in columns 0 and 1
            { "A", "B", "Y", "D", "Z", "F" },
            { "X", "X", "Z", "D", "W", "Q" }, // DDD match in column 3
            { "Y", "Y", "W", "D", "V", "R" },
            { "Z", "Z", "V", "D", "U", "S" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then all three different matches are found
        Assert.That(matches, Has.Count.EqualTo(4), "Should find all vertical matches of different element types");
    }

    [Test]
    public void GivenAGridWithAdjacentColumnsHavingMatches_WhenPlayerLooksForVerticalMatches_ThenAllAdjacentMatchesAreFound()
    {
        // Given a grid where adjacent columns have vertical matches
        var grid = new[,]
        {
            { "A", "B", "C", "D" }, // AAA in column 0, BBB in column 1
            { "A", "B", "E", "F" },
            { "A", "B", "G", "H" },
            { "X", "Y", "I", "J" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then both adjacent matches are found as separate matches
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both adjacent vertical matches separately");
    }

    [Test]
    public void GivenAGridWithVerticalMatchesAtGridEdges_WhenPlayerLooksForVerticalMatches_ThenEdgeMatchesAreFound()
    {
        // Given a grid with vertical matches at the top and bottom edges
        var grid = new[,]
        {
            { "A", "B", "C", "D" }, // AAA match at top edge
            { "A", "E", "F", "G" },
            { "A", "H", "I", "J" },
            { "X", "K", "L", "M" },
            { "Y", "N", "O", "P" },
            { "Z", "Q", "R", "Z" }, // ZZZ match at bottom edge
            { "W", "S", "T", "Z" },
            { "V", "U", "V", "Z" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for vertical matches
        var matches = _strategy.FindMatches(cells);

        // Then both edge matches are found
        Assert.That(matches, Has.Count.EqualTo(2), "Should find vertical matches at grid edges");
    }

    #endregion

    #endregion

    #region Find Potential Matches Tests

    [Test]
    public void GivenAnEmptyGrid_WhenCheckingForPotentialVerticalMatches_ThenReturnsGridHasEmptyCells()
    {
        // Given a completely empty grid with no elements
        var grid = new HorizontalVerticalSwapGrid<string>(new string?[2,2]);

        // When checking for potential vertical matches
        var result = _strategy.HasPotentialMatches(grid);

        // Then it returns that the grid has empty cells
        Assert.That(result.IsT1, Is.True, "Empty grid should not be considered to have potential vertical matches");
    }

    [Test]
    public void GivenAGridWithNoPotentialVerticalMatches_WhenCheckingForPotentialMatches_ThenReturnsFalse()
    {
        // Given a grid where no three consecutive elements are the same vertically
        var gridData = new[,]
        {
            { "A", "B", "C" },
            { "B", "D", "B" },
            { "A", "E", "A" }
        };
        var grid = new HorizontalVerticalSwapGrid<string>(gridData);

        // When checking for potential vertical matches
        var result = _strategy.HasPotentialMatches(grid);

        // Then the result is false
        Assert.That(result.IsT0, Is.True, "When passing correct grid the result must be boolean");
        Assert.That(result.AsT0, Is.False, "Grid with no consecutive identical vertical elements should not have potential matches");
    }

    [Test]
    public void GivenAGridWithNoPotentialVerticalMatchesWithTwoConsecutiveElements_WhenCheckingForPotentialMatches_ThenReturnsFalse()
    {
        // Given a grid where two columns have two consecutive identical elements but do not form a potential match
        var gridData = new[,]
        {
            { "A", "B", "C" },
            { "A", "B", "B" },
            { "D", "E", "A" }
        };
        var grid = new HorizontalVerticalSwapGrid<string>(gridData);

        // When checking for potential vertical matches
        var result = _strategy.HasPotentialMatches(grid);

        // Then it returns false
        Assert.That(result.IsT0, Is.True, "When passing correct grid the result must be boolean");
        Assert.That(result.AsT0, Is.False, "Grid with no consecutive identical vertical elements should not have potential matches");
    }

    [Test]
    public void GivenAGridWithPotentialVerticalMatchEqualToMinMatchLength_WhenCheckingForPotentialMatches_ThenReturnsTrue()
    {
        // Given a grid with potential vertical matches
        var gridData = new[,]
        {
            { "A", "B", "C" },
            { "A", "B", "D" }, // Potential match in column 0,1
            { "B", "A", "F" }
        };
        var grid = new HorizontalVerticalSwapGrid<string>(gridData);

        // When checking for potential vertical matches
        var result = _strategy.HasPotentialMatches(grid);

        // Then it returns true
        Assert.That(result.IsT0, Is.True, "When passing correct grid the result must be boolean");
        Assert.That(result.AsT0, Is.True, "Grid with potential vertical matches should return true");
    }

    [Test]
    public void GivenAGridWithPotentialMultipleMatches_WhenCheckingForPotentialMatches_ThenReturnsTrue()
    {
        // Given a grid with potential vertical matches
        var gridData = new[,]
        {
            { "A", "B", "C", "D", "E", "F"},
            { "A", "Z", "D", "E", "O", "F" }, // Potential match in column 0,1
            { "B", "A", "F", "H", "F", "J" },
            { "C", "Z", "G", "K", "L", "M" }
        };
        var grid = new HorizontalVerticalSwapGrid<string>(gridData);

        // When checking for potential vertical matches
        var result = _strategy.HasPotentialMatches(grid);

        // Then it returns true
        Assert.That(result.IsT0, Is.True, "When passing correct grid the result must be boolean");
        Assert.That(result.AsT0, Is.True, "Grid with potential vertical matches should return true");
    }

    [Test]
    public void GivenAGridWithPotentialVerticalMatchesGreaterThanMinMatchLength_WhenCheckingForPotentialMatches_ThenReturnsTrue()
    {
        // Given a grid with potential vertical matches greater than the minimum match length
        var gridData = new[,]
        {
            { "E", "A", "C" },
            { "D", "A", "D" }, // Potential match in column 1
            { "C", "B", "A" },
            { "F", "A", "H" }
        };
        var grid = new HorizontalVerticalSwapGrid<string>(gridData);

        // When checking for potential vertical matches
        var result = _strategy.HasPotentialMatches(grid);

        // Then it returns true
        Assert.That(result.IsT0, Is.True, "When passing correct grid the result must be boolean");
        Assert.That(result.AsT0, Is.True, "Grid with potential vertical matches greater than min length should return true");
    }

    [Test]
    public void GivenAGridWithPotentialVerticalMatchForTShape_WhenCheckingForPotentialMatches_ThenReturnsTrue()
    {
        // Given a grid with a potential vertical match that could form a T-shape
        var gridData = new[,]
        {
            { "Y", "A", "C" },
            { "B", "A", "D" }, // Potential match in column 1
            { "A", "E", "A" },
            { "G", "A", "I" }
        };
        var grid = new HorizontalVerticalSwapGrid<string>(gridData);

        // When checking for potential vertical matches
        var result = _strategy.HasPotentialMatches(grid);

        // Then it returns true
        Assert.That(result.IsT0, Is.True, "When passing correct grid the result must be boolean");
        Assert.That(result.AsT0, Is.True, "Grid with potential vertical matches for T-shape should return true");
    }

    [Test]
    public void GivenAGridWithPotentialVerticalMatchForLShape_WhenCheckingForPotentialMatches_ThenReturnsTrue()
    {
        // Given a grid with a potential vertical match that could form an L-shape
        var gridData = new[,]
        {
            { "Y", "C", "C" },
            { "C", "A", "C" }, // Potential match in column 1
            { "A", "E", "Z" },
            { "G", "A", "I" }
        };
        var grid = new HorizontalVerticalSwapGrid<string>(gridData);

        // When checking for potential vertical matches
        var result = _strategy.HasPotentialMatches(grid);

        // Then it returns true
        Assert.That(result.IsT0, Is.True, "When passing correct grid the result must be boolean");
        Assert.That(result.AsT0, Is.True, "Grid with potential vertical matches for T-shape should return true");
    }

    #endregion
}