using ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Grid.Matching.Tests.Strategy;

public class TMatchingStrategyTests : MGridTestUtility
{
    private readonly HorizontalMatchingStrategy<string> _horizontalStrategy = new(3);
    private readonly VerticalMatchingStrategy<string> _verticalStrategy = new(3);
    private readonly TMatchingStrategy<string> _strategy;

    public TMatchingStrategyTests()
    {
        _strategy = new TMatchingStrategy<string>(3, _horizontalStrategy, _verticalStrategy);
    }

    #region Scenarios Where No T-Shaped Patterns Are Found

    [Test]
    public void GivenAGridWithNoMatchingElements_WhenPlayerLooksForTShapedPatterns_ThenNoTMatchesAreFound()
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

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no T-shaped matches are found
        Assert.That(matches, Is.Empty, "No T-shaped patterns should be detected in alternating grid");
    }

    [Test]
    public void GivenSeparateHorizontalAndVerticalMatches_WhenPlayerLooksForTShapedPatterns_ThenNoTMatchesAreFound()
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

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no T-shaped matches are found because the matches don't intersect properly
        Assert.That(matches, Is.Empty, "Separate non-intersecting matches should not form T-shaped patterns");
    }

    [Test]
    public void GivenAnLShapedPattern_WhenPlayerLooksForTShapedPatterns_ThenNoTMatchesAreFound()
    {
        // Given a grid with an L-shaped pattern (corner intersection, not middle intersection)
        //   A A A  (horizontal arm)
        //   A      (vertical arm - corner intersection)
        //   A
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical match - corner intersection
            { "A", "N", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
            { "U", "V", "W", "X", "Y", "Z", null, null },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no T-shaped matches are found because L-shapes have corner intersections, not middle intersections
        Assert.That(matches, Is.Empty, "L-shaped patterns should not be recognized as T-shaped matches");
    }

    [Test]
    public void GivenACrossShapedPattern_WhenPlayerLooksForTShapedPatterns_ThenNoTMatchesAreFound()
    {
        // Given a grid with a cross-shaped pattern (intersection in middle of both lines)
        //     A      (vertical arm above)
        //   A A A    (horizontal arm through center)
        //     A      (vertical arm below)
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical part above center
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal through center
            { "S", "A", "T", "U", "V", "W", "X", "Y" }, // Vertical part below center
            { "Z", null, null, null, null, null, null, null },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no T-shaped matches are found because crosses have stems on both sides, not just one
        Assert.That(matches, Is.Empty, "Cross-shaped patterns should not be recognized as T-shaped matches");
    }

    [Test]
    public void GivenATShapePatternBrokenByEmptySpaces_WhenPlayerLooksForTShapedPatterns_ThenNoTMatchesAreFound()
    {
        // Given a grid where empty cells break what would be a T-shaped pattern
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match
            { "G", null, "H", "I", "J", "K", "L", "M" }, // Empty cell breaks vertical stem
            { "N", "A", "O", "P", "Q", "R", "S", "T" },
            { "U", "V", "W", "X", "Y", "Z", null, null },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no T-shaped matches are found because empty spaces break the pattern
        Assert.That(matches, Is.Empty, "T-shaped patterns broken by empty cells should not be detected");
    }

    [Test]
    public void GivenATShapeWithInsufficientArmLength_WhenPlayerLooksForTShapedPatterns_ThenNoTMatchesAreFound()
    {
        // Given a grid with T-like pattern but arms shorter than minimum required
        var grid = new[,]
        {
            { "A", "A", "B", "C", "D", "E", "F", "G" }, // Only 2 elements horizontally (below minimum)
            { "H", "A", "I", "J", "K", "L", "M", "N" }, // Only 2 elements in stem (below minimum)
            { "O", "P", "Q", "R", "S", "T", "U", "V" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then no T-shaped matches are found because arms are too short
        Assert.That(matches, Is.Empty, "T-shaped patterns with arms shorter than minimum should not be detected");
    }

    #endregion

    #region Scenarios Where T-Shaped Patterns Are Successfully Found

    [Test]
    public void GivenATShapePointingDownward_WhenPlayerLooksForTShapedPatterns_ThenOneTMatchIsFound()
    {
        // Given a grid with a T-shaped pattern pointing downward ⊤
        // A A A  (horizontal top arm)
        //   A    (vertical stem pointing down)
        //   A
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal top arm
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical stem down
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
            { "U", "V", "W", "X", "Y", "Z", null, null },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one T-shaped match pointing downward is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one T-shaped pattern pointing downward");
        Assert.That(matches[0].Count, Is.EqualTo(5), "T-shape should contain 5 elements (3 horizontal + 3 vertical - 1 intersection)");
    }

    [Test]
    public void GivenATShapePointingUpward_WhenPlayerLooksForTShapedPatterns_ThenOneTMatchIsFound()
    {
        // Given a grid with a T-shaped pattern pointing upward ⊥
        //   A    (vertical stem pointing up)
        //   A
        // A A A  (horizontal bottom arm)
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical stem up
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal bottom arm
            { "U", "V", "W", "X", "Y", "Z", null, null },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one T-shaped match pointing upward is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one T-shaped pattern pointing upward");
        Assert.That(matches[0].Count, Is.EqualTo(5), "T-shape should contain 5 elements (3 horizontal + 3 vertical - 1 intersection)");
    }

    [Test]
    public void GivenATShapePointingRightward_WhenPlayerLooksForTShapedPatterns_ThenOneTMatchIsFound()
    {
        // Given a grid with a T-shaped pattern pointing rightward ⊣
        // A      (vertical left arm)
        // A A A  (horizontal stem pointing right)
        // A      (vertical continues)
        var grid = new[,]
        {
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical left arm
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal stem right
            { "A", "S", "T", "U", "V", "W", "X", "Y" }, // Vertical continues
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one T-shaped match pointing rightward is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one T-shaped pattern pointing rightward");
        Assert.That(matches[0].Count, Is.EqualTo(5), "T-shape should contain 5 elements (3 horizontal + 3 vertical - 1 intersection)");
    }

    [Test]
    public void GivenATShapePointingLeftward_WhenPlayerLooksForTShapedPatterns_ThenOneTMatchIsFound()
    {
        // Given a grid with a T-shaped pattern pointing leftward ⊢
        //     A  (vertical right arm)
        // A A A  (horizontal stem pointing left)
        //     A  (vertical continues)
        var grid = new[,]
        {
            { "G", "H", "I", "A", "J", "K", "L", "M" }, // Vertical right arm
            { "N", "A", "A", "A", "O", "P", "Q", "R" }, // Horizontal stem left
            { "S", "T", "U", "A", "V", "W", "X", "Y" }, // Vertical continues
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one T-shaped match pointing leftward is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one T-shaped pattern pointing leftward");
        Assert.That(matches[0].Count, Is.EqualTo(5), "T-shape should contain 5 elements (3 horizontal + 3 vertical - 1 intersection)");
    }

    [Test]
    public void GivenALargerTShapedPattern_WhenPlayerLooksForTShapedPatterns_ThenOneExtendedTMatchIsFound()
    {
        // Given a grid with a larger T-shaped pattern (5-element horizontal, 4-element vertical)
        // A A A A A  (5-element horizontal top arm)
        //     A      (4-element vertical stem down)
        //     A
        //     A
        var grid = new[,]
        {
            { "A", "A", "A", "A", "A", "B", "C", "D" }, // 5-element horizontal arm
            { "G", "H", "Y", "A", "J", "K", "L", "M" }, // Vertical down from middle
            { "N", "O", "Z", "A", "Q", "R", "S", "T" }, // Vertical continues
            { "U", "V", "B", "A", "Y", "Z", null, null }, // Vertical continues
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then exactly one extended T-shaped match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find exactly one extended T-shaped pattern");
        Assert.That(matches[0].Count, Is.EqualTo(8), "Extended T-shape should contain 8 elements (5 horizontal + 4 vertical - 1 intersection)");
    }

    [Test]
    public void GivenMultipleSeparateTShapedPatterns_WhenPlayerLooksForTShapedPatterns_ThenAllTMatchesAreFound()
    {
        // Given a grid with two separate T-shaped patterns
        // A A A    C C C  (two horizontal arms)
        //   A        C    (two vertical stems)
        //   A        C
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "C", "C", "D" }, // Two horizontal matches
            { "E", "A", "F", "G", "H", "C", "I", "J" }, // Two vertical stems
            { "K", "A", "M", "N", "O", "C", "Q", "R" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then both T-shaped matches are found
        Assert.That(matches, Has.Count.EqualTo(2), "Should find both separate T-shaped patterns");
        Assert.That(matches.All(m => m.Count == 5), Is.True, "Both T-shapes should contain 5 elements each");
    }

    [Test]
    public void GivenTShapesWithDifferentOrientations_WhenPlayerLooksForTShapedPatterns_ThenAllOrientationsAreFound()
    {
        // Given a grid with T-shaped patterns in different orientations
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Downward T horizontal arm
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Downward T vertical stem
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Downward T continues
            { "U", "V", "W", "X", "Y", "Z", ".", "!" },
            { "Y", "Y", "Y", "Z", ".", "!", "@", "#" }, // Upward T horizontal arm
            { "$", "Y", "%", "^", "&", "*", "(", ")" }, // Upward T vertical stem
            { "-", "Y", "+", "=", "[", "]", "{", "}" }, // Upward T continues
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then T-shaped patterns in different orientations are found
        Assert.That(matches, Has.Count.EqualTo(2), "Should find T-shaped patterns in different orientations");
        Assert.That(matches.All(m => m.Count == 5), Is.True, "All T-shapes should contain 5 elements each");
    }

    [Test]
    public void GivenAMinimalTShapedPattern_WhenPlayerLooksForTShapedPatterns_ThenTheMinimalTMatchIsFound()
    {
        // Given a grid with the smallest possible T-shaped pattern (3x3 minimum)
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Exactly 3 elements horizontally
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Exactly 3 elements vertically
            { "N", "A", "O", "P", "Q", "R", "S", "T" },
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then the minimal T-shaped match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find the minimal T-shaped pattern");
        Assert.That(matches[0].Count, Is.EqualTo(5), "Minimal T-shape should contain exactly 5 elements");
    }

    [Test]
    public void GivenATShapeAtGridEdge_WhenPlayerLooksForTShapedPatterns_ThenTheEdgeTMatchIsFound()
    {
        // Given a grid with a T-shaped pattern positioned at the edge
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // T-shape at top edge
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical stem down
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
        };
        var cells = this.CreateTestReadableGrid(grid);

        // When the player looks for T-shaped patterns
        var matches = _strategy.FindMatches(cells);

        // Then the edge T-shaped match is found
        Assert.That(matches, Has.Count.EqualTo(1), "Should find T-shaped pattern even when positioned at grid edge");
        Assert.That(matches[0].Count, Is.EqualTo(5), "Edge T-shape should contain 5 elements like any minimal T-shape");
    }

    #endregion
}