using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Tests;

public static class CellsFromGridConverterExtensions
{
    public static IEnumerable<ElementCell<TElement>> CreateCellsFromGrid<TElement>(this MGridTestUtility _, TElement?[,] grid)
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

    public static void AssertGridMatches<TElement>(this MGridTestUtility _, IReadableGrid<TElement> grid, TElement?[,] expected)
    {
        foreach (var cell in grid)
        {
            var row = cell.RowIndex;
            var col = cell.ColumnIndex;

            var expectedValue = expected[row, col];
            var actualValue = cell.IsOccupied() ? cell.Element.AsT0 : default;
            Assert.That(actualValue, Is.EqualTo(expectedValue), $"Mismatch at ({row}, {col}): expected {expectedValue}, got {actualValue}");
        }
    }
}