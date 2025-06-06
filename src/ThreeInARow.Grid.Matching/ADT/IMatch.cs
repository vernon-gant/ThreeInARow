using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.ADT;

public interface IMatch<TElement> : IEnumerable<ElementCell<TElement>>
{
    // Commands

    OneOf<Success, MatchDoesNotIntersect> Merge(IMatch<TElement> other);

    TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor);

    // Queries

    int Count { get; }

    bool Intersects(IMatch<TElement> other);
}

public struct MatchDoesNotIntersect;