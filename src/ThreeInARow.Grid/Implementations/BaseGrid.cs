using System.Collections;
using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Implementations;

public abstract class BaseManageableGrid<TElement> : IManageableGrid<TElement>, IFillableGrid<TElement>, IReadableGrid<TElement> where TElement : IEquatable<TElement>
{
    private readonly CellContent<TElement>[,] _grid;

    protected BaseManageableGrid(TElement?[,] gridData)
    {
        _grid = new CellContent<TElement>[gridData.GetLength(0), gridData.GetLength(1)];
        for (int row = 0; row < gridData.GetLength(0); row++)
        {
            for (int column = 0; column < gridData.GetLength(1); column++)
            {
                _grid[row, column] = gridData[row, column] is null ? new EmptyCell() : gridData[row, column]!;
            }
        }
    }

    public OneOf<Success, InvalidSwap> Swap(Cell<TElement> firstCell, Cell<TElement> secondCell)
    {
        if (!CanSwap(firstCell, secondCell)) return new InvalidSwap();

        (_grid[firstCell.RowIndex, firstCell.ColumnIndex], _grid[secondCell.RowIndex, secondCell.ColumnIndex]) = (_grid[secondCell.RowIndex, secondCell.ColumnIndex], _grid[firstCell.RowIndex, firstCell.ColumnIndex]);

        return new Success();
    }

    public OneOf<Success, CellAlreadyDeleted> Delete(Cell<TElement> cell)
    {
        if (_grid[cell.RowIndex, cell.ColumnIndex].IsEmpty) return new CellAlreadyDeleted();

        _grid[cell.RowIndex, cell.ColumnIndex] = new EmptyCell();

        return new Success();
    }

    public OneOf<Success, ColumnIndexOutOfBounds, CanNotShiftDown> ShiftDown(int columnIndex)
    {
        if (!IsValidColumnIndex(columnIndex)) return new ColumnIndexOutOfBounds();

        if (IsColumnFull(columnIndex).AsT0 || !CanShiftDown(columnIndex).AsT0) return new CanNotShiftDown();

        var lowestEmptyRow = -1;

        for (int row = _grid.GetLength(0) - 1; row >= 0; row--)
        {
            if (_grid[row, columnIndex].IsOccupied) continue;

            lowestEmptyRow = row;
            break;
        }

        for (int row = lowestEmptyRow; row > 0; row--)
        {
            _grid[row, columnIndex] = _grid[row - 1, columnIndex];
        }

        _grid[0, columnIndex] = new EmptyCell();

        return new Success();
    }

    public OneOf<Success, ColumnIndexOutOfBounds, CanNotAddTop> AddTop(int columnIndex, TElement element)
    {
        if (!IsValidColumnIndex(columnIndex)) return new ColumnIndexOutOfBounds();

        if (IsColumnFull(columnIndex).AsT0 || _grid[0, columnIndex].IsOccupied) return new CanNotAddTop();

        _grid[0, columnIndex] = element;

        return new Success();
    }

    public OneOf<bool, ColumnIndexOutOfBounds> CanShiftDown(int columnIndex)
    {
        if (!IsValidColumnIndex(columnIndex)) return new ColumnIndexOutOfBounds();

        if (IsColumnFull(columnIndex).AsT0) return false;

        return Enumerable.Range(0, _grid.GetLength(0) - 1).Any(row => _grid[row, columnIndex].IsOccupied && _grid[row + 1, columnIndex].IsEmpty);
    }

    public List<int> FillableColumns
    {
        get
        {
            var fillableColumns = new List<int>();

            for (int column = 0; column < _grid.GetLength(1); column++)
            {
                if (!IsColumnFull(column).AsT0)
                {
                    fillableColumns.Add(column);
                }
            }

            return fillableColumns;
        }
    }

    private bool IsValidColumnIndex(int columnIndex) => columnIndex >= 0 && columnIndex < _grid.GetLength(1);

    public OneOf<bool, ColumnIndexOutOfBounds> IsColumnFull(int columnIndex)
    {
        if (!IsValidColumnIndex(columnIndex)) return new ColumnIndexOutOfBounds();

        for (int row = 0; row < _grid.GetLength(0); row++)
        {
            if (_grid[row, columnIndex].IsEmpty) return false;
        }

        return true;
    }

    public IEnumerator<Cell<TElement>> GetEnumerator()
    {
        for (int row = 0; row < _grid.GetLength(0); row++)
        {
            for (int column = 0; column < _grid.GetLength(1); column++)
            {
                yield return Cell<TElement>.FromGrid(this, row, column).AsT0;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected abstract bool CanSwap(Cell<TElement> firstCell, Cell<TElement> secondCell);

    public int RowCount => _grid.GetLength(0);

    public int ColumnCount => _grid.GetLength(1);
    public OneOf<CellContent<TElement>, CellOutOfBounds> ContentAt(int row, int column)
    {
        if (row < 0 || row >= RowCount || column < 0 || column >= ColumnCount)
            return new CellOutOfBounds();

        return _grid[row, column];
    }
}