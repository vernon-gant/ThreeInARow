using ThreeInARow.Grid.Matching.Implementations;

namespace ThreeInARow.Grid.Matching.Tests;

public class TMatchingStrategyTests : ICellsFromGridConverter
{
    private readonly HorizontalMatchingStrategy<string> _horizontalStrategy = new(3);
    private readonly VerticalMatchingStrategy<string> _verticalStrategy = new(3);
    private readonly TMatchingStrategy<string> _strategy;

    public TMatchingStrategyTests()
    {
        _strategy = new TMatchingStrategy<string>(3, _horizontalStrategy, _verticalStrategy);
    }

    [Test]
    public void When_NoHorizontalOrVerticalMatches_Then_ReturnsNoTMatches()
    {
        // Given - No matches in 8x8 grid
        var grid = new[,]
        {
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" },
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" },
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" },
            { "A", "B", "A", "B", "A", "B", "A", "B" },
            { "B", "A", "B", "A", "B", "A", "B", "A" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_HorizontalAndVerticalMatchesDoNotIntersect_Then_ReturnsNoTMatches()
    {
        // Given - Separate horizontal and vertical matches that don't touch
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match (row 0, cols 0-2)
            { "G", "H", "I", "B", "J", "K", "L", "M" },
            { "N", "O", "P", "B", "Q", "R", "S", "T" },
            { "U", "V", "W", "B", "X", "Y", "Z", null }, // Vertical match (col 3, rows 0-3)
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
    public void When_HorizontalAndVerticalMatchesIntersectAtEnd_Then_ReturnsNoTMatches()
    {
        // Given - L-shape pattern (intersection at corner, not T-shape)
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match(row 0, cols 0-2)
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical match(col 0, rows 0-2)
            { "A", "N", "O", "P", "Q", "R", "S", "T" }, // Intersection at (0,0) which is an END
            { "U", "V", "W", "X", "Y", "Z", null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then - Should be empty because intersection is at end, not middle (L-shape, not T-shape)
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_TShapePointingDown_Then_ReturnsOneTMatch()
    {
        // Given - T-shape pointing down ⊤
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal line (top of T)
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical stem going down
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical stem continues
            { "U", "V", "W", "X", "Y", "Z", null, null },
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
    public void When_TShapePointingUp_Then_ReturnsOneTMatch()
    {
        // Given - T-shape pointing up ⊥
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical stem going up
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical stem continues
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal line (bottom of T)
            { "U", "V", "W", "X", "Y", "Z", null, null },
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
        Assert.That(matches[0], Is.TypeOf<TMatch<string>>());
        Assert.That(matches[0].Count, Is.EqualTo(5));
    }

    [Test]
    public void When_TShapePointingRight_Then_ReturnsOneTMatch()
    {
        // Given - T-shape pointing right ⊣
        var grid = new[,]
        {
            { "A", "G", "H", "I", "J", "K", "L", "M" },
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal stem going right
            { "A", "S", "T", "U", "V", "W", "X", "Y" },
            { "Z", ".", ".", ".", ".", ".", null, null },
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
        Assert.That(matches[0], Is.TypeOf<TMatch<string>>());
        Assert.That(matches[0].Count, Is.EqualTo(5));
    }

    [Test]
    public void When_TShapePointingLeft_Then_ReturnsOneTMatch()
    {
        // Given - T-shape pointing left ⊢
        var grid = new[,]
        {
            { "G", "H", "I", "A", "J", "K", "L", "M" },
            { "N", "A", "A", "A", "O", "P", "Q", "R" }, // Horizontal stem going left
            { "S", "T", "U", "A", "V", "W", "X", "Y" },
            { "Z", ".", ".", ".", ".", ".", null, null },
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
        Assert.That(matches[0], Is.TypeOf<TMatch<string>>());
        Assert.That(matches[0].Count, Is.EqualTo(5));
    }

    [Test]
    public void When_MultipleTShapesExist_Then_ReturnsAllTMatches()
    {
        // Given - Two separate T-shapes
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "C", "C", "D" }, // First T horizontal, Second T horizontal
            { "E", "A", "F", "G", "H", "C", "I", "J" }, // First T stem down, Second T stem down
            { "K", "L", "M", "N", "O", "P", "Q", "R" },
            { "S", "T", "U", "V", "W", "X", "Y", "Z" },
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
        Assert.That(matches.All(m => m is TMatch<string>), Is.True);
    }

    [Test]
    public void When_GridHasEmptyCellsBreakingTMatch_Then_ReturnsNoMatches()
    {
        // Given - T-shape broken by empty cells
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match
            { "G", null, "H", "I", "J", "K", "L", "M" }, // Empty cell breaks vertical stem
            { "N", "O", "P", "Q", "R", "S", "T", "U" },
            { "V", "W", "X", "Y", "Z", ".", ".", "." },
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
    public void When_CrossShapeExists_Then_ReturnsNoTMatches()
    {
        // Given - Cross shape (NOT a T-shape) - this should NOT match
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical part above center
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal line through center
            { "S", "A", "T", "U", "V", "W", "X", "Y" }, // Vertical part below center
            { "Z", ".", ".", ".", ".", ".", null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then - Cross shape should NOT be detected as T-match
        Assert.That(matches, Is.Empty);
    }
}