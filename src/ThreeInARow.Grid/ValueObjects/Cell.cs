using OneOf;
using ThreeInARow.Grid.ADT;

namespace ThreeInARow.Grid.ValueObjects;

public readonly struct Cell<TElement> : IEquatable<Cell<TElement>>
{
    private readonly CellContent<TElement> _content;

    private Cell(CellContent<TElement> content, int row, int column)
    {
        RowIndex = row;
        ColumnIndex = column;
        _content = content;
    }

    public int RowIndex { get; }

    public int ColumnIndex { get; }

    public bool IsOccupied => _content.IsOccupied;

    public bool IsEmpty => _content.IsEmpty;

    public TElement Content {
        get
        {
            if (_content.IsT0)
                return _content.AsT0;

            throw new InvalidOperationException("Cell is empty.");
        }
    }

    public bool Equals(Cell<TElement> other) => RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;

    public override bool Equals(object? obj) => obj is Cell<TElement> cell && Equals(cell);

    public override int GetHashCode() => HashCode.Combine(RowIndex, ColumnIndex);

    public static OneOf<Cell<TElement>, CellOutOfBounds> FromGrid(IReadableGrid<TElement> grid, int row, int column)
    {
        var cellContent = grid.ContentAt(row, column);

        if (cellContent.IsT0) return new Cell<TElement>(cellContent.AsT0, row, column);

        return new CellOutOfBounds();
    }
}