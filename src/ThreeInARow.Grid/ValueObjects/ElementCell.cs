using OneOf;

namespace ThreeInARow.Grid.ValueObjects;

public record ElementCell<TElement>(OneOf<TElement, EmptyCell> Element, GridRow Row, GridColumn Column)
{
    public bool IsOccupied() => Element.IsT0;

    public int ColumnIndex => Column.Index;

    public int RowIndex => Row.Index;
}

public struct EmptyCell;