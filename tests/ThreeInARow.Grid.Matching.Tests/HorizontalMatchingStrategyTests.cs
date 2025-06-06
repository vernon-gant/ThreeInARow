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
}