namespace ThreeInARow.Grid.ValueObjects.Extensions;

public static class CellComparisonExtensions
{
    public static bool HasSameCoordinatesAs<TElement>(this Cell<TElement> cell, Cell<TElement> otherCell) => cell.RowIndex == otherCell.RowIndex && cell.ColumnIndex == otherCell.ColumnIndex;
}