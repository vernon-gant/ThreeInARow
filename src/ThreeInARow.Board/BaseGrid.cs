using System.Collections;
using OneOf;
using OneOf.Types;

namespace ThreeInARow.Board;

public abstract class BaseGrid<TElement>(TElement?[,] grid) : IGrid<TElement> where TElement : IEquatable<TElement>, IVisual
{
    protected BaseGrid(int rows, int columns) : this(new TElement?[rows, columns])
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Grid dimensions must be greater than zero.");
    }

    public OneOf<Success, InvalidSwap> Swap(GridCell first, GridCell second)
    {
        if (!CanSwap(first, second)) return new InvalidSwap();

        (grid[first.Row, first.Column], grid[second.Row, second.Column]) = (grid[second.Row, second.Column], grid[first.Row, first.Column]);

        return new Success();
    }

    public OneOf<Success, CellAlreadyDeleted> Delete(GridCell cell)
    {
        if (grid[cell.Row, cell.Column] == null) return new CellAlreadyDeleted();

        grid[cell.Row, cell.Column] = default;

        return new Success();
    }

    public OneOf<Success, ColumnOutOfBounds, ColumnIsFull> ShiftDown(int column)
    {
        if (!IsValidColumn(column)) return new ColumnOutOfBounds();

        if (IsColumnFull(column).AsT0) return new ColumnIsFull();

        var lowestEmptyRow = -1;

        for (int row = grid.GetLength(0) - 1; row >= 0; row--)
        {
            if (grid[row, column] != null) continue;

            lowestEmptyRow = row;
            break;
        }

        for (int row = lowestEmptyRow; row > 0; row--)
        {
            grid[row, column] = grid[row - 1, column];
        }

        grid[0, column] = default;

        return new Success();
    }

    public OneOf<Success, ColumnOutOfBounds, ColumnIsFull> AddTop(int column, TElement element)
    {
        if (!IsValidColumn(column)) return new ColumnOutOfBounds();

        if (IsColumnFull(column).AsT0 || grid[0, column] != null) return new ColumnIsFull();

        grid[0, column] = element;

        return new Success();
    }

    public OneOf<GridCell, CellOutOfBounds> TryGetCell(int row, int column) => IsValidRow(row) && IsValidColumn(column) ? new GridCell(row, column) : new CellOutOfBounds();

    public OneOf<bool, ColumnOutOfBounds> IsColumnFull(int column)
    {
        if (!IsValidColumn(column)) return new ColumnOutOfBounds();

        for (int row = 0; row < grid.GetLength(0); row++)
        {
            if (grid[row, column] == null) return false;
        }

        return true;
    }

    public IEnumerator<ElementCell<TElement>> GetEnumerator()
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int column = 0;  column < grid.GetLength(1); column++)
            {
                yield return new ElementCell<TElement>(grid[row, column] is null ? new EmptyCell() : grid[row, column]!, new GridCell(row, column));
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private bool IsValidColumn(int column) => column >= 0 && column < grid.GetLength(1);

    private bool IsValidRow(int row) => row >= 0 && row < grid.GetLength(0);

    protected abstract bool CanSwap(GridCell first, GridCell second);
}