using OneOf;
using ThreeInARow.Grid.Matching.ADT;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public class LMatch <TElement> : DistinctCellsMatch<TElement> where TElement : IEquatable<TElement>
{
    public override TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor) => visitor.Visit(this);

    private LMatch(DistinctCells<TElement> cells) : base(cells) { }

    public static OneOf<LMatch<TElement>, DifferentContentFound> Create(DistinctCells<TElement> cells) => CreateMatch(cells, c => new LMatch<TElement>(c));
}