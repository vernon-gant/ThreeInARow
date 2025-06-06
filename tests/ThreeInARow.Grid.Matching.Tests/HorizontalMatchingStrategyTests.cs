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

    [Test]
    public void When_GridHasMultipleHorizontalMatchesInSameRow_Then_ReturnsAllMatches()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D", "E", "F", "G", "H" },
            { "A", "A", "A", "B", "C", "C", "C", "E" }, // AAA match at (1,0-2) and CCC match at (1,4-6)
            { "C", "D", "E", "F", "G", "H", "A", "B" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(2));

        var firstMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 0));
        var secondMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 4));

        Assert.That(firstMatch.Count, Is.EqualTo(3));
        Assert.That(secondMatch.Count, Is.EqualTo(3));

        var firstMatchPositions = firstMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.ColumnIndex);
        var secondMatchPositions = secondMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.ColumnIndex);

        var expectedFirstPositions = new[] { (1, 0), (1, 1), (1, 2) };
        var expectedSecondPositions = new[] { (1, 4), (1, 5), (1, 6) };

        Assert.That(firstMatchPositions, Is.EqualTo(expectedFirstPositions));
        Assert.That(secondMatchPositions, Is.EqualTo(expectedSecondPositions));
    }

    [Test]
    public void When_GridHasHorizontalMatchesInDifferentRows_Then_ReturnsAllMatches()
    {
        // Given
        var grid = new[,]
        {
            { "A", "A", "A", "B", "C", "D", "E", "F" }, // AAA match at (0,0-2)
            { "B", "C", "D", "E", "F", "G", "H", "I" },
            { "C", "D", "B", "B", "B", "E", "F", "G" }, // BBB match at (2,2-4)
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(2));

        var firstRowMatch = matches.First(m => m.Any(cell => cell.RowIndex == 0));
        var thirdRowMatch = matches.First(m => m.Any(cell => cell.RowIndex == 2));

        Assert.That(firstRowMatch.Count, Is.EqualTo(3));
        Assert.That(thirdRowMatch.Count, Is.EqualTo(3));

        var firstRowPositions = firstRowMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.ColumnIndex);
        var thirdRowPositions = thirdRowMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.ColumnIndex);

        var expectedFirstRowPositions = new[] { (0, 0), (0, 1), (0, 2) };
        var expectedThirdRowPositions = new[] { (2, 2), (2, 3), (2, 4) };

        Assert.That(firstRowPositions, Is.EqualTo(expectedFirstRowPositions));
        Assert.That(thirdRowPositions, Is.EqualTo(expectedThirdRowPositions));
    }

    [Test]
    public void When_GridHasEmptyCellsBreakingMatch_Then_ReturnsNoMatches()
    {
        // Given
        var grid = new[,]
        {
            { "A", "A", null, "A", "A", "B", "C", "D" }, // AA-AA broken by empty cell
            { "B", "C", "C", null, "C", "C", "D", "E" }, // CC-CC broken by empty cell
            { "C", "D", "E", "F", "G", "H", "I", "J" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_GridHasValidMatchAfterEmptyCells_Then_ReturnsMatch()
    {
        // Given
        var grid = new[,]
        {
            { null, null, "A", "A", "A", "B", "C", "D" }, // AAA match after empty cells at (0,2-4)
            { "B", "C", "D", "E", "F", "G", "H", "I" },
            { "C", "D", "E", "F", "G", "H", "I", "J" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(3));

        var matchPositions = matches[0].Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.ColumnIndex);
        var expectedPositions = new[] { (0, 2), (0, 3), (0, 4) };
        Assert.That(matchPositions, Is.EqualTo(expectedPositions));
    }

    [Test]
    public void When_GridHasValidMatchBeforeEmptyCells_Then_ReturnsMatch()
    {
        // Given
        var grid = new[,]
        {
            { "A", "A", "A", null, null, "B", "C", "D" }, // AAA match before empty cells at (0,0-2)
            { "B", "C", "D", "E", "F", "G", "H", "I" },
            { "C", "D", "E", "F", "G", "H", "I", "J" },
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(3));

        var matchPositions = matches[0].Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.ColumnIndex);
        var expectedPositions = new[] { (0, 0), (0, 1), (0, 2) };
        Assert.That(matchPositions, Is.EqualTo(expectedPositions));
    }
}