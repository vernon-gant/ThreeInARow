using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Implementations;

/// <summary>
/// Default implementation of a grid that allows swapping elements horizontally or vertically.
/// </summary>
public class HorizontalVerticalSwapGrid<TElement> : BaseManageableGrid<TElement> where TElement : IEquatable<TElement>
{
    public HorizontalVerticalSwapGrid(TElement?[,] gridData) : base(gridData) { }

    protected override bool CanSwap(Cell<TElement> firstCell, Cell<TElement> secondCell)
    {
        var isHorizontal = firstCell.RowIndex == secondCell.RowIndex && Math.Abs(firstCell.ColumnIndex - secondCell.ColumnIndex) == 1;
        var isVertical = firstCell.ColumnIndex == secondCell.ColumnIndex && Math.Abs(firstCell.RowIndex - secondCell.RowIndex) == 1;

        return isHorizontal || isVertical;
    }
}