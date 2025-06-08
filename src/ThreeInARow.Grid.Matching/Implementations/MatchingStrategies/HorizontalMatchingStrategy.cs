using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public class HorizontalMatchingStrategy<TElement>(int minMatchLength) : VerticalHorizontalMatchingStrategyBase<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    protected override (ElementCell<TElement> First, ElementCell<TElement> Second) GetNeighborCells(ElementCell<TElement> cell)
    {
        var aboveCell = cell with { Row = cell.Row - 1 };
        var belowCell = cell with { Row = cell.Row + 1 };
        return (aboveCell, belowCell);
    }

    protected override bool CanExtendGroup(List<ElementCell<TElement>> group, ElementCell<TElement> cell)
    {
        var lastCell = group.Last();
        return lastCell.Element.Equals(cell.Element) && lastCell.ColumnIndex + 1 == cell.ColumnIndex;
    }

    protected override Func<ElementCell<TElement>, int> GroupingKey() => cell => cell.RowIndex;

    protected override Func<ElementCell<TElement>, int> OrderByKey() => cell => cell.ColumnIndex;

    protected override IMatch<TElement> CreateMatch(IEnumerable<ElementCell<TElement>> cells) => new HorizontalMatch<TElement>(cells.ToHashSet());
}