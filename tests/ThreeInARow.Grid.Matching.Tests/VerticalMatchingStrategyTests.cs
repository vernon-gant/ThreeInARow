using ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Tests;

public class VerticalMatchingStrategyTests : ICellsFromGridConverter
{
    private readonly VerticalMatchingStrategy<string> _strategy = new(minMatchLength: 3);

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
    public void When_GridHasNoVerticalMatches_Then_ReturnsNoMatches()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C" },
            { "B", "A", "B" },
            { "A", "B", "A" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void When_GridHasOneVerticalMatchOfMinimumLength_Then_ReturnsOneMatch()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D" },
            { "A", "C", "D", "E" }, // AAA match at column 0, positions (0,0), (1,0), (2,0)
            { "A", "D", "E", "F" },
            { "B", "E", "F", "G" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(3));

        var matchPositions = matches[0].Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);
        var expectedPositions = new[] { (0, 0), (1, 0), (2, 0) };
        Assert.That(matchPositions, Is.EqualTo(expectedPositions));
    }

    [Test]
    public void When_GridHasVerticalMatchLongerThanMinimum_Then_ReturnsOneMatchWithCorrectLength()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D" },
            { "A", "C", "D", "E" }, // AAAAA match at column 0, positions (0,0) through (4,0)
            { "A", "D", "E", "F" },
            { "A", "E", "F", "G" },
            { "A", "F", "G", "H" },
            { "B", "G", "H", "I" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(5));

        var matchPositions = matches[0].Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);
        var expectedPositions = new[] { (0, 0), (1, 0), (2, 0), (3, 0), (4, 0) };
        Assert.That(matchPositions, Is.EqualTo(expectedPositions));
    }

    [Test]
    public void When_GridHasMultipleVerticalMatchesInSameColumn_Then_ReturnsAllMatches()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D" },
            { "A", "C", "D", "E" }, // AAA match at (0-2, 0)
            { "A", "D", "E", "F" },
            { "B", "E", "F", "G" }, // Separator
            { "C", "F", "G", "H" }, // CCC match at (4-6, 0)
            { "C", "G", "H", "I" },
            { "C", "H", "I", "J" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(2));

        var firstMatch = matches.First(m => m.Any(cell => cell.RowIndex == 0));
        var secondMatch = matches.First(m => m.Any(cell => cell.RowIndex == 4));

        Assert.That(firstMatch.Count, Is.EqualTo(3));
        Assert.That(secondMatch.Count, Is.EqualTo(3));

        var firstMatchPositions = firstMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);
        var secondMatchPositions = secondMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);

        var expectedFirstPositions = new[] { (0, 0), (1, 0), (2, 0) };
        var expectedSecondPositions = new[] { (4, 0), (5, 0), (6, 0) };

        Assert.That(firstMatchPositions, Is.EqualTo(expectedFirstPositions));
        Assert.That(secondMatchPositions, Is.EqualTo(expectedSecondPositions));
    }

    [Test]
    public void When_GridHasVerticalMatchesInDifferentColumns_Then_ReturnsAllMatches()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D" }, // AAA match at column 0, BBB match at column 1
            { "A", "B", "D", "E" },
            { "A", "B", "E", "F" },
            { "B", "C", "F", "G" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(2));

        var firstColumnMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 0));
        var secondColumnMatch = matches.First(m => m.Any(cell => cell.ColumnIndex == 1));

        Assert.That(firstColumnMatch.Count, Is.EqualTo(3));
        Assert.That(secondColumnMatch.Count, Is.EqualTo(3));

        var firstColumnPositions = firstColumnMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);
        var secondColumnPositions = secondColumnMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);

        var expectedFirstColumnPositions = new[] { (0, 0), (1, 0), (2, 0) };
        var expectedSecondColumnPositions = new[] { (0, 1), (1, 1), (2, 1) };

        Assert.That(firstColumnPositions, Is.EqualTo(expectedFirstColumnPositions));
        Assert.That(secondColumnPositions, Is.EqualTo(expectedSecondColumnPositions));
    }

    [Test]
    public void When_GridHasEmptyCellsBreakingMatch_Then_ReturnsNoMatches()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D" },
            { "A", "C", "D", "E" }, // AA-AA broken by empty cell
            { null, "D", "E", "F" },
            { "A", "E", "F", "G" },
            { "A", "F", "G", "H" }
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
            { null, "B", "C", "D" },
            { null, "C", "D", "E" },
            { "A", "D", "E", "F" }, // AAA match at column 0, positions (2,0), (3,0), (4,0)
            { "A", "E", "F", "G" },
            { "A", "F", "G", "H" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(3));

        var matchPositions = matches[0].Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);
        var expectedPositions = new[] { (2, 0), (3, 0), (4, 0) };
        Assert.That(matchPositions, Is.EqualTo(expectedPositions));
    }

    [Test]
    public void When_GridHasValidMatchBeforeEmptyCells_Then_ReturnsMatch()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D" }, // AAA match at column 0, positions (0,0), (1,0), (2,0)
            { "A", "C", "D", "E" },
            { "A", "D", "E", "F" },
            { null, "E", "F", "G" },
            { null, "F", "G", "H" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Count, Is.EqualTo(3));

        var matchPositions = matches[0].Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);
        var expectedPositions = new[] { (0, 0), (1, 0), (2, 0) };
        Assert.That(matchPositions, Is.EqualTo(expectedPositions));
    }

    [Test]
    public void When_GridHasMatchesSeparatedByEmptyCells_Then_ReturnsBothMatches()
    {
        // Given
        var grid = new[,]
        {
            { "A", "B", "C", "D" }, // AAA at (0-2, 0) and AAA at (4-6, 0)
            { "A", "C", "D", "E" },
            { "A", "D", "E", "F" },
            { null, "E", "F", "G" },
            { "A", "F", "G", "H" },
            { "A", "G", "H", "I" },
            { "A", "H", "I", "J" }
        };
        var cells = this.CreateCellsFromGrid(grid);

        // When
        var matches = _strategy.FindMatches(cells);

        // Then
        Assert.That(matches, Has.Count.EqualTo(2));

        var firstMatch = matches.First(m => m.Any(cell => cell.RowIndex == 0));
        var secondMatch = matches.First(m => m.Any(cell => cell.RowIndex == 4));

        Assert.That(firstMatch.Count, Is.EqualTo(3));
        Assert.That(secondMatch.Count, Is.EqualTo(3));

        var firstMatchPositions = firstMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);
        var secondMatchPositions = secondMatch.Select(cell => (cell.RowIndex, cell.ColumnIndex)).OrderBy(pos => pos.RowIndex);

        var expectedFirstPositions = new[] { (0, 0), (1, 0), (2, 0) };
        var expectedSecondPositions = new[] { (4, 0), (5, 0), (6, 0) };

        Assert.That(firstMatchPositions, Is.EqualTo(expectedFirstPositions));
        Assert.That(secondMatchPositions, Is.EqualTo(expectedSecondPositions));
    }
}