using System.Collections;
using OneOf;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.TestingUtilities;

public class TestReadableGrid<TElement>(CellContent<TElement>[,] grid) : IReadableGrid<TElement>
{
    public IEnumerator<Cell<TElement>> GetEnumerator()
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                yield return Cell<TElement>.FromGrid(this, row, col).AsT0;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int RowCount => grid.GetLength(0);

    public int ColumnCount => grid.GetLength(1);

    public OneOf<CellContent<TElement>, CellOutOfBounds> ContentAt(int row, int column)
    {
        if (row < 0 || row >= RowCount || column < 0 || column >= ColumnCount)
            return new CellOutOfBounds();

        return grid[row, column];
    }
}