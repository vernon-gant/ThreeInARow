using System.Collections;
using OneOf;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public abstract class BaseMatch<TElement>(HashSet<Cell<TElement>> cells) : IMatch<TElement>
{
    private HashSet<Cell<TElement>> _cells = cells;

    public int Count => _cells.Count;

    public OneOf<HashSet<Cell<TElement>>, MatchDoesNotIntersect> Merge(IMatch<TElement> other)
    {
        var intersection = _cells.Intersect(other).ToList();

        if (intersection.Count == 0)
            return new MatchDoesNotIntersect();

        var mergedCells = new HashSet<Cell<TElement>>(_cells);
        foreach (var cell in other)
        {
            mergedCells.Add(cell);
        }

        return mergedCells;
    }

    public bool Intersects(IMatch<TElement> other) => _cells.Intersect(other).Any();

    public IEnumerator<Cell<TElement>> GetEnumerator() => _cells.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public abstract TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor);
}