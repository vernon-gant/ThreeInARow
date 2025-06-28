using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Grid.Matching.Tests.Strategy;

public class CrossMatchingStrategyTests : MGridTestUtility
{
    private readonly HorizontalMatchingStrategy<string> _horizontalStrategy = new(3);
    private readonly VerticalMatchingStrategy<string> _verticalStrategy = new(3);
    private readonly CrossMatchingStrategy<string> _strategy;

    public CrossMatchingStrategyTests()
    {
        _strategy = new CrossMatchingStrategy<string>(3, _horizontalStrategy, _verticalStrategy);
    }

    #region Scenarios Where No Cross Patterns Are Found

    [Test]
    public void GivenAGridWithNoMatchingElements_WhenPlayerLooksForCrossPatterns_ThenNoCrossMatchesAreFound()
    {
        // Given a grid with alternating elements that create no matches
        var grid = new[,]
        {
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" },
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then no cross matches are found
        Assert.That(matches, Is.Empty, "No cross patterns should be detected in alternating grid");
    }

    [Test]
    public void GivenSeparateHorizontalAndVerticalMatches_WhenPlayerLooksForCrossPatterns_ThenNoCrossMatchesAreFound()
    {
        // Given a grid with separate horizontal and vertical matches that don't intersect
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match (separate)
            { "G", "H", "I", "B", "J", "K", "L", "M" },
            { "N", "O", "P", "B", "Q", "R", "S", "T" },
            { "U", "V", "W", "B", "X", "Y", "Z", null }, // Vertical match (separate)
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then no cross matches are found because the matches don't intersect
        Assert.That(matches, Is.Empty, "Separate non-intersecting matches should not form cross patterns");
    }

    [Test]
    public void GivenATShapedPattern_WhenPlayerLooksForCrossPatterns_ThenNoCrossMatchesAreFound()
    {
        // Given a grid with a T-shaped pattern (missing one arm of the cross)
        //   A
        // A A A
        //   (missing bottom arm)
        var grid = new[,]
        {
            { "B", "A", "C", "D", "E", "F", "G", "H" }, // Vertical top arm
            { "A", "A", "A", "I", "J", "K", "L", "M" }, // Horizontal middle arm
            { "N", "O", "P", "Q", "R", "S", "T", "U" }, // No vertical bottom arm
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then no cross matches are found because T-shapes are incomplete crosses
        Assert.That(matches, Is.Empty, "T-shaped patterns should not be recognized as cross matches");
    }

    [Test]
    public void GivenAnLShapedPattern_WhenPlayerLooksForCrossPatterns_ThenNoCrossMatchesAreFound()
    {
        // Given a grid with an L-shaped pattern (missing two arms of the cross)
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal part of L
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical part starts here
            { "A", "N", "O", "P", "Q", "R", "S", "T" }, // Vertical part continues
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then no cross matches are found because L-shapes are incomplete crosses
        Assert.That(matches, Is.Empty, "L-shaped patterns should not be recognized as cross matches");
    }

    [Test]
    public void GivenACrossPatternBrokenByEmptySpaces_WhenPlayerLooksForCrossPatterns_ThenNoCrossMatchesAreFound()
    {
        // Given a grid where empty cells break what would be a cross pattern
        //   A
        // A A A
        //   ∅ (empty cell breaks the bottom arm)
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical top arm
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal middle arm
            { "S", null, "T", "U", "V", "W", "X", "Y" }, // Empty cell breaks vertical bottom
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then no cross matches are found because empty spaces break the pattern
        Assert.That(matches, Is.Empty, "Cross patterns broken by empty cells should not be detected");
    }

    [Test]
    public void GivenAnIncompleteCrossWithOnlyThreeArms_WhenPlayerLooksForCrossPatterns_ThenNoCrossMatchesAreFound()
    {
        // Given a grid with only three arms of what would be a cross
        //   A
        // A A (missing right arm)
        //   A
        var grid = new[,]
        {
            { "B", "A", "C", "D", "E", "F", "G", "H" }, // Vertical top arm
            { "A", "A", "I", "J", "K", "L", "M", "N" }, // Horizontal left arm only
            { "O", "A", "P", "Q", "R", "S", "T", "U" }, // Vertical bottom arm
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then no cross matches are found because all four arms are required
        Assert.That(matches, Is.Empty, "Incomplete crosses with only three arms should not be detected");
    }

    #endregion

    #region Scenarios Where Cross Patterns Are Successfully Found

    [Test]
    public void GivenAMinimalCrossPattern_WhenPlayerLooksForCrossPatterns_ThenOneCrossMatchIsFound()
    {
        // Given a grid with the smallest possible cross pattern (3x3)
        //   A
        // A A A
        //   A
        var grid = new[,]
        {
            { "B", "A", "C", "D", "E", "F", "G", "H" }, // Vertical top arm
            { "A", "A", "A", "I", "J", "K", "L", "M" }, // Horizontal middle arm
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical bottom arm
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one cross match is found with the correct number of elements
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one cross pattern");
        Assert.That(matches[0].Count, Is.EqualTo(5), "Minimal cross should contain 5 elements (3 horizontal + 3 vertical - 1 center)");
        Assert.That(matches[0], Is.InstanceOf<CrossMatch<string>>(), "Match should be of type CrossMatch<string>");
    }

    [Test]
    public void GivenALargeSymmetricCrossPattern_WhenPlayerLooksForCrossPatterns_ThenOneCrossMatchIsFound()
    {
        // Given a grid with a large symmetric cross (5 horizontal, 5 vertical)
        //     A
        //     A
        // A A A A A
        //     A
        //     A
        var grid = new[,]
        {
            { "I", "J", "A", "K", "L", "M", "N", "O" }, // Vertical top arm
            { "P", "Q", "A", "R", "S", "T", "U", "V" }, // Vertical continues
            { "A", "A", "A", "A", "A", "W", "X", "Y" }, // 5-element horizontal arm
            { "Z", ".", "A", "B", "C", "D", "E", "F" }, // Vertical bottom arm
            { "G", "H", "A", "I", "J", "K", "L", "M" }, // Vertical continues
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one large cross match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one large cross pattern");
        Assert.That(matches[0].Count, Is.EqualTo(9), "Large symmetric cross should contain 9 elements (5 + 5 - 1 center)");
        Assert.That(matches[0], Is.InstanceOf<CrossMatch<string>>(), "Match should be of type CrossMatch<string>");
    }

    [Test]
    public void GivenAnAsymmetricCrossPattern_WhenPlayerLooksForCrossPatterns_ThenOneCrossMatchIsFound()
    {
        // Given a grid with an asymmetric cross (4 horizontal, 3 vertical)
        //   A
        // A A A A
        //   A
        var grid = new[,]
        {
            { "B", "A", "C", "D", "E", "F", "G", "H" }, // Vertical top arm
            { "A", "A", "A", "A", "I", "J", "K", "L" }, // 4-element horizontal arm
            { "M", "A", "N", "O", "P", "Q", "R", "S" }, // Vertical bottom arm
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one asymmetric cross match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one asymmetric cross pattern");
        Assert.That(matches[0].Count, Is.EqualTo(6), "Asymmetric cross should contain 6 elements (4 + 3 - 1 center)");
        Assert.That(matches[0], Is.InstanceOf<CrossMatch<string>>(), "Match should be of type CrossMatch<string>");
    }

    [Test]
    public void GivenMultipleSeparateCrossPatterns_WhenPlayerLooksForCrossPatterns_ThenAllCrossMatchesAreFound()
    {
        // Given a grid with two separate cross patterns of the same element type
        //   A       B
        // A A A   B B B
        //   A       B
        var grid = new[,]
        {
            { "C", "A", "D", "E", "B", "F", "G", "H" }, // Two vertical top arms
            { "A", "A", "A", "B", "B", "B", "Z", "J" }, // Two horizontal middle arms
            { "K", "A", "L", "M", "B", "N", "O", "P" }, // Two vertical bottom arms
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then both cross matches are found
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both separate cross patterns");
        Assert.That(matches.All(m => m.Count == 5), Is.True, "Both crosses should be minimal 3x3 patterns with 5 elements each");
        Assert.That(matches.All(m => m is CrossMatch<string>), Is.True, "All matches should be of type CrossMatch<string>");
    }

    [Test]
    public void GivenACrossWithExtendedArms_WhenPlayerLooksForCrossPatterns_ThenOneLargeCrossMatchIsFound()
    {
        // Given a grid with a cross having longer arms (6 horizontal, 4 vertical)
        //     A
        //     A
        // A A A A A A
        //     A
        var grid = new[,]
        {
            { "I", "J", "A", "K", "L", "M", "N", "O" }, // Vertical top arm
            { "P", "Q", "A", "R", "S", "T", "U", "V" }, // Vertical continues
            { "A", "A", "A", "A", "A", "A", "W", "X" }, // 6-element horizontal arm
            { "Y", "Z", "A", ".", "B", "C", "D", "E" }, // Vertical bottom arm
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one extended cross match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one extended cross pattern");
        Assert.That(matches[0].Count, Is.EqualTo(9), "Extended cross should contain 9 elements (6 + 4 - 1 center)");
        Assert.That(matches[0], Is.InstanceOf<CrossMatch<string>>(), "Match should be of type CrossMatch<string>");
    }

    [Test]
    public void GivenMultipleCrossesOfDifferentSizes_WhenPlayerLooksForCrossPatterns_ThenAllCrossMatchesAreFound()
    {
        // Given a grid with two crosses of different sizes
        //   A         B
        // A A A   B B B B
        //   A         B
        //   A
        var grid = new[,]
        {
            { "C", "A", "D", "E", "B", "F", "G", "H" }, // Different vertical top arms
            { "A", "A", "A", "B", "B", "B", "B", "B" }, // Different horizontal arm sizes
            { "J", "A", "K", "L", "B", "M", "N", "O" }, // Different vertical bottom arms
            { "P", "A", "Q", "R", "S", "T", "U", "V" }, // First cross has longer vertical
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for cross patterns
        var matches = _strategy.FindMatches(cells);

        // Then both differently-sized cross matches are found
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both differently-sized cross patterns");
        Assert.That(matches.All(m => m is CrossMatch<string>), Is.True, "All matches should be of type CrossMatch<string>");
    }

    #endregion
}