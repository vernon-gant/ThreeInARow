namespace ThreeInARow.Board.Tests;

[TestFixture]
public abstract class BaseGridBehaviorTests<TGrid> where TGrid : IGrid<Element>
{
    private TGrid _grid = default!;

    protected abstract TGrid CreateGrid(Element?[,] gridData);

    #region Helper Methods

    private Element?[,] EmptyGrid(int rows, int columns) => new Element?[rows, columns];

    #endregion

    #region Delete Command Tests

    [Test]
    public void Delete_GivenOccupiedCell_WhenDeleting_ThenReturnsSuccessAndCellBecomesEmpty()
    {
        // Given a grid with an occupied cell at position (1,1)
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, new("A"), null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var cell = _grid.TryGetCell(1, 1).AsT0;

        // When deleting the occupied cell
        var result = _grid.Delete(cell);

        // Then operation succeeds and cell becomes empty
        Assert.That(result.IsT0, Is.True, "Delete should return Success");
        var cellAfterDelete = _grid.Single(c => c.IsInRow(cell.Row) && c.IsInColumn(cell.Column));
        Assert.That(cellAfterDelete.IsOccupied(), Is.False, "Cell should be empty after deletion");
    }

    [Test]
    public void Delete_GivenAlreadyEmptyCell_WhenDeleting_ThenReturnsCellAlreadyDeleted()
    {
        // Given a grid with an empty cell at position (1,1)
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var cell = _grid.TryGetCell(1, 1).AsT0;

        // When attempting to delete the empty cell
        var result = _grid.Delete(cell);

        // Then operation fails with CellAlreadyDeleted
        Assert.That(result.IsT1, Is.True, "Delete should return CellAlreadyDeleted");
    }

    [Test]
    public void Delete_GivenCellDeletedTwice_WhenDeletingSecondTime_ThenReturnsCellAlreadyDeleted()
    {
        // Given a grid with an occupied cell that gets deleted
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, null, new("A"), null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var cell = _grid.TryGetCell(2, 2).AsT0;
        _grid.Delete(cell); // First deletion

        // When attempting to delete the same cell again
        var result = _grid.Delete(cell);

        // Then operation fails with CellAlreadyDeleted
        Assert.That(result.IsT1, Is.True, "Second delete should return CellAlreadyDeleted");
    }

    #endregion

    #region AddTop Command Tests

