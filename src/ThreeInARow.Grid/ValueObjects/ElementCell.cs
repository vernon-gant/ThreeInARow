using OneOf;
using ThreeInARow.Grid.ADT;

namespace ThreeInARow.Grid.ValueObjects;

public class ElementCell<TElement>(OneOf<TElement, EmptyCell> element, GridRow row, GridColumn column)
{
    public bool IsOccupied() => element.IsT0;

    public bool IsInColumn(int toCompare) => column.Index == toCompare;

    public bool IsInRow(int toCompare) => row.Index == toCompare;
}

public struct EmptyCell;