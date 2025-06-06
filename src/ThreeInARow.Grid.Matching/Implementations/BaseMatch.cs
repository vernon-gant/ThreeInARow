using System.Collections;
using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations;

public abstract class BaseMatch<TElement>(List<ElementCell<TElement>> cells) : IMatch<TElement>
{
    protected List<ElementCell<TElement>> _cells = cells;

    public OneOf<Success, MatchDoesNotIntersect> Merge(IMatch<TElement> other)
    {
        if (!Intersects(other))
            return new MatchDoesNotIntersect();

        _cells.AddRange(other);
        _cells = _cells.Distinct().ToList();

        return new Success();
    }

    public int Count => _cells.Count;

    public bool Intersects(IMatch<TElement> other) => _cells.Any(other.Contains);

    public IEnumerator<ElementCell<TElement>> GetEnumerator() => _cells.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public abstract TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor);
}