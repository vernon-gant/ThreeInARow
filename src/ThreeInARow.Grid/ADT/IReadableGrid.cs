using OneOf;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.ADT;

public interface IReadableGrid<TElement> : IEnumerable<ElementCell<TElement>>
{
    /// <summary>
    /// Query to make sure that we always operate with valid cells when working with the grid or in the context of the grid. The <see cref="ElementCell{TElement}"/> is always a valid cell on the grid.
    /// </summary>
    OneOf<ElementCell<TElement>, CellOutOfBounds> TryGetCell(int row, int column);
}