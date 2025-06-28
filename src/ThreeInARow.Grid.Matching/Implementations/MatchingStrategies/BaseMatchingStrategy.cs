using OneOf;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public abstract class BaseMatchingStrategy<TElement>(int minMatchLength) : IMatchingStrategy<TElement> where TElement : IEquatable<TElement>
{
    protected readonly int _minMatchLength = minMatchLength;

    public abstract List<IMatch<TElement>> FindMatches(IReadableGrid<TElement> grid);

    public abstract OneOf<bool, GridHasEmptyCells, GridHasMatches> HasPotentialMatches(IReadableGrid<TElement> grid);

    protected abstract IMatch<TElement> CreateMatch(DistinctCells<TElement> cells);
}