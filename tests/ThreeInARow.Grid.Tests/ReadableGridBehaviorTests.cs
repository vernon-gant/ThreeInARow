using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Implementations;

namespace ThreeInARow.Grid.Tests;

[TestFixture]
public abstract class ReadableGridBehaviorTests<TGrid> where TGrid : IReadableGrid<Element>
{
    private TGrid _grid = default!;

    protected abstract TGrid CreateGrid(Element?[,] gridData);

    #region Helper Methods

    private Element?[,] EmptyGrid(int rows, int columns) => new Element?[rows, columns];

    #endregion

    #region TryGetCell Query Tests

    [Test]
    public void TryGetCell_GivenValidCoordinates_WhenGettingCell_ThenReturnsValidElementCell()
    {
        // Given a 4x4 grid with valid coordinates
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var row = 2;
        var column = 3;

        // When attempting to get the cell
        var result = _grid.TryGetCell(row, column);

        // Then operation succeeds and returns valid ElementCell
        Assert.That(result.IsT0, Is.True, "TryGetCell should return ElementCell for valid coordinates");
        var cell = result.AsT0;
        Assert.That(cell.RowIndex, Is.EqualTo(row), "ElementCell should have correct row index");
        Assert.That(cell.ColumnIndex, Is.EqualTo(column), "ElementCell should have correct column index");
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
        Assert.That(result.AsT1, Is.TypeOf<CellOutOfBounds>(), "Should return CellOutOfBounds error");
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
        Assert.That(result.AsT1, Is.TypeOf<CellOutOfBounds>(), "Should return CellOutOfBounds error");
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
        Assert.That(result.AsT1, Is.TypeOf<CellOutOfBounds>(), "Should return CellOutOfBounds error");
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

        // Verify specific occupied cells
        var topLeftCell = allCells[0]; // (0,0)
        Assert.That(topLeftCell.IsOccupied(), Is.True, "Top-left cell should be occupied");
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