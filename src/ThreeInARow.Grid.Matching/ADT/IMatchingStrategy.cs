using OneOf;
using ThreeInARow.Grid.ADT;

namespace ThreeInARow.Grid.Matching.ADT;

public interface IMatchingStrategy<TElement> where TElement : IEquatable<TElement>
{
    List<IMatch<TElement>> FindMatches(IReadableGrid<TElement> grid);

    OneOf<bool, GridHasEmptyCells, GridHasMatches> HasPotentialMatches(IReadableGrid<TElement> grid);
}

public struct GridHasEmptyCells;

public struct GridHasMatches;