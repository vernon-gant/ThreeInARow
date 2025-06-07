using ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

namespace ThreeInARow.Grid.Matching.Tests;

public class CrossMatchingStrategyTests : ICellsFromGridConverter
{
    private readonly HorizontalMatchingStrategy<string> _horizontalStrategy = new(3);
    private readonly VerticalMatchingStrategy<string> _verticalStrategy = new(3);
    private readonly CrossMatchingStrategy<string> _strategy;

    public CrossMatchingStrategyTests()
    {
        _strategy = new CrossMatchingStrategy<string>(3, _horizontalStrategy, _verticalStrategy);
    }

    #region Negative Cases

    [Test]
    public void When_NoMatches_Then_ReturnsNoCrossMatches()
    {
        // Given - No matches in grid
        var grid = new[,]
        {
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" },
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_MatchesDoNotIntersect_Then_ReturnsNoCrossMatches()
    {
        // Given - Separate matches that don't touch
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match
            { "G", "H", "I", "B", "J", "K", "L", "M" },
            { "N", "O", "P", "B", "Q", "R", "S", "T" },
            { "U", "V", "W", "B", "X", "Y", "Z", null }, // Vertical match
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_TShapeExists_Then_ReturnsNoCrossMatches()
    {
        // Given - T-shape (missing bottom arm of cross)
        //   A
        // A A A
        //   (missing A here)
        var grid = new[,]
        {
            { "B", "A", "C", "D", "E", "F", "G", "H" }, // Vertical top
            { "A", "A", "A", "I", "J", "K", "L", "M" }, // Horizontal middle
            { "N", "O", "P", "Q", "R", "S", "T", "U" }, // No vertical bottom
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_LShapeExists_Then_ReturnsNoCrossMatches()
    {
        // Given - L-shape (missing two arms of cross)
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal part
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical part (corner intersection)
            { "A", "N", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_EmptyCellsBreakCross_Then_ReturnsNoCrossMatches()
    {
        // Given - Cross shape broken by empty cell
        //   A
        // A A A
        //   ∅ (empty breaks bottom arm)
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical top
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal middle
            { "S", null, "T", "U", "V", "W", "X", "Y" }, // Empty breaks vertical bottom
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_IncompleteCrossShape_Then_ReturnsNoCrossMatches()
    {
        // Given - Missing one arm of cross (only 3 arms present)
        //   A
        // A A (missing right arm)
        //   A
        var grid = new[,]
        {
            { "B", "A", "C", "D", "E", "F", "G", "H" }, // Vertical top
            { "A", "A", "I", "J", "K", "L", "M", "N" }, // Horizontal left only
            { "O", "A", "P", "Q", "R", "S", "T", "U" }, // Vertical bottom
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    #endregion

    #region Positive Cases

    [Test]
    public void When_MinimumCrossShape_Then_ReturnsOneCrossMatch()
    {
        // Given - Minimum cross shape (3x3)
        //   A
        // A A A
        //   A
        var grid = new[,]
        {
            { "B", "A", "C", "D", "E", "F", "G", "H" }, // Vertical top
            { "A", "A", "A", "I", "J", "K", "L", "M" }, // Horizontal middle
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical bottom
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(5)); // 3 horizontal + 3 vertical - 1 center
    }

    [Test]
    public void When_LargerSymmetricCross_Then_ReturnsOneCrossMatch()
    {
        // Given - Larger symmetric cross (5 horizontal, 5 vertical)
        //     A
        //     A
        // A A A A A
        //     A
        //     A
        var grid = new[,]
        {
            { "I", "J", "A", "K", "L", "M", "N", "O" }, // Vertical top
            { "P", "Q", "A", "R", "S", "T", "U", "V" }, // Vertical continues
            { "A", "A", "A", "A", "A", "W", "X", "Y" }, // 5-cell horizontal
            { "Z", ".", "A", "B", "C", "D", "E", "F" }, // Vertical bottom
            { "G", "H", "A", "I", "J", "K", "L", "M" }, // Vertical continues
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(9)); // 5 + 5 - 1 center
    }

    [Test]
    public void When_AsymmetricCross_Then_ReturnsOneCrossMatch()
    {
        // Given - Asymmetric cross (4 horizontal, 3 vertical)
        //   A
        // A A A A
        //   A
        var grid = new[,]
        {
            { "B", "A", "C", "D", "E", "F", "G", "H" }, // Vertical top
            { "A", "A", "A", "A", "I", "J", "K", "L" }, // 4-cell horizontal
            { "M", "A", "N", "O", "P", "Q", "R", "S" }, // Vertical bottom
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(6)); // 4 + 3 - 1 center
    }

    [Test]
    public void When_MultipleCrossShapes_Then_ReturnsAllCrossMatches()
    {
        // Given - Two separate cross shapes
        //   A       B
        // A A A   B B B
        //   A       B
        var grid = new[,]
        {
            { "C", "A", "D", "E", "B", "F", "G", "H" }, // Two vertical tops
            { "A", "A", "A", "B", "B", "B", "Z", "J" }, // Two horizontal middles
            { "K", "A", "L", "M", "B", "N", "O", "P" }, // Two vertical bottoms
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(2));
        Assert.That(matches.All(m => m.Count == 5), Is.True); // Both 3x3 crosses
    }

    [Test]
    public void When_LargeCrossWithExtendedArms_Then_ReturnsOneCrossMatch()
    {
        // Given - Cross with longer arms (6 horizontal, 4 vertical)
        //     A
        //     A
        // A A A A A A
        //     A
        var grid = new[,]
        {
            { "I", "J", "A", "K", "L", "M", "N", "O" }, // Vertical top
            { "P", "Q", "A", "R", "S", "T", "U", "V" }, // Vertical continues
            { "A", "A", "A", "A", "A", "A", "W", "X" }, // 6-cell horizontal
            { "Y", "Z", "A", ".", "B", "C", "D", "E" }, // Vertical bottom
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(9)); // 6 + 4 - 1 center
    }

    [Test]
    public void When_CrossShapesDifferentSizes_Then_ReturnsAllMatches()
    {
        // Given - Two crosses of different sizes
        //   A         B
        // A A A   B B B B
        //   A         B
        //   A
        var grid = new[,]
        {
            { "C", "A", "D", "E", "B", "F", "G", "H" }, // Different vertical tops
            { "A", "A", "A", "B", "B", "B", "B", "B" }, // Different horizontal sizes
            { "J", "A", "K", "L", "B", "M", "N", "O" }, // Different vertical bottoms
            { "P", "A", "Q", "R", "S", "T", "U", "V" }, // First cross longer vertical
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(2));

        var firstCross = matches.First(m => m.Any(cell => cell.ColumnIndex == 1));
        var secondCross = matches.First(m => m.Any(cell => cell.ColumnIndex == 4));

        Assert.That(firstCross.Count, Is.EqualTo(6)); // 3 + 4 - 1
        Assert.That(secondCross.Count, Is.EqualTo(7)); // 5 + 3 - 1
    }

    #endregion
}