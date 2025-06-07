using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations;

public class VerticalMatch<TElement>(HashSet<ElementCell<TElement>> cells) : BaseMatch<TElement>(cells)
{
    public override TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor) => visitor.Visit(this);
}