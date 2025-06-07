using ThreeInARow.Grid.ADT;

namespace ThreeInARow.Grid.Tests;

[TestFixture]
public abstract class ReadableGridBehaviorTests<TGrid> : GridTestsBase<IReadableGrid<string>>
{
    #region Inspecting Individual Cells

    [Test]
    public void GivenAValidPositionOnTheGrid_WhenPlayerInspectsTheCell_ThenTheCellInformationIsReturned()
    {
        // Given a 4x4 grid with a specific position within bounds
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var row = 2;
        var column = 3;

        // When the player inspects the cell at that position
        var result = _grid.TryGetCell(row, column);

        // Then the cell information is successfully returned
        Assert.That(result.IsT0, Is.True, "Cell information should be returned for valid coordinates");
        var cell = result.AsT0;
        Assert.That(cell.RowIndex, Is.EqualTo(row), "Cell should report correct row position");
        Assert.That(cell.ColumnIndex, Is.EqualTo(column), "Cell should report correct column position");
    }

    [Test]
    public void GivenAPositionWithInvalidRowCoordinate_WhenPlayerTriesToInspectTheCell_ThenAnOutOfBoundsErrorIsReturned()
    {
        // Given a 4x4 grid where the player tries to access a row that doesn't exist
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var invalidRow = 10;
        var validColumn = 1;

        // When the player attempts to inspect the cell at the invalid position
        var result = _grid.TryGetCell(invalidRow, validColumn);

        // Then an out of bounds error is returned
        Assert.That(result.IsT1, Is.True, "Should return error for row outside grid boundaries");
        Assert.That(result.AsT1, Is.TypeOf<CellOutOfBounds>(), "Should indicate the position is out of bounds");
    }

    [Test]
    public void GivenAPositionWithInvalidColumnCoordinate_WhenPlayerTriesToInspectTheCell_ThenAnOutOfBoundsErrorIsReturned()
    {
        // Given a 4x4 grid where the player tries to access a column that doesn't exist
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var validRow = 1;
        var invalidColumn = -5;

        // When the player attempts to inspect the cell at the invalid position
        var result = _grid.TryGetCell(validRow, invalidColumn);

        // Then an out of bounds error is returned
        Assert.That(result.IsT1, Is.True, "Should return error for column outside grid boundaries");
        Assert.That(result.AsT1, Is.TypeOf<CellOutOfBounds>(), "Should indicate the position is out of bounds");
    }

    [Test]
    public void GivenAPositionCompletelyOutsideTheGrid_WhenPlayerTriesToInspectTheCell_ThenAnOutOfBoundsErrorIsReturned()
    {
        // Given a 4x4 grid where the player tries to access coordinates completely outside the grid
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var invalidRow = -1;
        var invalidColumn = 10;

        // When the player attempts to inspect the cell at the completely invalid position
        var result = _grid.TryGetCell(invalidRow, invalidColumn);

        // Then an out of bounds error is returned
        Assert.That(result.IsT1, Is.True, "Should return error for coordinates completely outside grid");
        Assert.That(result.AsT1, Is.TypeOf<CellOutOfBounds>(), "Should indicate both coordinates are out of bounds");
    }

    [Test]
    public void GivenACellContainingAnElement_WhenPlayerInspectsIt_ThenTheCellReportsAsOccupied()
    {
        // Given a grid with an element placed at position (1,1)
        var gridData = new[,]
        {
            { null, null, null, null },
            { null, "A", null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);

        // When the player inspects the cell containing the element
        var result = _grid.TryGetCell(1, 1);

        // Then the cell reports as occupied
        Assert.That(result.IsT0, Is.True, "Should successfully retrieve cell with element");
        var cell = result.AsT0;
        Assert.That(cell.IsOccupied(), Is.True, "Cell containing an element should report as occupied");
    }

    [Test]
    public void GivenAnEmptyCell_WhenPlayerInspectsIt_ThenTheCellReportsAsEmpty()
    {
        // Given a grid with an empty position at (2,2)
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);

        // When the player inspects the empty cell
        var result = _grid.TryGetCell(2, 2);

        // Then the cell reports as empty
        Assert.That(result.IsT0, Is.True, "Should successfully retrieve empty cell");
        var cell = result.AsT0;
        Assert.That(cell.IsOccupied(), Is.False, "Empty cell should report as not occupied");
    }

    #endregion

    #region Scanning the Entire Grid

