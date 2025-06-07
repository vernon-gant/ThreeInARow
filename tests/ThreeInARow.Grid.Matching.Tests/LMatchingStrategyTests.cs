using ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Grid.Matching.Tests;

public class LMatchingStrategyTests : MGridTestUtility
{
    private readonly HorizontalMatchingStrategy<string> _horizontalStrategy = new(3);
    private readonly VerticalMatchingStrategy<string> _verticalStrategy = new(3);
    private readonly LMatchingStrategy<string> _strategy;

    public LMatchingStrategyTests()
    {
        _strategy = new LMatchingStrategy<string>(3, _horizontalStrategy, _verticalStrategy);
    }

    #region Scenarios Where No L-Shaped Patterns Are Found

    [Test]
    public void GivenAGridWithNoMatchingElements_WhenPlayerLooksForLShapedPatterns_ThenNoLMatchesAreFound()
    {
        // Given a grid with alternating elements that create no matches
        var grid = new[,]
        {
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" },
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no L-shaped matches are found
        Assert.That(matches, Is.Empty, "No L-shaped patterns should be detected in alternating grid");
    }

    [Test]
    public void GivenSeparateHorizontalAndVerticalMatches_WhenPlayerLooksForLShapedPatterns_ThenNoLMatchesAreFound()
    {
        // Given a grid with separate horizontal and vertical matches that don't form corner intersections
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match (separate)
            { "G", "H", "I", "B", "J", "K", "L", "M" },
            { "N", "O", "P", "B", "Q", "R", "S", "T" },
            { "U", "V", "W", "B", "X", "Y", "Z", null }, // Vertical match (separate)
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no L-shaped matches are found because the matches don't form corner intersections
        Assert.That(matches, Is.Empty, "Separate non-intersecting matches should not form L-shaped patterns");
    }

    [Test]
    public void GivenATShapedPattern_WhenPlayerLooksForLShapedPatterns_ThenNoLMatchesAreFound()
    {
        // Given a grid with a T-shaped pattern (middle intersection, not corner)
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal line
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical stem (middle intersection)
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no L-shaped matches are found because T-shapes have middle intersections, not corner intersections
        Assert.That(matches, Is.Empty, "T-shaped patterns should not be recognized as L-shaped matches");
    }

    [Test]
    public void GivenACrossShapedPattern_WhenPlayerLooksForLShapedPatterns_ThenNoLMatchesAreFound()
    {
        // Given a grid with a cross-shaped pattern (middle intersections)
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical above center
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal through center
            { "S", "A", "T", "U", "V", "W", "X", "Y" }, // Vertical below center
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no L-shaped matches are found because cross shapes have middle intersections, not corner intersections
        Assert.That(matches, Is.Empty, "Cross-shaped patterns should not be recognized as L-shaped matches");
    }

    [Test]
    public void GivenAnLShapePatternBrokenByEmptySpaces_WhenPlayerLooksForLShapedPatterns_ThenNoLMatchesAreFound()
    {
        // Given a grid where empty cells break what would be an L-shaped pattern
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal part of potential L
            { null, "G", "H", "I", "J", "K", "L", "M" }, // Empty cell breaks vertical part
            { "A", "N", "O", "P", "Q", "R", "S", "T" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no L-shaped matches are found because empty spaces break the pattern
        Assert.That(matches, Is.Empty, "L-shaped patterns broken by empty cells should not be detected");
    }

    [Test]
    public void GivenAnLShapeWithArmsTooShort_WhenPlayerLooksForLShapedPatterns_ThenNoLMatchesAreFound()
    {
        // Given a grid where both arms of the potential L are shorter than the minimum required length
        var grid = new[,]
        {
            { "A", "A", "B", "C", "D", "E", "F", "G" }, // Only 2 elements horizontally (below minimum)
            { "A", "H", "I", "J", "K", "L", "M", "N" }, // Only 2 elements vertically (below minimum)
            { "O", "P", "Q", "R", "S", "T", "U", "V" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no L-shaped matches are found because both arms are too short
        Assert.That(matches, Is.Empty, "L-shaped patterns with arms shorter than minimum should not be detected");
    }

    #endregion

    #region Scenarios Where L-Shaped Patterns Are Successfully Found

    [Test]
    public void GivenAnLShapeWithTopLeftCorner_WhenPlayerLooksForLShapedPatterns_ThenOneLMatchIsFound()
    {
        // Given a grid with an L-shaped pattern where the corner is at the top-left
        // A A A  (horizontal arm extending right)
        // A      (vertical arm extending down)
        // A
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal arm
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical arm down
            { "A", "N", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one L-shaped match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one L-shaped pattern with top-left corner");
        Assert.That(matches[0].Count, Is.EqualTo(5), "L-shape should contain 5 elements (3 horizontal + 3 vertical - 1 corner)");
    }

    [Test]
    public void GivenAnLShapeWithTopRightCorner_WhenPlayerLooksForLShapedPatterns_ThenOneLMatchIsFound()
    {
        // Given a grid with an L-shaped pattern where the corner is at the top-right
        //   A A A  (horizontal arm extending left)
        //     A    (vertical arm extending down)
        //     A
        var grid = new[,]
        {
            { "B", "A", "A", "A", "C", "D", "E", "F" }, // Horizontal arm
            { "G", "H", "I", "A", "J", "K", "L", "M" }, // Vertical arm down
            { "N", "O", "P", "A", "Q", "R", "S", "T" }, // Vertical continues
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one L-shaped match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one L-shaped pattern with top-right corner");
        Assert.That(matches[0].Count, Is.EqualTo(5), "L-shape should contain 5 elements (3 horizontal + 3 vertical - 1 corner)");
    }

    [Test]
    public void GivenAnLShapeWithBottomLeftCorner_WhenPlayerLooksForLShapedPatterns_ThenOneLMatchIsFound()
    {
        // Given a grid with an L-shaped pattern where the corner is at the bottom-left
        // A      (vertical arm extending up)
        // A
        // A A A  (horizontal arm extending right)
        var grid = new[,]
        {
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical arm up
            { "A", "N", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal arm
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one L-shaped match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one L-shaped pattern with bottom-left corner");
        Assert.That(matches[0].Count, Is.EqualTo(5), "L-shape should contain 5 elements (3 horizontal + 3 vertical - 1 corner)");
    }

    [Test]
    public void GivenAnLShapeWithBottomRightCorner_WhenPlayerLooksForLShapedPatterns_ThenOneLMatchIsFound()
    {
        // Given a grid with an L-shaped pattern where the corner is at the bottom-right
        //     A    (vertical arm extending up)
        //     A
        // A A A    (horizontal arm extending left)
        var grid = new[,]
        {
            { "G", "H", "I", "A", "J", "K", "L", "M" }, // Vertical arm up
            { "N", "O", "P", "A", "Q", "R", "S", "T" }, // Vertical continues
            { "B", "A", "A", "A", "C", "D", "E", "F" }, // Horizontal arm
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one L-shaped match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one L-shaped pattern with bottom-right corner");
        Assert.That(matches[0].Count, Is.EqualTo(5), "L-shape should contain 5 elements (3 horizontal + 3 vertical - 1 corner)");
    }

    [Test]
    public void GivenLShapesOfDifferentSizes_WhenPlayerLooksForLShapedPatterns_ThenAllLMatchesAreFoundWithCorrectSizes()
    {
        // Given a grid with two L-shaped patterns of different sizes
        var grid = new[,]
        {
            { "A", "A", "A", "A", "B", "B", "B", "C" }, // 4-element and 3-element horizontal arms
            { "A", "E", "F", "G", "H", "I", "B", "J" }, // Vertical arms extending down
            { "A", "K", "L", "M", "N", "O", "B", "P" }, // Vertical continues
            { "A", "Q", "R", "S", "T", "U", "B", "V" }, // First L has longer vertical arm
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then both L-shaped matches are found with their correct sizes
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both L-shaped patterns of different sizes");

        var firstMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 0));
        var secondMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 6));

        Assert.That(firstMatch.Count, Is.EqualTo(7), "First L-shape should contain 7 elements (4 horizontal + 4 vertical - 1 corner)");
        Assert.That(secondMatch.Count, Is.EqualTo(6), "Second L-shape should contain 6 elements (3 horizontal + 4 vertical - 1 corner)");
    }

    [Test]
    public void GivenMultipleLShapesOfTheSameSize_WhenPlayerLooksForLShapedPatterns_ThenAllLMatchesAreFound()
    {
        // Given a grid with multiple identical L-shaped patterns
        var grid = new[,]
        {
            { "A", "A", "A", "B", "B", "B", "C", "C" }, // Three horizontal arms
            { "A", "D", "E", "B", "F", "G", "C", "H" }, // Three vertical arms
            { "A", "I", "J", "B", "K", "L", "C", "M" }, // All same size L-shapes
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then all identical L-shaped matches are found
        Assert.That(matches, Has.Count.EqualTo(2), "Should find multiple L-shaped patterns of the same size");
        Assert.That(matches.All(m => m.Count == 5), Is.True, "All L-shapes should contain 5 elements (3 horizontal + 3 vertical - 1 corner)");
    }

    [Test]
    public void GivenAMinimalLShapedPattern_WhenPlayerLooksForLShapedPatterns_ThenTheMinimalLMatchIsFound()
    {
        // Given a grid with the smallest possible L-shaped pattern (3x3)
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Exactly 3 elements horizontally
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Exactly 3 elements vertically
            { "A", "N", "O", "P", "Q", "R", "S", "T" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then the minimal L-shaped match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find the minimal L-shaped pattern");
        Assert.That(matches[0].Count, Is.EqualTo(5), "Minimal L-shape should contain exactly 5 elements");
    }

    [Test]
    public void GivenLShapesWithMixedOrientations_WhenPlayerLooksForLShapedPatterns_ThenAllDifferentOrientationsAreFound()
    {
        // Given a grid with L-shaped patterns in different orientations
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "C", "C", "C" }, // Top-left L and top-right L
            { "A", "D", "E", "F", "G", "H", "I", "C" }, // Vertical arms in different directions
            { "A", "J", "K", "L", "M", "N", "O", "C" }, // Different arm lengths
            { "P", "Q", "R", "S", "T", "U", "V", "W" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then all L-shaped patterns with different orientations are found
        Assert.That(matches, Has.Count.EqualTo(2), "Should find L-shaped patterns in different orientations");

        var leftL = matches.First(m => m.Any(cell => cell.ColumnIndex == 0));
        var rightL = matches.First(m => m.Any(cell => cell.ColumnIndex == 7));

        Assert.That(leftL.Count, Is.EqualTo(5), "Left L-shape should contain 5 elements (3 horizontal + 3 vertical - 1 corner)");
        Assert.That(rightL.Count, Is.EqualTo(6), "Right L-shape should contain 6 elements (4 horizontal + 3 vertical - 1 corner)");
    }

    [Test]
    public void GivenAnLShapeAtGridEdge_WhenPlayerLooksForLShapedPatterns_ThenTheEdgeLMatchIsFound()
    {
        // Given a grid with an L-shaped pattern positioned at the edge
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // L-shape at top edge
            { "A", "G", "H", "I", "J", "K", "L", "M" },
            { "A", "N", "O", "P", "Q", "R", "S", "T" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When the player looks for L-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then the edge L-shaped match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find L-shaped pattern even when positioned at grid edge");
        Assert.That(matches[0].Count, Is.EqualTo(5), "Edge L-shape should contain 5 elements like any minimal L-shape");
    }

    #endregion
}