using OneOf;
using ThreeInARow.Grid.Matching.ADT;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public class VerticalMatch<TElement> : DistinctCellsMatch<TElement> where TElement : IEquatable<TElement>
{
    public override TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor) => visitor.Visit(this);

    private VerticalMatch(DistinctCells<TElement> cells) : base(cells) { }

    public static OneOf<VerticalMatch<TElement>, DifferentContentFound> Create(DistinctCells<TElement> cells) => CreateMatch(cells, c => new VerticalMatch<TElement>(c));
}