namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public class CrossMatchingStrategy<TElement>(int minMatchLength, HorizontalMatchingStrategy<TElement> horizontalStrategy, VerticalMatchingStrategy<TElement> verticalStrategy)
    : CombinedFiguresMatchingStrategy<TElement>(minMatchLength, horizontalStrategy, verticalStrategy) where TElement : IEquatable<TElement>
{
    private const int IntersectionPointCount = 5;

    protected override int FigureIntersectionPointCount() => IntersectionPointCount;
}