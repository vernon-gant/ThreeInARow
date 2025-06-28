using ThreeInARow.Grid.Matching.ADT;
using OneOf;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public class CrossMatch<TElement> : DistinctCellsMatch<TElement> where TElement : IEquatable<TElement>
{
    public override TResult Accept<TResult>(IMatchVisitor<TResult, TElement> visitor) => visitor.Visit(this);

    private CrossMatch(DistinctCells<TElement> cells) : base(cells) { }

    public static OneOf<CrossMatch<TElement>, DifferentContentFound> Create(DistinctCells<TElement> cells) => CreateMatch(cells, c => new CrossMatch<TElement>(c));
}