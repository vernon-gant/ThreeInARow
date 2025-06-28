using OneOf;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.ADT;

public interface IMatch<TElement> : IEnumerable<Cell<TElement>> where TElement : IEquatable<TElement>
{
    // Commands

    TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor);

    // Queries

    TElement Element { get; }

    int Count { get; }

    OneOf<DistinctCells<TElement>, DoesNotIntersect> Merge(IMatch<TElement> other);

    bool Intersects(IMatch<TElement> other);
}

public struct DoesNotIntersect;