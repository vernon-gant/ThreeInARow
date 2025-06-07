using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public class TMatch<TElement>(HashSet<ElementCell<TElement>> cells) : BaseMatch<TElement>(cells)
{
    public override TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor) => visitor.Visit(this);
}