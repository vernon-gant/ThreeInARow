using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations;

public class HorizontalMatchingStrategy<TElement>(int minMatchLength) : BaseMatchingStrategy<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    public override List<IMatch<TElement>> FindMatches(IEnumerable<ElementCell<TElement>> cells)
    {
        return new List<IMatch<TElement>>();
    }
}