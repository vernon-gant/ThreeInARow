using System.Diagnostics;
using OneOf;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Implementations.Queries;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public abstract class VerticalHorizontalMatchingStrategyBase<TElement>(int minMatchLength) : BaseMatchingStrategy<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    public override List<IMatch<TElement>> FindMatches(IReadableGrid<TElement> grid) => grid.Where(cell => cell.IsOccupied).GroupBy(GroupingKey()).SelectMany(FindMatches).ToList();

    private IEnumerable<IMatch<TElement>> FindMatches(IGrouping<int, Cell<TElement>> rowGroup) =>
        rowGroup.OrderBy(OrderByKey()).Aggregate(seed: new List<List<Cell<TElement>>>(), func: GroupConsecutiveMatchingCells).Where(group => group.Count >= _minMatchLength).Select(CreateMatch);

    private List<List<Cell<TElement>>> GroupConsecutiveMatchingCells(List<List<Cell<TElement>>> groups, Cell<TElement> currentCell)
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

    private bool SequenceHasMinLengthMinusOneSameElements(IEnumerable<Cell<TElement>> sequence) => sequence.CountBy(cell => cell.Content).Count(pair => pair.Value == _minMatchLength - 1) == 1;

    private bool FormsPotentialMatch(IEnumerable<Cell<TElement>> sequence, IReadableGrid<TElement> grid)
    {
        var cells = sequence.ToList();
        var groupedByCount = cells.CountBy(cell => cell.Content).ToDictionary();

        Debug.Assert(groupedByCount.Count == 2, "There should be exactly two distinct contents in the sequence");

        var distinctCell = cells.First(cell => groupedByCount[cell.Content] == 1);
        var (firstNeighbor, secondNeighbor) = GetNeighborCells(distinctCell, grid);

        var bothAreOutOfBounds = firstNeighbor.IsT1 && secondNeighbor.IsT1;
        if (bothAreOutOfBounds)
            return false;

        var sameCellsContent = groupedByCount.Where(pair => pair.Value != 1).Select(pair => pair.Key).FirstOrDefault();
        var bothAreOk = firstNeighbor.IsT0 && secondNeighbor.IsT0;
        if (bothAreOk)
            return firstNeighbor.AsT0.Content.Equals(sameCellsContent) || secondNeighbor.AsT0.Content.Equals(sameCellsContent);

        return firstNeighbor.IsT0 ? firstNeighbor.AsT0.Content.Equals(sameCellsContent) : secondNeighbor.IsT0 && secondNeighbor.AsT0.Content.Equals(sameCellsContent);
    }

    protected abstract (OneOf<Cell<TElement>, CellOutOfBounds> First, OneOf<Cell<TElement>, CellOutOfBounds> Second) GetNeighborCells(Cell<TElement> cell, IReadableGrid<TElement> grid);

    protected abstract bool CanExtendGroup(List<Cell<TElement>> group, Cell<TElement> cell);

    protected abstract Func<Cell<TElement>, int> GroupingKey();

    protected abstract Func<Cell<TElement>, int> OrderByKey();
}