using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public class LMatchingStrategy<TElement>(int minMatchLength, HorizontalMatchingStrategy<TElement> horizontalStrategy, VerticalMatchingStrategy<TElement> verticalStrategy)
    : CombinedFiguresMatchingStrategy<TElement>(minMatchLength, horizontalStrategy, verticalStrategy) where TElement : IEquatable<TElement>
{
    private const int IntersectionPointCount = 3;

    protected override int FigureIntersectionPointCount() => IntersectionPointCount;

    protected override IMatch<TElement> CreateMatch(IEnumerable<ElementCell<TElement>> cells) => new LMatch<TElement>(cells.ToHashSet());
}