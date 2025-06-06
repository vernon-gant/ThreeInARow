using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Tests;

public static class CellsFromGridConverterExtensions
{
    public static IEnumerable<ElementCell<TElement>> CreateCellsFromGrid<TElement>(this ICellsFromGridConverter _, TElement?[,] grid)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                var element = grid[row, col];
                yield return new ElementCell<TElement>(element is null ? new EmptyCell() : element, new GridRow(row), new GridColumn(col));
            }
        }
    }
}