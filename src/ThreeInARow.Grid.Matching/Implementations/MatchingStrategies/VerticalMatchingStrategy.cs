using OneOf;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Implementations.Queries;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;
using ThreeInARow.Grid.ValueObjects.Extensions;

namespace ThreeInARow.Grid.Matching.Implementations.MatchingStrategies;

public class VerticalMatchingStrategy<TElement>(int minMatchLength) : VerticalHorizontalMatchingStrategyBase<TElement>(minMatchLength) where TElement : IEquatable<TElement>
{
    protected override (OneOf<Cell<TElement>, CellOutOfBounds> First, OneOf<Cell<TElement>, CellOutOfBounds> Second) GetNeighborCells(Cell<TElement> cell, IReadableGrid<TElement> grid)
    {
        var leftCell = cell.Left(grid);
        var rightCell = cell.Right(grid);
        return (leftCell, rightCell);
    }


    protected override Func<Cell<TElement>, int> GroupingKey() => cell => cell.ColumnIndex;

    protected override Func<Cell<TElement>, int> OrderByKey() => cell => cell.RowIndex;

    protected override bool CanExtendGroup(List<Cell<TElement>> group, Cell<TElement> cell)
    {
        var lastCell = group.Last();
        return lastCell.Content.Equals(cell.Content) && lastCell.RowIndex + 1 == cell.RowIndex;
    }

    protected override IMatch<TElement> CreateMatch(DistinctCells<TElement> cells) => VerticalMatch<TElement>.Create(cells).AsT0;
}