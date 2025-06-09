using OneOf;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public class HorizontalMatchingStrategy<TElement>(int minMatchLength) : VerticalHorizontalMatchingStrategyBase<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    protected override (OneOf<Cell<TElement>, CellOutOfBounds> First, OneOf<Cell<TElement>, CellOutOfBounds> Second) GetNeighborCells(Cell<TElement> cell, IReadableGrid<TElement> grid)
    {
        var aboveCell = cell.Top(grid);
        var belowCell = cell.Bottom(grid);
        return (aboveCell, belowCell);
    }

    protected override bool CanExtendGroup(List<Cell<TElement>> group, Cell<TElement> cell)
    {
        var lastCell = group.Last();
        return lastCell.Content.Equals(cell.Content) && lastCell.ColumnIndex + 1 == cell.ColumnIndex;
    }

    protected override Func<Cell<TElement>, int> GroupingKey() => cell => cell.RowIndex;

    protected override Func<Cell<TElement>, int> OrderByKey() => cell => cell.ColumnIndex;

    protected override IMatch<TElement> CreateMatch(IEnumerable<Cell<TElement>> cells) => new HorizontalMatch<TElement>(cells.ToHashSet());
}