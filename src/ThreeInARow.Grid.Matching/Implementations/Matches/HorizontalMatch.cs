using OneOf;
using ThreeInARow.Grid.Matching.ADT;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public class HorizontalMatch<TElement> : DistinctCellsMatch<TElement> where TElement : IEquatable<TElement>
{
    public override TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor) => visitor.Visit(this);

    private HorizontalMatch(DistinctCells<TElement> cells) : base(cells) { }

    public static OneOf<HorizontalMatch<TElement>, DifferentContentFound> Create(DistinctCells<TElement> cells) => CreateMatch(cells, c => new HorizontalMatch<TElement>(c));
}