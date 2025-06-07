using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public class VerticalMatchingStrategy<TElement>(int minMatchLength) : BaseMatchingStrategy<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    private readonly int _minMatchLength = minMatchLength;

    public override List<IMatch<TElement>> FindMatches(IEnumerable<ElementCell<TElement>> cells)
    {
        return cells
            .Where(cell => cell.IsOccupied())
            .GroupBy(cell => cell.ColumnIndex)
            .SelectMany(FindVerticalMatchesInColumn)
            .ToList();
    }

    private IEnumerable<IMatch<TElement>> FindVerticalMatchesInColumn(IGrouping<int, ElementCell<TElement>> columnGroup)
    {
        return columnGroup
            .OrderBy(cell => cell.RowIndex)
            .Aggregate(seed: new List<List<ElementCell<TElement>>>(), func: GroupConsecutiveMatchingCells)
            .Where(group => group.Count >= _minMatchLength)
            .Select(group => new VerticalMatch<TElement>(group.ToHashSet()));
    }

    private List<List<ElementCell<TElement>>> GroupConsecutiveMatchingCells(List<List<ElementCell<TElement>>> groups, ElementCell<TElement> currentCell)
    {
        var lastGroup = groups.LastOrDefault();

        if (lastGroup == null ||
            !CanExtendGroup(lastGroup, currentCell))
        {
            groups.Add([currentCell]);
        }
        else
        {
            lastGroup.Add(currentCell);
        }

        return groups;
    }

    private bool CanExtendGroup(List<ElementCell<TElement>> group, ElementCell<TElement> cell)
    {
        var lastCell = group.Last();
        return lastCell.Element.Equals(cell.Element) && lastCell.RowIndex + 1 == cell.RowIndex;
    }
}