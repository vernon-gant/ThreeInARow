using ThreeInARow.Grid.Matching.Implementations;
using ThreeInARow.Grid.Matching.Implementations.Matches;

namespace ThreeInARow.Grid.Matching.ADT;

public interface IMatchVisitor<TResult, TElement>
{
    TResult Visit(HorizontalMatch<TElement> match);

    TResult Visit(VerticalMatch<TElement> match);

    TResult Visit(TMatch<TElement> match);

    TResult Visit(LMatch<TElement> match);
}