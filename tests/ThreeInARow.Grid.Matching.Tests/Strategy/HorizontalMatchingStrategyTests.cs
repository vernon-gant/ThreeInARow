using ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Grid.Matching.Tests.Strategy;

public class HorizontalMatchingStrategyTests : MGridTestUtility
{
    private readonly HorizontalMatchingStrategy<string> _strategy = new(minMatchLength: 3);

    #region Scenarios Where No Horizontal Matches Are Found

    [Test]
    public void GivenAnEmptyGrid_WhenPlayerLooksForHorizontalMatches_ThenNoMatchesAreFound()
    {
        // Given a completely empty grid with no elements
        var readableGrid = this.CreateTestReadableGrid(new string[0, 0]);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(readableGrid);

        // Then no matches are found
        Assert.That(matches, Is.Empty, "Empty grid should contain no horizontal matches");
    }

    [Test]
    public void GivenAGridWithNoConsecutiveElements_WhenPlayerLooksForHorizontalMatches_ThenNoMatchesAreFound()
    {
        // Given a grid where no three consecutive elements are the same horizontally
        var grid = new[,]
        {
            { "A", "B", "A", "C", "C", "A", "B", "C" },
            { "B", "A", "B", "A", "B", "C", "A", "B" },
            { "C", "C", "A", "B", "A", "B", "C", "A" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then no matches are found because no row has three consecutive identical elements
        Assert.That(matches, Is.Empty, "Grid with no consecutive identical elements should have no horizontal matches");
    }

    [Test]
    public void GivenAGridWithEmptyCellsBreakingPotentialMatches_WhenPlayerLooksForHorizontalMatches_ThenNoMatchesAreFound()
    {
        // Given a grid where empty cells break what would otherwise be valid matches
        var grid = new[,]
        {
            { "A", "A", null, "A", "A", "B", "C", "D" }, // AA-AA broken by empty cell
            { "B", "C", "C", null, "C", "C", "D", "E" }, // CC-CC broken by empty cell
            { "C", "D", "E", "F", "G", "H", "I", "J" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then no matches are found because empty cells break the continuity
        Assert.That(matches, Is.Empty, "Empty cells should break potential horizontal matches");
    }

    #endregion

    #region Scenarios Where Single Horizontal Matches Are Found

    [Test]
    public void GivenAGridWithOneMinimalHorizontalMatch_WhenPlayerLooksForHorizontalMatches_ThenOneMatchIsFound()
    {
        // Given a grid with exactly three consecutive identical elements in one row
        var grid = new[,]
        {
            { "A", "B", "C", "D", "E", "F", "G", "H" },
            { "B", "A", "A", "A", "B", "C", "D", "E" }, // AAA horizontal match
            { "C", "D", "E", "F", "G", "H", "A", "B" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then exactly one match is found with the correct positions
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one horizontal match");
    }

    [Test]
    public void GivenAGridWithOneExtendedHorizontalMatch_WhenPlayerLooksForHorizontalMatches_ThenOneLongerMatchIsFound()
    {
        // Given a grid with more than three consecutive identical elements in one row
        var grid = new[,]
        {
            { "A", "B", "C", "D", "E", "F", "G", "H" },
            { "B", "A", "A", "A", "A", "A", "D", "E" }, // AAAAA horizontal match (5 elements)
            { "C", "D", "E", "F", "G", "H", "A", "B" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then exactly one extended match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one extended horizontal match");
        Assert.That(matches[0].Count, Is.EqualTo(5), "Extended match should include all five consecutive elements");
    }

    [Test]
    public void GivenAGridWithAMatchAfterEmptySpaces_WhenPlayerLooksForHorizontalMatches_ThenTheMatchIsFound()
    {
        // Given a grid where a valid match occurs after some empty cells
        var grid = new[,]
        {
            { null, null, "A", "A", "A", "B", "C", "D" }, // AAA match after empty cells
            { "B", "C", "D", "E", "F", "G", "H", "I" },
            { "C", "D", "E", "F", "G", "H", "I", "J" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then the match is found despite the preceding empty cells
        Assert.That(matches, Has.Count.EqualTo(1), "Should find horizontal match even after empty cells");
        Assert.That(matches[0].Count, Is.EqualTo(3), "Match should include exactly three elements");
    }

    [Test]
    public void GivenAGridWithAMatchBeforeEmptySpaces_WhenPlayerLooksForHorizontalMatches_ThenTheMatchIsFound()
    {
        // Given a grid where a valid match occurs before some empty cells
        var grid = new[,]
        {
            { "A", "A", "A", null, null, "B", "C", "D" }, // AAA match before empty cells
            { "B", "C", "D", "E", "F", "G", "H", "I" },
            { "C", "D", "E", "F", "G", "H", "I", "J" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then the match is found despite the following empty cells
        Assert.That(matches, Has.Count.EqualTo(1), "Should find horizontal match even before empty cells");
        Assert.That(matches[0].Count, Is.EqualTo(3), "Match should include exactly three elements");
    }

    #endregion

    #region Scenarios Where Multiple Horizontal Matches Are Found

    [Test]
    public void GivenAGridWithMultipleMatchesInTheSameRow_WhenPlayerLooksForHorizontalMatches_ThenAllMatchesAreFound()
    {
        // Given a grid with two separate horizontal matches in the same row
        var grid = new[,]
        {
            { "A", "B", "C", "D", "E", "F", "G", "H" },
            { "A", "A", "A", "B", "C", "C", "C", "E" }, // AAA match and CCC match in same row
            { "C", "D", "E", "F", "G", "H", "A", "B" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then both matches are found
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both horizontal matches in the same row");

        var firstMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 0));
        var secondMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 4));

        Assert.That(firstMatch.Count, Is.EqualTo(3), "First match should have three elements");
        Assert.That(secondMatch.Count, Is.EqualTo(3), "Second match should have three elements");
    }

    [Test]
    public void GivenAGridWithMatchesInDifferentRows_WhenPlayerLooksForHorizontalMatches_ThenAllMatchesAreFound()
    {
        // Given a grid with horizontal matches in different rows
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // AAA match in first row
            { "B", "C", "D", "E", "F", "G", "H", "I" },
            { "C", "D", "B", "B", "B", "E", "F", "G" }, // BBB match in third row
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then both matches are found across different rows
        Assert.That(matches, Has.Count.EqualTo(2), "Should find horizontal matches in different rows");

        var firstRowMatch = matches.First(m => m.Any(cell => cell.RowIndex == 0));
        var thirdRowMatch = matches.First(m => m.Any(cell => cell.RowIndex == 2));

        Assert.That(firstRowMatch.Count, Is.EqualTo(3), "First row match should have three elements");
        Assert.That(thirdRowMatch.Count, Is.EqualTo(3), "Third row match should have three elements");
    }

    [Test]
    public void GivenAGridWithMatchesSeparatedByEmptySpaces_WhenPlayerLooksForHorizontalMatches_ThenBothMatchesAreFound()
    {
        // Given a grid with two matches in the same row separated by empty cells
        var grid = new[,]
        {
            { "A", "A", "A", null, null, "A", "A", "A" }, // AAA at start and AAA at end, separated by empty cells
            { "B", "C", "D", "E", "F", "G", "H", "I" },
            { "C", "D", "E", "F", "G", "H", "I", "J" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then both matches are found despite the empty cells between them
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both matches separated by empty cells");

        var firstMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 0));
        var secondMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 5));

        Assert.That(firstMatch.Count, Is.EqualTo(3), "First match should have three elements");
        Assert.That(secondMatch.Count, Is.EqualTo(3), "Second match should have three elements");
    }

    [Test]
    public void GivenAGridWithMultipleMatchesOfDifferentElements_WhenPlayerLooksForHorizontalMatches_ThenAllDifferentMatchesAreFound()
    {
        // Given a grid with horizontal matches of different element types
        var grid = new[,]
        {
            { "A", "A", "A", "B", "B", "B", "C", "D" }, // AAA and BBB matches in same row
            { "E", "F", "G", "H", "I", "J", "K", "L" },
            { "C", "C", "C", "M", "N", "D", "D", "D" }, // CCC and DDD matches in same row
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then all four different matches are found
        Assert.That(matches, Has.Count.EqualTo(4), "Should find all horizontal matches of different element types");
        Assert.That(matches.All(m => m.Count == 3), Is.True, "All matches should have exactly three elements");
    }

    [Test]
    public void GivenAGridWithAdjacentMatchesOfDifferentElements_WhenPlayerLooksForHorizontalMatches_ThenBothMatchesAreFoundSeparately()
    {
        // Given a grid where different element matches are adjacent to each other
        var grid = new[,]
        {
            { "X", "Y", "Z", "W", "V", "U", "T", "S" },
            { "A", "A", "A", "B", "B", "B", "R", "Q" }, // AAA immediately followed by BBB
            { "P", "O", "N", "M", "L", "K", "J", "I" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for horizontal matches
        var matches = _strategy.FindMatches(cells);

        // Then both adjacent matches are found as separate matches
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both adjacent matches separately");

        var aMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 0));
        var bMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 3));

        Assert.That(aMatch.Count, Is.EqualTo(3), "A match should have three elements");
        Assert.That(bMatch.Count, Is.EqualTo(3), "B match should have three elements");
    }

    #endregion
}