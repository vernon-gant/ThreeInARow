using System.Diagnostics;
using OneOf;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Implementations.Queries;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public class VerticalMatchingStrategy<TElement>(int minMatchLength) : BaseMatchingStrategy<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    public override List<IMatch<TElement>> FindMatches(IEnumerable<ElementCell<TElement>> cells) => cells.GroupByColumn().SelectMany(FindVerticalMatchesInColumn).ToList();

    public override OneOf<bool, GridHasEmptyCells, GridHasMatches> HasPotentialMatches(IReadableGrid<TElement> grid)
    {
        if (grid.IsEmpty())
            return new GridHasEmptyCells();

        if (FindMatches(grid).Count != 0)
            return new GridHasMatches();

        return grid.ToColumnDictionary().SelectMany(column => column.Value.ToNLengthSequences(_minMatchLength)).Where(SequenceHasMinLengthMinusOneSameElements).Any(sequence => FormsPotentialMatch(sequence, grid));
    }

    private bool SequenceHasMinLengthMinusOneSameElements(IEnumerable<ElementCell<TElement>> sequence) => sequence.CountBy(cell => cell.Element).Count(pair => pair.Value == _minMatchLength - 1) == 1;

    private static bool FormsPotentialMatch(IEnumerable<ElementCell<TElement>> sequence, IReadableGrid<TElement> grid)
    {
        var elementCells = sequence.ToList();
        var groupedByCount = elementCells.GroupBy(cell => elementCells.Count(c => c.Element.Equals(cell.Element))).ToDictionary(group => group.First(), group => group.Count());

        var distinctCell = groupedByCount.Where(pair => pair.Value == 1).Select(pair => pair.Key).First();
        Debug.Assert(distinctCell.Element.IsT0, "Distinct cell must not be empty");

        var sameElement = groupedByCount.Where(pair => !pair.Key.Equals(distinctCell)).Select(pair => pair.Key).First().Element;
        Debug.Assert(sameElement.IsT0, "All cells with same element must not be empty");

        var leftDistinctCell = distinctCell with { Column = distinctCell.Column - 1 };
        var leftDistinctNeighbor = grid.TryGetCell(leftDistinctCell.Row, leftDistinctCell.Column);
        var rightDistinctCell = distinctCell with { Column = distinctCell.Column + 1 };
        var rightDistinctNeighbor = grid.TryGetCell(rightDistinctCell.Row, rightDistinctCell.Column);

        var bothAreOutOfBounds = leftDistinctNeighbor.IsT1 && rightDistinctNeighbor.IsT1;
        if (bothAreOutOfBounds)
            return false;

        var bothAreOk = leftDistinctNeighbor.IsT0 && rightDistinctNeighbor.IsT0;
        if (bothAreOk)
            return leftDistinctNeighbor.AsT0.Element.Equals(sameElement) || rightDistinctNeighbor.AsT0.Element.Equals(sameElement);

        return leftDistinctNeighbor.IsT0 ? leftDistinctNeighbor.AsT0.Element.Equals(sameElement) : rightDistinctNeighbor.AsT0.Element.Equals(sameElement);
    }

    private IEnumerable<IMatch<TElement>> FindVerticalMatchesInColumn(IGrouping<int, ElementCell<TElement>> columnGroup) =>
        columnGroup
            .OrderBy(cell => cell.RowIndex)
            .Aggregate(seed: new List<List<ElementCell<TElement>>>(), func: GroupConsecutiveMatchingCells)
            .Where(group => group.Count >= _minMatchLength)
            .Select(group => new VerticalMatch<TElement>(group.ToHashSet()));

    private List<List<ElementCell<TElement>>> GroupConsecutiveMatchingCells(List<List<ElementCell<TElement>>> groups, ElementCell<TElement> currentCell)
    {
        var lastGroup = groups.LastOrDefault();

        if (lastGroup == null || !CanExtendGroup(lastGroup, currentCell))
            groups.Add([currentCell]);
        else
            lastGroup.Add(currentCell);

        return groups;
    }

    private static bool CanExtendGroup(List<ElementCell<TElement>> group, ElementCell<TElement> cell)
    {
        var lastCell = group.Last();
        return lastCell.Element.Equals(cell.Element) && lastCell.RowIndex + 1 == cell.RowIndex;
    }
}