namespace ThreeInARow.Board;

/// <summary>
/// Default implementation of a grid that allows swapping elements horizontally or vertically.
/// </summary>
public class HorizontalVerticalSwapGrid<TElement>(TElement?[,] grid) : BaseGrid<TElement>(grid) where TElement : IEquatable<TElement>, IVisual
{
    protected override bool CanSwap(GridCell first, GridCell second)
    {
        var isHorizontal = first.Row == second.Row && Math.Abs(first.Column - second.Column) == 1;
        var isVertical = first.Column == second.Column && Math.Abs(first.Row - second.Row) == 1;

        return isHorizontal || isVertical;
    }
}