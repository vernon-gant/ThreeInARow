using ThreeInARow.Grid.Matching.Implementations;

namespace ThreeInARow.Grid.Matching.ADT;

public interface IMatchVisitor<TResult, TElement>
{
    TResult Visit(HorizontalMatch<TElement> match);

    TResult Visit(VerticalMatch<TElement> match);
}