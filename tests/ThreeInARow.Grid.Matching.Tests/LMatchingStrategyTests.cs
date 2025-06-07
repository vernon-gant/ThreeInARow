using ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

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

    #region Negative Cases

    [Test]
    public void When_NoMatches_Then_ReturnsNoLMatches()
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
    public void When_MatchesDoNotIntersect_Then_ReturnsNoLMatches()
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
    public void When_TShapeExists_Then_ReturnsNoLMatches()
    {
        // Given - T-shape (middle intersection, not corner)
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal line
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical stem (middle intersection)
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
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
    public void When_CrossShapeExists_Then_ReturnsNoLMatches()
    {
        // Given - Cross shape (middle intersections)
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical above
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal through center
            { "S", "A", "T", "U", "V", "W", "X", "Y" }, // Vertical below
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
    public void When_EmptyCellsBreakLShape_Then_ReturnsNoLMatches()
    {
        // Given - L-shape broken by empty cells
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal part
            { null, "G", "H", "I", "J", "K", "L", "M" }, // Empty breaks vertical
            { "A", "N", "O", "P", "Q", "R", "S", "T" },
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
    public void When_BothArmsTooShort_Then_ReturnsNoLMatches()
    {
        // Given - Both arms only 2 cells (below minimum)
        var grid = new[,]
        {
            { "A", "A", "B", "C", "D", "E", "F", "G" }, // Only 2 horizontally
            { "A", "H", "I", "J", "K", "L", "M", "N" }, // Only 2 vertically
            { "O", "P", "Q", "R", "S", "T", "U", "V" },
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
    public void When_LShapeTopLeft_Then_ReturnsOneLMatch()
    {
        // Given - L-shape with corner at top-left
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal part
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical part down
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
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(5)); // 3 + 3 - 1
    }

    [Test]
    public void When_LShapeTopRight_Then_ReturnsOneLMatch()
    {
        // Given - L-shape with corner at top-right
        var grid = new[,]
        {
            { "B", "A", "A", "A", "C", "D", "E", "F" }, // Horizontal part
            { "G", "H", "I", "A", "J", "K", "L", "M" }, // Vertical part down
            { "N", "O", "P", "A", "Q", "R", "S", "T" }, // Vertical continues
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
        Assert.That(matches[0].Count, Is.EqualTo(5)); // 3 + 3 - 1
    }

    [Test]
    public void When_LShapeBottomLeft_Then_ReturnsOneLMatch()
    {
        // Given - L-shape with corner at bottom-left
        var grid = new[,]
        {
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical part up
            { "A", "N", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal part
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
        Assert.That(matches[0].Count, Is.EqualTo(5)); // 3 + 3 - 1
    }

    [Test]
    public void When_LShapeBottomRight_Then_ReturnsOneLMatch()
    {
        // Given - L-shape with corner at bottom-right
        var grid = new[,]
        {
            { "G", "H", "I", "A", "J", "K", "L", "M" }, // Vertical part up
            { "N", "O", "P", "A", "Q", "R", "S", "T" }, // Vertical continues
            { "B", "A", "A", "A", "C", "D", "E", "F" }, // Horizontal part
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
        Assert.That(matches[0].Count, Is.EqualTo(5)); // 3 + 3 - 1
    }

    [Test]
    public void When_LargerLShapes_Then_ReturnsCorrectSizes()
    {
        // Given - Two L-shapes of different sizes
        var grid = new[,]
        {
            { "A", "A", "A", "A", "B", "B", "B", "C" }, // 4-cell and 3-cell horizontal
            { "A", "E", "F", "G", "H", "I", "B", "J" }, // Vertical arms down
            { "A", "K", "L", "M", "N", "O", "B", "P" }, // Vertical continues
            { "A", "Q", "R", "S", "T", "U", "B", "V" }, // First L longer vertical
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

        var firstMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 0));
        var secondMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 6));

        Assert.That(firstMatch.Count, Is.EqualTo(7)); // 4 + 4 - 1
        Assert.That(secondMatch.Count, Is.EqualTo(6)); // 3 + 4 - 1
    }

    [Test]
    public void When_MultipleLShapesSameSize_Then_ReturnsAllMatches()
    {
        // Given - Three identical L-shapes
        var grid = new[,]
        {
            { "A", "A", "A", "B", "B", "B", "C", "C" }, // Three horizontal matches
            { "A", "D", "E", "B", "F", "G", "C", "H" }, // Three vertical arms
            { "A", "I", "J", "B", "K", "L", "C", "M" }, // All same size
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
        Assert.That(matches.All(m => m.Count == 5), Is.True); // All 3 + 3 - 1
    }

    [Test]
    public void When_LShapeWithMinimumSize_Then_ReturnsOneMatch()
    {
        // Given - Minimum valid L-shape (3x3)
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Exactly 3 horizontal
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Exactly 3 vertical
            { "A", "N", "O", "P", "Q", "R", "S", "T" },
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
        Assert.That(matches[0].Count, Is.EqualTo(5));
    }

    [Test]
    public void When_LShapeWithMixedOrientations_Then_ReturnsAllMatches()
    {
        // Given - Mixed L-shape orientations in same grid
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "C", "C", "C" }, // Top-left L, Top-right L
            { "A", "D", "E", "F", "G", "H", "I", "C" }, // Vertical arms
            { "A", "J", "K", "L", "M", "N", "O", "C" }, // Vertical continues
            { "P", "Q", "R", "S", "T", "U", "V", "W" },
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

        var leftL = matches.First(m => m.Any(cell => cell.ColumnIndex == 0));
        var rightL = matches.First(m => m.Any(cell => cell.ColumnIndex == 7));

        Assert.That(leftL.Count, Is.EqualTo(5)); // 3 + 3 - 1
        Assert.That(rightL.Count, Is.EqualTo(6)); // 4 + 3 - 1
    }

    #endregion
}