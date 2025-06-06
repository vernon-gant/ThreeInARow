using System.Collections;
using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Implementations;

public abstract class BaseManageableGrid<TElement>(TElement?[,] grid) : IManageableGrid<TElement>, IFillableGrid<TElement>, IReadableGrid<TElement> where TElement : IEquatable<TElement>
{
    protected BaseManageableGrid(int rows, int columns) : this(new TElement?[rows, columns])
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Grid dimensions must be greater than zero.");
    }

    public OneOf<Success, InvalidSwap> Swap(GridRow firstRow, GridColumn firstColumn, GridRow secondRow, GridColumn secondColumn)
    {
        if (!CanSwap(firstRow, firstColumn, secondRow, secondColumn)) return new InvalidSwap();

        (grid[firstRow.Index, firstColumn.Index], grid[secondRow.Index, secondColumn.Index]) = (grid[secondRow.Index, secondColumn.Index], grid[firstRow.Index, firstColumn.Index]);

        return new Success();
    }

    public OneOf<Success, CellAlreadyDeleted> Delete(GridRow row, GridColumn column)
    {
        if (grid[row.Index, column.Index] == null) return new CellAlreadyDeleted();

        grid[row.Index, column.Index] = default;

        return new Success();
    }

    public OneOf<Success, ColumnIsFull> ShiftDown(GridColumn column)
    {
        if (IsColumnFull(column)) return new ColumnIsFull();

        var lowestEmptyRow = -1;

        for (int row = grid.GetLength(0) - 1; row >= 0; row--)
        {
            if (grid[row, column.Index] != null) continue;

            lowestEmptyRow = row;
            break;
        }

        for (int row = lowestEmptyRow; row > 0; row--)
        {
            grid[row, column.Index] = grid[row - 1, column.Index];
        }

        grid[0, column.Index] = default;

        return new Success();
    }

    public OneOf<Success, ColumnIsFull> AddTop(GridColumn column, TElement element)
    {
        if (IsColumnFull(column) || grid[0, column.Index] != null) return new ColumnIsFull();

        grid[0, column.Index] = element;

        return new Success();
    }

    public OneOf<ElementCell<TElement>, CellOutOfBounds> TryGetCell(int row, int column) => IsValidRow(row) && IsValidColumn(column)
        ? new ElementCell<TElement>(grid[row, column] is null ? new EmptyCell() : grid[row, column]!, new GridRow(row), new GridColumn(column))
        : new CellOutOfBounds();

    public List<GridColumn> FillableColumns
    {
        get
        {
            var fillableColumns = new List<GridColumn>();

            for (int column = 0; column < grid.GetLength(1); column++)
            {
                if (!IsColumnFull(new GridColumn(column)))
                {
                    fillableColumns.Add(new GridColumn(column));
                }
            }

            return fillableColumns;
        }
    }

    public bool IsColumnFull(GridColumn column)
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            if (grid[row, column.Index] == null) return false;
        }

        return true;
    }

    public IEnumerator<ElementCell<TElement>> GetEnumerator()
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int column = 0;  column < grid.GetLength(1); column++)
            {
                yield return new ElementCell<TElement>(grid[row, column] is null ? new EmptyCell() : grid[row, column]!, new GridRow(row), new GridColumn(column));
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private bool IsValidColumn(int column) => column >= 0 && column < grid.GetLength(1);

    private bool IsValidRow(int row) => row >= 0 && row < grid.GetLength(0);

    protected abstract bool CanSwap(GridRow firstRow, GridColumn firstColumn, GridRow secondRow, GridColumn secondColumn);
}