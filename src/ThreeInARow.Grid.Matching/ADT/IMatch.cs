﻿using OneOf;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.ADT;

public interface IMatch<TElement> : IEnumerable<Cell<TElement>>
{
    // Commands

    TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor);

    // Queries

    int Count { get; }

    OneOf<HashSet<Cell<TElement>>, MatchDoesNotIntersect> Merge(IMatch<TElement> other);

    bool Intersects(IMatch<TElement> other);
}

public struct MatchDoesNotIntersect;