using OneOf;
using System.Diagnostics;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;
using ThreeInARow.Grid.ValueObjects.Extensions;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public abstract class CombinedFiguresMatchingStrategy<TElement>(int minMatchLength, HorizontalMatchingStrategy<TElement> horizontalStrategy, VerticalMatchingStrategy<TElement> verticalStrategy)
    : BaseMatchingStrategy<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    public override List<IMatch<TElement>> FindMatches(IReadableGrid<TElement> grid)
    {
        var horizontalMatches = horizontalStrategy.FindMatches(grid);
        var verticalMatches = verticalStrategy.FindMatches(grid);

        var tMatches = FindTMatches(horizontalMatches, verticalMatches, grid);

        return tMatches;
    }

    public override OneOf<bool, GridHasEmptyCells, GridHasMatches> HasPotentialMatches(IReadableGrid<TElement> grid) => false;

    private List<IMatch<TElement>> FindTMatches(List<IMatch<TElement>> horizontalMatches, List<IMatch<TElement>> verticalMatches, IReadableGrid<TElement> grid)
    {
        var tMatches = new List<IMatch<TElement>>();

        foreach (var horizontalMatch in horizontalMatches)
        {
            foreach (var verticalMatch in verticalMatches)
            {
                if (!horizontalMatch.Intersects(verticalMatch) || !IntersectionPointIsNotEnd(horizontalMatch, verticalMatch, grid))
                    continue;

                var mergedMatchResult = horizontalMatch.Merge(verticalMatch);
                tMatches.Add(CreateMatch(mergedMatchResult.AsT0));
            }
        }

        return tMatches;
    }

    private bool IntersectionPointIsNotEnd(IMatch<TElement> horizontalMatch, IMatch<TElement> verticalMatch, IReadableGrid<TElement> grid)
    {
        var orderedHorizontalCells = horizontalMatch.OrderBy(cell => cell.RowIndex).ThenBy(cell => cell.ColumnIndex).ToList();
        var orderedVerticalCells = verticalMatch.OrderBy(cell => cell.RowIndex).ThenBy(cell => cell.ColumnIndex).ToList();
        var intersectionCell = orderedHorizontalCells.First(cell => orderedVerticalCells.Any(vCell => vCell.HasSameCoordinatesAs(cell)));
        var surroundingIntersection = new List<OneOf<Cell<TElement>, CellOutOfBounds>>
        {
            intersectionCell,
            intersectionCell.Top(grid),
            intersectionCell.Bottom(grid),
            intersectionCell.Left(grid),
            intersectionCell.Right(grid)
        }.Where(cell => cell.IsT0).Select(cell => cell.AsT0).ToList();
        var mergedMatchResult = horizontalMatch.Merge(verticalMatch);

        Debug.Assert(mergedMatchResult.IsT0);

        var mergedMatch = mergedMatchResult.AsT0;

        mergedMatch.IntersectWith(surroundingIntersection);

        return mergedMatch.Count == FigureIntersectionPointCount();
    }

    protected abstract int FigureIntersectionPointCount();
}