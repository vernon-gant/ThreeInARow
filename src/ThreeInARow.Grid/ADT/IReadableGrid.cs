using OneOf;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.ADT;

public interface IReadableGrid<TElement> : IEnumerable<Cell<TElement>>
{
    int RowCount { get; }

    int ColumnCount { get; }

    OneOf<CellContent<TElement>, CellOutOfBounds> ContentAt(int row, int column);
}