using System.Collections;
using System.Diagnostics;
using OneOf;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public abstract class DistinctCellsMatch<TElement> : IMatch<TElement> where TElement : IEquatable<TElement>
{
    private readonly DistinctCells<TElement> _cells;

    protected DistinctCellsMatch(DistinctCells<TElement> cells) => _cells = cells;

    public int Count => _cells.Count;

    public OneOf<DistinctCells<TElement>, DoesNotIntersect> Merge(IMatch<TElement> other)
    {
        var otherCellsResult = DistinctCells<TElement>.Create(other);
        Debug.Assert(otherCellsResult.IsT0, "Expected other to be a DistinctCells<TElement>.");
        return _cells.Merge(otherCellsResult.AsT0);
    }

    public bool Intersects(IMatch<TElement> other)
    {
        var otherCellsResult = DistinctCells<TElement>.Create(other);
        Debug.Assert(otherCellsResult.IsT0, "Expected other to be a DistinctCells<TElement>.");
        return _cells.Intersects(otherCellsResult.AsT0);
    }

    protected static OneOf<TMatch, DifferentContentFound> CreateMatch<TMatch>(DistinctCells<TElement> cells, Func<DistinctCells<TElement>, TMatch> factory) where TMatch : DistinctCellsMatch<TElement>
    {
        var firstContent = cells.First().Content;

        if (!cells.All(cell => cell.Content.Equals(firstContent)))
            return new DifferentContentFound();

        return factory(cells);
    }

    public IEnumerator<Cell<TElement>> GetEnumerator() => _cells.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public abstract TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor);

    public TElement Element => _cells.First().Content;
}

public struct DifferentContentFound;