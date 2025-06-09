using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public class VerticalMatch<TElement>(HashSet<Cell<TElement>> cells) : BaseMatch<TElement>(cells)
{
    public override TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor) => visitor.Visit(this);
}