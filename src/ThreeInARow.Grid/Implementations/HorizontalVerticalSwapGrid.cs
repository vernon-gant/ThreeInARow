using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Implementations;

/// <summary>
/// Default implementation of a grid that allows swapping elements horizontally or vertically.
/// </summary>
public class HorizontalVerticalSwapGrid<TElement>(TElement?[,] grid) : BaseManageableGrid<TElement>(grid) where TElement : IEquatable<TElement>
{
    protected override bool CanSwap(GridRow firstRow, GridColumn firstColumn, GridRow secondRow, GridColumn secondColumn)
    {
        var isHorizontal = firstRow.Index == secondRow.Index && Math.Abs(firstColumn.Index - secondColumn.Index) == 1;
        var isVertical = firstColumn.Index == secondColumn.Index && Math.Abs(firstRow.Index - secondRow.Index) == 1;

        return isHorizontal || isVertical;
    }
}