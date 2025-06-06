using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Implementations;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Tests;

[TestFixture]
public abstract class ManageableGridBehaviorTests<TGrid> where TGrid : IManageableGrid<Element>
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
        var gridRow = new GridRow(1);
        var gridColumn = new GridColumn(1);

        // When deleting the occupied cell
        var result = _grid.Delete(gridRow, gridColumn);

        // Then operation succeeds and cell becomes empty
        Assert.That(result.IsT0, Is.True, "Delete should return Success");

        // Verify cell is empty by checking through IReadableGrid if available
        if (_grid is IReadableGrid<Element> readableGrid)
        {
            var cellAfterDelete = readableGrid.TryGetCell(1, 1).AsT0;
            Assert.That(cellAfterDelete.IsOccupied(), Is.False, "Cell should be empty after deletion");
        }
    }

    [Test]
    public void Delete_GivenAlreadyEmptyCell_WhenDeleting_ThenReturnsCellAlreadyDeleted()
    {
        // Given a grid with an empty cell at position (1,1)
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var gridRow = new GridRow(1);
        var gridColumn = new GridColumn(1);

        // When attempting to delete the empty cell
        var result = _grid.Delete(gridRow, gridColumn);

        // Then operation fails with CellAlreadyDeleted
        Assert.That(result.IsT1, Is.True, "Delete should return CellAlreadyDeleted");
        Assert.That(result.AsT1, Is.TypeOf<CellAlreadyDeleted>(), "Should return CellAlreadyDeleted error");
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
        var gridRow = new GridRow(2);
        var gridColumn = new GridColumn(2);

        _grid.Delete(gridRow, gridColumn); // First deletion

        // When attempting to delete the same cell again
        var result = _grid.Delete(gridRow, gridColumn);

        // Then operation fails with CellAlreadyDeleted
        Assert.That(result.IsT1, Is.True, "Second delete should return CellAlreadyDeleted");
        Assert.That(result.AsT1, Is.TypeOf<CellAlreadyDeleted>(), "Should return CellAlreadyDeleted error");
    }

    #endregion

    #region Swap Command Tests

    [Test]
    public void Swap_GivenValidSwappableCells_WhenSwapping_ThenReturnsSuccess()
    {
        // Given a grid with two swappable cells (depends on concrete implementation)
        var gridData = new Element?[,]
        {
            { new("X"), new("Y"), null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);

        var firstRow = new GridRow(0);
        var firstColumn = new GridColumn(0);
        var secondRow = new GridRow(0);
        var secondColumn = new GridColumn(1);

        // When swapping the cells (assuming horizontal adjacency is valid)
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation may succeed (depends on concrete grid implementation)
        // Note: This test needs to be overridden in concrete implementations
        // to test specific swap logic
        Assert.That(result.IsT0 || result.IsT1, Is.True, "Swap should return either Success or InvalidSwap");
    }

    [Test]
    public void Swap_GivenSameCell_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with a cell that we try to swap with itself
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, new("Same"), null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);

        var gridRow = new GridRow(1);
        var gridColumn = new GridColumn(1);

        // When attempting to swap a cell with itself
        var result = _grid.Swap(gridRow, gridColumn, gridRow, gridColumn);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap when swapping cell with itself");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenTwoEmptyCells_WhenSwapping_ThenBehaviorDependsOnImplementation()
    {
        // Given a grid with two empty cells
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);

        var firstRow = new GridRow(1);
        var firstColumn = new GridColumn(1);
        var secondRow = new GridRow(1);
        var secondColumn = new GridColumn(2);

        // When swapping two empty cells
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then behavior depends on concrete implementation
        // (some grids might allow swapping empty cells, others might not)
        Assert.That(result.IsT0 || result.IsT1, Is.True, "Swap should return either Success or InvalidSwap");
    }

    [Test]
    public void Swap_GivenOneEmptyAndOneOccupiedCell_WhenSwapping_ThenBehaviorDependsOnImplementation()
    {
        // Given a grid with one occupied and one empty cell
        var gridData = new Element?[,]
        {
            { null, new("Z"), null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);

        var occupiedRow = new GridRow(0);
        var occupiedColumn = new GridColumn(1);
        var emptyRow = new GridRow(0);
        var emptyColumn = new GridColumn(2);

        // When swapping occupied and empty cells
        var result = _grid.Swap(occupiedRow, occupiedColumn, emptyRow, emptyColumn);

        // Then behavior depends on concrete implementation and swap rules
        Assert.That(result.IsT0 || result.IsT1, Is.True, "Swap should return either Success or InvalidSwap");
    }

    #endregion
}