    [Test]
    public void AddTop_GivenColumnWithSpaceAtTop_WhenAddingElement_ThenReturnsSuccessAndElementIsPlaced()
    {
        // Given a grid where column 1 has space at the top
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, new("B"), null, null }
        };
        _grid = CreateGrid(gridData);
        var element = new Element("A");

        // When adding element to top of column 1
        var result = _grid.AddTop(1, element);

        // Then operation succeeds and element is placed at top
        Assert.That(result.IsT0, Is.True, "AddTop should return Success");
        var topCell = _grid.Single(c => c.IsInRow(0) && c.IsInColumn(1));
        Assert.That(topCell.IsOccupied(), Is.True, "Top cell should be occupied");
        Assert.That(topCell.MatchesWith(new ElementCell<Element>(element, new GridCell(0, 1))), Is.True, "Top cell should contain the added element");
    }

    [Test]
    public void AddTop_GivenInvalidColumn_WhenAddingElement_ThenReturnsColumnOutOfBounds()
    {
        // Given a 4x4 grid
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var element = new Element("C");
        var invalidColumn = 10; // Beyond grid bounds

        // When attempting to add element to invalid column
        var result = _grid.AddTop(invalidColumn, element);

        // Then operation fails with ColumnOutOfBounds
        Assert.That(result.IsT1, Is.True, "AddTop should return ColumnOutOfBounds");
    }

    [Test]
    public void AddTop_GivenFullColumn_WhenAddingElement_ThenReturnsColumnIsFull()
    {
        // Given a grid where column 2 is completely full
        var gridData = new Element?[,]
        {
            { null, null, new("A"), null },
            { null, null, new("B"), null },
            { null, null, new("C"), null },
            { null, null, new("D"), null }
        };
        _grid = CreateGrid(gridData);
        var element = new Element("E");

        // When attempting to add another element to the full column
        var result = _grid.AddTop(2, element);

        // Then operation fails with ColumnIsFull
        Assert.That(result.IsT2, Is.True, "AddTop should return ColumnIsFull");
    }

    [Test]
    public void AddTop_GivenColumnWithTopOccupied_WhenAddingElement_ThenReturnsColumnIsFull()
    {
        // Given a grid where only the top row of column 3 is occupied
        var gridData = new Element?[,]
        {
            { null, null, null, new("Existing") },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var element = new Element("New");

        // When attempting to add another element
        var result = _grid.AddTop(3, element);

        // Then operation fails with ColumnIsFull
        Assert.That(result.IsT2, Is.True, "AddTop should return ColumnIsFull when top is occupied");
    }

    #endregion

    #region ShiftDown Command Tests

    [Test]
    public void ShiftDown_GivenColumnWithGapInMiddle_WhenShifting_ThenElementsMoveDownAndTopBecomesEmpty()
    {
        // Given a grid where column 1 has elements with a gap in the middle
        var gridData = new Element?[,]
        {
            { null, new("Top"),    null, null },
            { null, null,          null, null },
            { null, new("Bottom"), null, null },
            { null, null,          null, null }
        };
        _grid = CreateGrid(gridData);

        // When shifting column 1 down
        var result = _grid.ShiftDown(1);

        // Then operation succeeds and elements shift down with top becoming empty
        Assert.That(result.IsT0, Is.True, "ShiftDown should return Success");

        var topCell = _grid.Single(c => c.IsInRow(0) && c.IsInColumn(1));
        Assert.That(topCell.IsOccupied(), Is.False, "Top cell should be empty after shift");

        var middleCell = _grid.Single(c => c.IsInRow(1) && c.IsInColumn(1));
        Assert.That(middleCell.IsOccupied(), Is.True, "Middle cell should now be occupied");
    }

    [Test]
    public void ShiftDown_GivenInvalidColumn_WhenShifting_ThenReturnsColumnOutOfBounds()
    {
        // Given a 4x4 grid
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var invalidColumn = -1;

        // When attempting to shift invalid column
        var result = _grid.ShiftDown(invalidColumn);

        // Then operation fails with ColumnOutOfBounds
        Assert.That(result.IsT1, Is.True, "ShiftDown should return ColumnOutOfBounds");
    }

    [Test]
    public void ShiftDown_GivenFullColumnWithNoGaps_WhenShifting_ThenReturnsColumnIsFull()
    {
        // Given a grid where column 0 is completely full with no gaps
        var gridData = new Element?[,]
        {
            { new("A"), null, null, null },
            { new("B"), null, null, null },
            { new("C"), null, null, null },
            { new("D"), null, null, null }
        };
        _grid = CreateGrid(gridData);

        // When attempting to shift down the full column
        var result = _grid.ShiftDown(0);

        // Then operation fails with ColumnIsFull
        Assert.That(result.IsT2, Is.True, "ShiftDown should return ColumnIsFull");
    }

    [Test]
    public void ShiftDown_GivenEmptyColumn_WhenShifting_ThenReturnsSuccessAndNoChange()
    {
        // Given a grid where column 2 is completely empty
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);

        // When attempting to shift down the empty column
        var result = _grid.ShiftDown(2);

        // Then operation fails with ColumnIsFull (no elements to shift)
        Assert.That(result.IsT0, Is.True, "ShiftDown should return Success for empty column");
    }

    #endregion

    #region TryGetCell Query Tests

    [Test]
    public void TryGetCell_GivenValidCoordinates_WhenGettingCell_ThenReturnsValidGridCell()
    {
        // Given a 4x4 grid with valid coordinates
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var row = 2;
        var column = 3;

        // When attempting to get the cell
        var result = _grid.TryGetCell(row, column);

        // Then operation succeeds and returns valid GridCell
        Assert.That(result.IsT0, Is.True, "TryGetCell should return GridCell for valid coordinates");
        var cell = result.AsT0;
        Assert.That(cell.Row, Is.EqualTo(row), "GridCell should have correct row");
        Assert.That(cell.Column, Is.EqualTo(column), "GridCell should have correct column");
    }

    [Test]
    public void TryGetCell_GivenInvalidRowCoordinate_WhenGettingCell_ThenReturnsCellOutOfBounds()
    {
        // Given a 4x4 grid with invalid row coordinate
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var invalidRow = 10;
        var validColumn = 1;

        // When attempting to get the cell
        var result = _grid.TryGetCell(invalidRow, validColumn);

        // Then operation fails with CellOutOfBounds
        Assert.That(result.IsT1, Is.True, "TryGetCell should return CellOutOfBounds for invalid row");
    }

    [Test]
    public void TryGetCell_GivenInvalidColumnCoordinate_WhenGettingCell_ThenReturnsCellOutOfBounds()
    {
        // Given a 4x4 grid with invalid column coordinate
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var validRow = 1;
        var invalidColumn = -5;

        // When attempting to get the cell
        var result = _grid.TryGetCell(validRow, invalidColumn);

        // Then operation fails with CellOutOfBounds
        Assert.That(result.IsT1, Is.True, "TryGetCell should return CellOutOfBounds for invalid column");
    }

    [Test]
    public void TryGetCell_GivenBothInvalidCoordinates_WhenGettingCell_ThenReturnsCellOutOfBounds()
    {
        // Given a 4x4 grid with both invalid coordinates
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var invalidRow = -1;
        var invalidColumn = 10;

        // When attempting to get the cell
        var result = _grid.TryGetCell(invalidRow, invalidColumn);

        // Then operation fails with CellOutOfBounds
        Assert.That(result.IsT1, Is.True, "TryGetCell should return CellOutOfBounds for both invalid coordinates");
    }

    #endregion

    #region IsColumnFull Query Tests

    [Test]
    public void IsColumnFull_GivenCompletelyFullColumn_WhenChecking_ThenReturnsTrue()
    {
        // Given a grid where column 1 is completely full
        var gridData = new Element?[,]
        {
            { null, new("A"), null, null },
            { null, new("B"), null, null },
            { null, new("C"), null, null },
            { null, new("D"), null, null }
        };
        _grid = CreateGrid(gridData);

        // When checking if column 1 is full
        var result = _grid.IsColumnFull(1);

        // Then operation succeeds and returns true
        Assert.That(result.IsT0, Is.True, "IsColumnFull should return bool for valid column");
        Assert.That(result.AsT0, Is.True, "Column should be reported as full");
    }

    [Test]
    public void IsColumnFull_GivenPartiallyFullColumn_WhenChecking_ThenReturnsFalse()
    {
        // Given a grid where column 2 is partially full
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, new("A"), null },
            { null, null, null, null },
            { null, null, new("B"), null }
        };
        _grid = CreateGrid(gridData);

        // When checking if column 2 is full
        var result = _grid.IsColumnFull(2);

        // Then operation succeeds and returns false
        Assert.That(result.IsT0, Is.True, "IsColumnFull should return bool for valid column");
        Assert.That(result.AsT0, Is.False, "Partially full column should be reported as not full");
    }

    [Test]
    public void IsColumnFull_GivenEmptyColumn_WhenChecking_ThenReturnsFalse()
    {
        // Given a grid where column 3 is completely empty
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);

        // When checking if column 3 is full
        var result = _grid.IsColumnFull(3);

        // Then operation succeeds and returns false
        Assert.That(result.IsT0, Is.True, "IsColumnFull should return bool for valid column");
        Assert.That(result.AsT0, Is.False, "Empty column should be reported as not full");
    }

    [Test]
    public void IsColumnFull_GivenInvalidColumn_WhenChecking_ThenReturnsColumnOutOfBounds()
    {
        // Given a 4x4 grid with invalid column index
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var invalidColumn = 20;

        // When checking if invalid column is full
        var result = _grid.IsColumnFull(invalidColumn);

        // Then operation fails with ColumnOutOfBounds
        Assert.That(result.IsT1, Is.True, "IsColumnFull should return ColumnOutOfBounds for invalid column");
    }

    #endregion

    #region Enumeration Tests

    [Test]
    public void Enumeration_GivenGridWithMixedCells_WhenIterating_ThenReturnsAllCellsInRowMajorOrder()
    {
        // Given a grid with some occupied and some empty cells
        var gridData = new Element?[,]
        {
            { new("A"), null, null, null },
            { null, new("B"), null, null },
            { null, null, new("C"), null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);

        // When iterating through the grid
        var allCells = _grid.ToList();

        // Then all cells are returned in row-major order (row 0, then row 1, etc.)
        Assert.That(allCells.Count, Is.EqualTo(16), "4x4 grid should have 16 cells");

        // Check that cells are in row-major order
        for (int expectedRow = 0; expectedRow < 4; expectedRow++)
        {
            for (int expectedCol = 0; expectedCol < 4; expectedCol++)
            {
                var cellIndex = expectedRow * 4 + expectedCol;
                var cell = allCells[cellIndex];
                Assert.That(cell.IsInRow(expectedRow), Is.True, $"Cell at index {cellIndex} should be in row {expectedRow}");
                Assert.That(cell.IsInColumn(expectedCol), Is.True, $"Cell at index {cellIndex} should be in column {expectedCol}");
            }
        }

        // Verify specific occupied cells
        var topLeftCell = allCells[0]; // (0,0)
        Assert.That(topLeftCell.IsOccupied(), Is.True, "Top-left cell should be occupied");
        Assert.That(topLeftCell.MatchesWith(new ElementCell<Element>(new Element("A"), new GridCell(0, 0))), Is.True, "Top-left should contain element A");
    }

    [Test]
    public void Enumeration_GivenEmptyGrid_WhenIterating_ThenReturnsAllEmptyCells()
    {
        // Given a completely empty grid
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);

        // When iterating through the grid
        var allCells = _grid.ToList();

        // Then all cells are empty
        Assert.That(allCells.Count, Is.EqualTo(16), "4x4 grid should have 16 cells");
        Assert.That(allCells.All(cell => !cell.IsOccupied()), Is.True, "All cells should be empty");
    }

    [Test]
    public void Enumeration_GivenFullGrid_WhenIterating_ThenReturnsAllOccupiedCells()
    {
        // Given a completely full grid
        var gridData = new Element?[,]
        {
            { new("R0C0"), new("R0C1"), new("R0C2"), new("R0C3") },
            { new("R1C0"), new("R1C1"), new("R1C2"), new("R1C3") },
            { new("R2C0"), new("R2C1"), new("R2C2"), new("R2C3") },
            { new("R3C0"), new("R3C1"), new("R3C2"), new("R3C3") }
        };
        _grid = CreateGrid(gridData);

        // When iterating through the grid
        var allCells = _grid.ToList();

        // Then all cells are occupied
        Assert.That(allCells.Count, Is.EqualTo(16), "4x4 grid should have 16 cells");
        Assert.That(allCells.All(cell => cell.IsOccupied()), Is.True, "All cells should be occupied");
    }

    #endregion
}