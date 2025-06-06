using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations;

public abstract class BaseMatchingStrategy<TElement>(int minMatchLength) : IMatchingStrategy<TElement>
    where TElement : IEquatable<TElement>
{
    public abstract List<IMatch<TElement>> FindMatches(IEnumerable<ElementCell<TElement>> cells);
}