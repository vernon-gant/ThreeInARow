using ThreeInARow.Grid.Matching.Implementations;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Tests;

public class HorizontalMatchingStrategyTests : ICellsFromGridConverter
{
    private readonly HorizontalMatchingStrategy<string> _strategy = new(minMatchLength: 3);

    [Test]
    public void When_GridIsEmpty_Then_ReturnsNoMatches()
    {
        // Given
        var emptyCells = Enumerable.Empty<ElementCell<string>>();

        // When
        var matches = _strategy.FindMatches(emptyCells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_GridHasNoHorizontalMatches_Then_ReturnsNoMatches()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "A", "C", "C", "A", "B", "C" },
            { "B", "A", "B", "A", "B", "C", "A", "B" },
            { "C", "C", "A", "B", "A", "B", "C", "A" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_GridHasOneHorizontalMatchOfMinimumLength_Then_ReturnsOneMatch()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D", "E", "F", "G", "H" },
            { "B", "A", "A", "A", "B", "C", "D", "E" },
            { "C", "D", "E", "F", "G", "H", "A", "B" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));

        var matchPositions = matches[0].Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.ColumnIndex);
        var expectedPositions = new[] { (1, 1), (1, 2), (1, 3) };
        Assert.That(matchPositions, Is.EqualTo(expectedPositions));
    }

    [Test]
    public void When_GridHasHorizontalMatchLongerThanMinimum_Then_ReturnsOneMatchWithCorrectLength()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D", "E", "F", "G", "H" },
            { "B", "A", "A", "A", "A", "A", "D", "E" },
            { "C", "D", "E", "F", "G", "H", "A", "B" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(5));

        var matchPositions = matches[0].Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.ColumnIndex);
        var expectedPositions = new[] { (1, 1), (1, 2), (1, 3), (1, 4), (1, 5) };
        Assert.That(matchPositions, Is.EqualTo(expectedPositions));
    }
}