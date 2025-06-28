using OneOf;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Implementations.Queries;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;
using ThreeInARow.Grid.ValueObjects.Extensions;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public class VerticalMatchingStrategy<TElement>(int minMatchLength) : VerticalHorizontalMatchingStrategyBase<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    public override List<IMatch<TElement>> FindMatches(IReadableGrid<TElement> grid) => grid.GroupByColumn().SelectMany(FindVerticalMatchesInColumn).ToList();
    protected override IMatch<TElement> CreateMatch(IEnumerable<Cell<TElement>> cells) => new VerticalMatch<TElement>(cells.ToHashSet());

    private IEnumerable<IMatch<TElement>> FindVerticalMatchesInColumn(IGrouping<int, Cell<TElement>> columnGroup) =>
        columnGroup
            .OrderBy(cell => cell.RowIndex)
            .Aggregate(seed: new List<List<Cell<TElement>>>(), func: GroupConsecutiveMatchingCells)
            .Where(group => group.Count >= _minMatchLength)
            .Select(group => new VerticalMatch<TElement>(group.ToHashSet()));

    private List<List<Cell<TElement>>> GroupConsecutiveMatchingCells(List<List<Cell<TElement>>> groups, Cell<TElement> currentCell)
    {
        var lastGroup = groups.LastOrDefault();

        if (lastGroup == null || !CanExtendGroup(lastGroup, currentCell))
            groups.Add([currentCell]);
        else
            lastGroup.Add(currentCell);

        return groups;
    }

    protected override (OneOf<Cell<TElement>, CellOutOfBounds> First, OneOf<Cell<TElement>, CellOutOfBounds> Second) GetNeighborCells(Cell<TElement> cell, IReadableGrid<TElement> grid)
    {
        var leftCell = cell.Left(grid);
        var rightCell = cell.Right(grid);
        return (leftCell, rightCell);
    }

    protected override Func<Cell<TElement>, int> GroupingKey() => cell => cell.ColumnIndex;

    protected override Func<Cell<TElement>, int> OrderByKey() => cell => cell.RowIndex;

    protected override bool CanExtendGroup(List<Cell<TElement>> group, Cell<TElement> cell)
    {
        var lastCell = group.Last();
        return lastCell.Content.Equals(cell.Content) && lastCell.RowIndex + 1 == cell.RowIndex;
    }
}