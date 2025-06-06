using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.ADT;

public interface IMatchingStrategy<TElement> where TElement : IEquatable<TElement>
{
    List<IMatch<TElement>> FindMatches(IEnumerable<ElementCell<TElement>> cells);
}