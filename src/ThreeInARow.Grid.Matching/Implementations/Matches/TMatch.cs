using OneOf;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public class TMatch<TElement> : DistinctCellsMatch<TElement> where TElement : IEquatable<TElement>
{
    public override TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor) => visitor.Visit(this);

    private TMatch(DistinctCells<TElement> cells) : base(cells) { }

    public static OneOf<TMatch<TElement>, DifferentContentFound> Create(DistinctCells<TElement> cells) => CreateMatch(cells, c => new TMatch<TElement>(c));
}