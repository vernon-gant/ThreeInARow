using OneOf;
using ThreeInARow.Grid.ADT;

namespace ThreeInARow.Grid.ValueObjects.Extensions;

public static class CellNavigationExtensions
{
    public static OneOf<Cell<TElement>, CellOutOfBounds> Top<TElement>(this Cell<TElement> cell, IReadableGrid<TElement> grid)
    {
        if (cell.RowIndex == 0)
            return new CellOutOfBounds();

        return Cell<TElement>.FromGrid(grid, cell.RowIndex - 1, cell.ColumnIndex);
    }

    public static OneOf<Cell<TElement>, CellOutOfBounds> Bottom<TElement>(this Cell<TElement> cell, IReadableGrid<TElement> grid)
    {
        if (cell.RowIndex == grid.RowCount - 1)
            return new CellOutOfBounds();

        return Cell<TElement>.FromGrid(grid, cell.RowIndex + 1, cell.ColumnIndex);
    }

    public static OneOf<Cell<TElement>, CellOutOfBounds> Left<TElement>(this Cell<TElement> cell, IReadableGrid<TElement> grid)
    {
        if (cell.ColumnIndex == 0)
            return new CellOutOfBounds();

        return Cell<TElement>.FromGrid(grid, cell.RowIndex, cell.ColumnIndex - 1);
    }

    public static OneOf<Cell<TElement>, CellOutOfBounds> Right<TElement>(this Cell<TElement> cell, IReadableGrid<TElement> grid)
    {
        if (cell.ColumnIndex == grid.ColumnCount - 1)
            return new CellOutOfBounds();

        return Cell<TElement>.FromGrid(grid, cell.RowIndex, cell.ColumnIndex + 1);
    }
}