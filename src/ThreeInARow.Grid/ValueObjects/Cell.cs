using OneOf;
using ThreeInARow.Grid.ADT;

namespace ThreeInARow.Grid.ValueObjects;

public readonly record struct Cell<TElement>
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

    public OneOf<Cell<TElement>, CellOutOfBounds> Top(IReadableGrid<TElement> grid)
    {
        if (RowIndex == 0)
            return new CellOutOfBounds();

        return new Cell<TElement>(grid.ContentAt(RowIndex - 1, ColumnIndex).AsT0, RowIndex - 1, ColumnIndex);
    }

    public OneOf<Cell<TElement>, CellOutOfBounds> Bottom(IReadableGrid<TElement> grid)
    {
        if (RowIndex == grid.RowCount - 1)
            return new CellOutOfBounds();

        return new Cell<TElement>(grid.ContentAt(RowIndex + 1, ColumnIndex).AsT0, RowIndex + 1, ColumnIndex);
    }

    public OneOf<Cell<TElement>, CellOutOfBounds> Left(IReadableGrid<TElement> grid)
    {
        if (ColumnIndex == 0)
            return new CellOutOfBounds();

        return new Cell<TElement>(grid.ContentAt(RowIndex, ColumnIndex - 1).AsT0, RowIndex, ColumnIndex - 1);
    }

    public OneOf<Cell<TElement>, CellOutOfBounds> Right(IReadableGrid<TElement> grid)
    {
        if (ColumnIndex == grid.ColumnCount - 1)
            return new CellOutOfBounds();

        return new Cell<TElement>(grid.ContentAt(RowIndex, ColumnIndex + 1).AsT0, RowIndex, ColumnIndex + 1);
    }

    public bool HasSameCoordinatesAs(Cell<TElement> other) => RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;

    public static OneOf<Cell<TElement>, CellOutOfBounds> FromGrid(IReadableGrid<TElement> grid, int row, int column)
    {
        var cellContent = grid.ContentAt(row, column);

        if (cellContent.IsT0) return new Cell<TElement>(cellContent.AsT0, row, column);

        return new CellOutOfBounds();
    }
}