    [Test]
    public void GivenAGridWithVariousElements_WhenPlayerScansTheEntireGrid_ThenAllCellsAreReturnedInLogicalOrder()
    {
        // Given a grid containing a mix of occupied and empty cells
        var gridData = new[,]
        {
            { "A", null, null, null },
            { null, "B", null, null },
            { null, null, "C", null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);

        // When the player scans through the entire grid
        var allCells = _grid.ToList();

        // Then all cells are returned in row-major order (left-to-right, top-to-bottom)
        Assert.That(allCells.Count, Is.EqualTo(16), "4x4 grid should contain exactly 16 cells");

        var topLeftCell = allCells[0];
        Assert.That(topLeftCell.RowIndex, Is.EqualTo(0), "First cell should be from row 0");
        Assert.That(topLeftCell.ColumnIndex, Is.EqualTo(0), "First cell should be from column 0");
        Assert.That(topLeftCell.IsOccupied(), Is.True, "Top-left cell should contain element A");

        var secondCell = allCells[1];
        Assert.That(secondCell.RowIndex, Is.EqualTo(0), "Second cell should be from row 0");
        Assert.That(secondCell.ColumnIndex, Is.EqualTo(1), "Second cell should be from column 1");
    }

    [Test]
    public void GivenACompletelyEmptyGrid_WhenPlayerScansTheEntireGrid_ThenAllCellsReportAsEmpty()
    {
        // Given a grid with no elements in any position
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);

        // When the player scans through the entire grid
        var allCells = _grid.ToList();

        // Then all cells are reported as empty
        Assert.That(allCells.Count, Is.EqualTo(16), "4x4 grid should contain exactly 16 cells");
        Assert.That(allCells.All(cell => !cell.IsOccupied()), Is.True, "All cells in empty grid should report as not occupied");
    }

    [Test]
    public void GivenACompletelyFilledGrid_WhenPlayerScansTheEntireGrid_ThenAllCellsReportAsOccupied()
    {
        // Given a grid where every position contains an element
        var gridData = new[,]
        {
            { ".", ".", ".", "." },
            { ".", ".", ".", "." },
            { ".", ".", ".", "." },
            { ".", ".", ".", "." }
        };
        _grid = CreateGrid(gridData);

        // When the player scans through the entire grid
        var allCells = _grid.ToList();

        // Then all cells are reported as occupied
        Assert.That(allCells.Count, Is.EqualTo(16), "4x4 grid should contain exactly 16 cells");
        Assert.That(allCells.All(cell => cell.IsOccupied()), Is.True, "All cells in filled grid should report as occupied");
    }

    [Test]
    public void GivenAGridWithElementsInSpecificPattern_WhenPlayerScansTheGrid_ThenThePatternIsDetectableInTheResults()
    {
        // Given a grid with elements arranged in a diagonal pattern
        var gridData = new[,]
        {
            { "X", null, null, null },
            { null, "X", null, null },
            { null, null, "X", null },
            { null, null, null, "X" }
        };
        _grid = CreateGrid(gridData);

        // When the player scans through the grid
        var allCells = _grid.ToList();
        var occupiedCells = allCells.Where(cell => cell.IsOccupied()).ToList();

        // Then the diagonal pattern is detectable in the results
        Assert.That(occupiedCells.Count, Is.EqualTo(4), "Should find exactly 4 occupied cells in diagonal pattern");
    }

    #endregion

    #region Grid State Analysis

    [Test]
    public void GivenAGridWithKnownElementDistribution_WhenPlayerAnalyzesTheGridState_ThenAccurateStatisticsAreAvailable()
    {
        // Given a grid with a known distribution of elements and empty spaces
        var gridData = new[,]
        {
            { "A", "B", "A", null },
            { "B", null, "B", "A" },
            { null, "A", null, "B" },
            { "A", null, "B", null }
        };
        _grid = CreateGrid(gridData);

        // When the player analyzes the current grid state
        var allCells = _grid.ToList();
        var occupiedCells = allCells.Where(cell => cell.IsOccupied()).ToList();
        var emptyCells = allCells.Where(cell => !cell.IsOccupied()).ToList();

        Assert.Multiple(() =>
        {
            // Then accurate statistics about the grid state are available
            Assert.That(allCells.Count, Is.EqualTo(16), "Grid should have total of 16 cells");
            Assert.That(occupiedCells.Count, Is.EqualTo(10), "Grid should have 10 occupied cells");
            Assert.That(emptyCells.Count, Is.EqualTo(6), "Grid should have 6 empty cells");
        });
    }

    [Test]
    public void GivenMultipleScansOfTheSameGrid_WhenPlayerRepeatedlyChecksTheState_ThenConsistentResultsAreReturned()
    {
        // Given a static grid that doesn't change between scans
        var gridData = new[,]
        {
            { "X", null, "Y", null },
            { null, "X", null, "Y" },
            { "Y", null, "X", null },
            { null, "Y", null, "X" }
        };
        _grid = CreateGrid(gridData);

        // When the player scans the grid multiple times
        var firstScan = _grid.ToList();
        var secondScan = _grid.ToList();

        // Then consistent results are returned each time
        Assert.That(firstScan.Count, Is.EqualTo(secondScan.Count), "Multiple scans should return same number of cells");
    }

    #endregion
}