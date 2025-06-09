using NUnit.Framework;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.TestingUtilities;

public static class MGridTestUtilityExtensions
{
    public static IReadableGrid<TElement> CreateTestReadableGrid<TElement>(this MGridTestUtility _, TElement?[,] grid)
    {
        CellContent<TElement>[,] testGrid = new CellContent<TElement>[grid.GetLength(0), grid.GetLength(1)];
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                testGrid[row, col] = grid[row, col] is null ? new EmptyCell() : grid[row, col]!;
            }
        }

        return new TestReadableGrid<TElement>(testGrid);
    }

    public static void AssertGridMatches<TElement>(this MGridTestUtility _, IReadableGrid<TElement> grid, TElement?[,] expected)
    {
        foreach (var cell in grid)
        {
            var row = cell.RowIndex;
            var col = cell.ColumnIndex;

            var expectedValue = expected[row, col];
            var actualValue = cell.IsOccupied ? cell.Content : default;
            Assert.That(actualValue, Is.EqualTo(expectedValue), $"Mismatch at ({row}, {col}): expected {expectedValue}, got {actualValue}");
        }
    }
}