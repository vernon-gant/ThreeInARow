using System.Collections;
using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations;

public abstract class BaseMatch<TElement>(HashSet<ElementCell<TElement>> cells) : IMatch<TElement>
{
    protected HashSet<ElementCell<TElement>> _cells = cells;

    public int Count => _cells.Count;

    public OneOf<HashSet<ElementCell<TElement>>, MatchDoesNotIntersect> Merge(IMatch<TElement> other)
    {
        var intersection = _cells.Intersect(other).ToList();

        if (intersection.Count == 0)
            return new MatchDoesNotIntersect();

        var mergedCells = new HashSet<ElementCell<TElement>>(_cells);
        foreach (var cell in other)
        {
            mergedCells.Add(cell);
        }

        return mergedCells;
    }

    public bool Intersects(IMatch<TElement> other) => _cells.Intersect(other).Any();

    public IEnumerator<ElementCell<TElement>> GetEnumerator() => _cells.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public abstract TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor);
}