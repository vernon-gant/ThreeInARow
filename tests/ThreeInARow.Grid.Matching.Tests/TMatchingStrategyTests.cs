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

    # region Negative Cases

    [Test]
    public void When_NoHorizontalOrVerticalMatches_Then_ReturnsNoTMatches()
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
    public void When_MatchesDoNotIntersect_Then_ReturnsNoTMatches()
    {
        // Given - Separate horizontal and vertical matches
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
    public void When_LShapeExists_Then_ReturnsNoTMatches()
    {
        // Given - L-shape (intersection at corner, not middle)
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical match
            { "A", "N", "O", "P", "Q", "R", "S", "T" }, // Corner intersection
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
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_CrossShapeExists_Then_ReturnsNoTMatches()
    {
        // Given - Cross shape (intersection in middle of both lines)
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical part above
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal through center
            { "S", "A", "T", "U", "V", "W", "X", "Y" }, // Vertical part below
            { "Z", null, null, null, null, null, null, null },
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
    public void When_EmptyCellsBreakTShape_Then_ReturnsNoMatches()
    {
        // Given - T-shape broken by empty cell
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal match
            { "G", null, "H", "I", "J", "K", "L", "M" }, // Empty cell breaks vertical
            { "N", "A", "O", "P", "Q", "R", "S", "T" },
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
        Assert.That(matches, Is.Empty);
    }

    # endregion

    #region Positive Cases

    [Test]
    public void When_TShapePointingDown_Then_ReturnsOneTMatch()
    {
        // Given - T pointing down ⊤
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal top
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical stem down
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
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
        // Given - T pointing up ⊥
        var grid = new[,]
        {
            { "G", "A", "H", "I", "J", "K", "L", "M" }, // Vertical stem up
            { "N", "A", "O", "P", "Q", "R", "S", "T" }, // Vertical continues
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // Horizontal bottom
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
    public void When_TShapePointingRight_Then_ReturnsOneTMatch()
    {
        // Given - T pointing right ⊣
        var grid = new[,]
        {
            { "A", "G", "H", "I", "J", "K", "L", "M" }, // Vertical left
            { "A", "A", "A", "N", "O", "P", "Q", "R" }, // Horizontal stem right
            { "A", "S", "T", "U", "V", "W", "X", "Y" }, // Vertical continues
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
    public void When_TShapePointingLeft_Then_ReturnsOneTMatch()
    {
        // Given - T pointing left ⊢
        var grid = new[,]
        {
            { "G", "H", "I", "A", "J", "K", "L", "M" }, // Vertical right
            { "N", "A", "A", "A", "O", "P", "Q", "R" }, // Horizontal stem left
            { "S", "T", "U", "A", "V", "W", "X", "Y" }, // Vertical continues
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
    public void When_LargerTShape_Then_ReturnsOneTMatch()
    {
        // Given - T with 5-cell horizontal, 4-cell vertical
        var grid = new[,]
        {
            { "A", "A", "A", "A", "A", "B", "C", "D" }, // 5-cell horizontal
            { "G", "H", "Y", "A", "J", "K", "L", "M" }, // Vertical down from middle
            { "N", "O", "Z", "A", "Q", "R", "S", "T" }, // Vertical continues
            { "U", "V", "B", "A", "Y", "Z", null, null }, // Vertical continues
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
        Assert.That(matches[0].Count, Is.EqualTo(8)); // 5 + 4 - 1
    }

    [Test]
    public void When_MultipleTShapes_Then_ReturnsAllTMatches()
    {
        // Given - Two separate T-shapes
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "C", "C", "D" }, // Two horizontal matches
            { "E", "A", "F", "G", "H", "C", "I", "J" }, // Two vertical stems
            { "K", "A", "M", "N", "O", "C", "Q", "R" },
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
    }

    #endregion
}