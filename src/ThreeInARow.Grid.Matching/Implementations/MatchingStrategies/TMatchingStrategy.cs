﻿using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public class TMatchingStrategy<TElement>(int minMatchLength, HorizontalMatchingStrategy<TElement> horizontalStrategy, VerticalMatchingStrategy<TElement> verticalStrategy)
    : CombinedFiguresMatchingStrategy<TElement>(minMatchLength, horizontalStrategy, verticalStrategy) where TElement : IEquatable<TElement>
{
    private const int IntersectionPointCount = 4;

    protected override int FigureIntersectionPointCount() => IntersectionPointCount;

    protected override IMatch<TElement> CreateMatch(DistinctCells<TElement> cells) => TMatch<TElement>.Create(cells).AsT0;
}