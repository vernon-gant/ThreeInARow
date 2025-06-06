﻿using System.Diagnostics;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public abstract class CombinedFiguresMatchingStrategy<TElement>(int minMatchLength, HorizontalMatchingStrategy<TElement> horizontalStrategy, VerticalMatchingStrategy<TElement> verticalStrategy)
    : BaseMatchingStrategy<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    public override List<IMatch<TElement>> FindMatches(IEnumerable<ElementCell<TElement>> cells)
    {
        var cellsList = cells.ToList();
        var horizontalMatches = horizontalStrategy.FindMatches(cellsList);
        var verticalMatches = verticalStrategy.FindMatches(cellsList);

        var tMatches = FindTMatches(horizontalMatches, verticalMatches);

        return tMatches;
    }

    private List<IMatch<TElement>> FindTMatches(List<IMatch<TElement>> horizontalMatches, List<IMatch<TElement>> verticalMatches)
    {
        var tMatches = new List<IMatch<TElement>>();

        foreach (var horizontalMatch in horizontalMatches)
        {
            foreach (var verticalMatch in verticalMatches)
            {
                if (!horizontalMatch.Intersects(verticalMatch) || !IntersectionPointIsNotEnd(horizontalMatch, verticalMatch)) continue;

                var mergedMatchResult = horizontalMatch.Merge(verticalMatch);
                tMatches.Add(CreateMatch(mergedMatchResult.AsT0));
            }
        }

        return tMatches;
    }

    private bool IntersectionPointIsNotEnd(IMatch<TElement> horizontalMatch, IMatch<TElement> verticalMatch)
    {
        var orderedHorizontalCells = horizontalMatch.OrderBy(cell => cell.RowIndex).ThenBy(cell => cell.ColumnIndex).ToList();
        var orderedVerticalCells = verticalMatch.OrderBy(cell => cell.RowIndex).ThenBy(cell => cell.ColumnIndex).ToList();
        var intersectionCell = orderedHorizontalCells.First(cell => orderedVerticalCells.Any(vCell => vCell.Row == cell.Row && vCell.Column == cell.Column));
        var surroundingIntersection = new List<ElementCell<TElement>>
        {
            intersectionCell,
            intersectionCell with { Row = intersectionCell.Row - 1 },
            intersectionCell with { Row = intersectionCell.Row + 1 },
            intersectionCell with { Column = intersectionCell.Column - 1 },
            intersectionCell with { Column = intersectionCell.Column + 1 }
        };
        var mergedMatchResult = horizontalMatch.Merge(verticalMatch);

        Debug.Assert(mergedMatchResult.IsT0);

        var mergedMatch = mergedMatchResult.AsT0;

        mergedMatch.IntersectWith(surroundingIntersection);

        return mergedMatch.Count == FigureIntersectionPointCount();
    }

    protected abstract int FigureIntersectionPointCount();

    protected abstract IMatch<TElement> CreateMatch(IEnumerable<ElementCell<TElement>> cells);
}