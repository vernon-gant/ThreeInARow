using System.Diagnostics;
using OneOf;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Implementations.Queries;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public abstract class VerticalHorizontalMatchingStrategyBase<TElement>(int minMatchLength) : BaseMatchingStrategy<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    public override List<IMatch<TElement>> FindMatches(IEnumerable<ElementCell<TElement>> cells) => cells.Where(cell => cell.IsOccupied()).GroupBy(GroupingKey()).SelectMany(FindMatches).ToList();

    private IEnumerable<IMatch<TElement>> FindMatches(IGrouping<int, ElementCell<TElement>> rowGroup) =>
        rowGroup.OrderBy(OrderByKey()).Aggregate(seed: new List<List<ElementCell<TElement>>>(), func: GroupConsecutiveMatchingCells).Where(group => group.Count >= _minMatchLength).Select(CreateMatch);

    private List<List<ElementCell<TElement>>> GroupConsecutiveMatchingCells(List<List<ElementCell<TElement>>> groups, ElementCell<TElement> currentCell)
    {
        var lastGroup = groups.LastOrDefault();

        if (lastGroup == null || !CanExtendGroup(lastGroup, currentCell))
            groups.Add([currentCell]);
        else
            lastGroup.Add(currentCell);

        return groups;
    }

    public override OneOf<bool, GridHasEmptyCells, GridHasMatches> HasPotentialMatches(IReadableGrid<TElement> grid)
    {
        if (grid.IsEmpty())
            return new GridHasEmptyCells();

        if (FindMatches(grid).Count != 0)
            return new GridHasMatches();

        return grid.ToColumnDictionary().SelectMany(column => column.Value.ToNLengthSequences(_minMatchLength)).Where(SequenceHasMinLengthMinusOneSameElements).Any(sequence => FormsPotentialMatch(sequence, grid));
    }

    private bool SequenceHasMinLengthMinusOneSameElements(IEnumerable<ElementCell<TElement>> sequence) => sequence.CountBy(cell => cell.Element).Count(pair => pair.Value == _minMatchLength - 1) == 1;

    private bool FormsPotentialMatch(IEnumerable<ElementCell<TElement>> sequence, IReadableGrid<TElement> grid)
    {
        var elementCells = sequence.ToList();
        var groupedByCount = elementCells.GroupBy(cell => elementCells.Count(c => c.Element.Equals(cell.Element))).ToDictionary(group => group.First(), group => group.Count());

        var distinctCell = groupedByCount.Where(pair => pair.Value == 1).Select(pair => pair.Key).First();
        Debug.Assert(distinctCell.Element.IsT0, "Distinct cell must not be empty");

        var sameElement = groupedByCount.Where(pair => !pair.Key.Equals(distinctCell)).Select(pair => pair.Key).First().Element;
        Debug.Assert(sameElement.IsT0, "All cells with same element must not be empty");

        var neighborCells = GetNeighborCells(distinctCell);
        var firstNeighbor = grid.TryGetCell(neighborCells.First.Row, neighborCells.First.Column);
        var secondNeighbor = grid.TryGetCell(neighborCells.Second.Row, neighborCells.Second.Column);

        var bothAreOutOfBounds = firstNeighbor.IsT1 && secondNeighbor.IsT1;
        if (bothAreOutOfBounds)
            return false;

        var bothAreOk = firstNeighbor.IsT0 && secondNeighbor.IsT0;
        if (bothAreOk)
            return firstNeighbor.AsT0.Element.Equals(sameElement) || secondNeighbor.AsT0.Element.Equals(sameElement);

        return firstNeighbor.IsT0 ? firstNeighbor.AsT0.Element.Equals(sameElement) : secondNeighbor.IsT0 && secondNeighbor.AsT0.Element.Equals(sameElement);
    }

    protected abstract (ElementCell<TElement> First, ElementCell<TElement> Second) GetNeighborCells(ElementCell<TElement> cell);

    protected abstract bool CanExtendGroup(List<ElementCell<TElement>> group, ElementCell<TElement> cell);

    protected abstract Func<ElementCell<TElement>, int> GroupingKey();

    protected abstract Func<ElementCell<TElement>, int> OrderByKey();
}