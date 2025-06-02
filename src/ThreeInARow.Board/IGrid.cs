using OneOf;
using OneOf.Types;

namespace ThreeInARow.Board;

/// <summary>
/// This ADT is responsible just for the grid structure manipulations without any game logic. To abstract away matching and other game logic but still be able
/// to analyze the state the enumerable with <see cref="ElementCell{TElement}"/> is used to iterate over the grid cells in a read-only manner. The <see cref="GridCell"/> is returned solely for the purpose of identifying the cell in the grid
/// and is only meant to be created by the grid itself.
/// </summary>
/// <typeparam name="TElement"></typeparam>
public interface IGrid<TElement> : IEnumerable<ElementCell<TElement>> where TElement : IEquatable<TElement>, IVisual
{

    #region Commands

    /// <remarks>Postcondition: The two cells are swapped.</remarks>
    OneOf<Success, InvalidSwap> Swap(GridCell first, GridCell second);

    /// <remarks>Precondition: The cell must not be already deleted.</remarks>
    /// <remarks>Postcondition: The cell is deleted and can not be used anymore.</remarks>
    OneOf<Success, CellAlreadyDeleted> Delete(GridCell cell);

    /// <remarks>Precondition: The column is not full.</remarks>
    /// <remarks>Postcondition: The elements in the column are shifted down by one row, and the topmost cell is empty.</remarks>
    OneOf<Success, ColumnOutOfBounds, ColumnIsFull> ShiftDown(int column);

    /// <remarks>Precondition: The column is not full.</remarks>
    /// <remarks>Postcondition: The element is added to the top of the column.</remarks>
    OneOf<Success, ColumnOutOfBounds, ColumnIsFull> AddTop(int column, TElement element);

    #endregion


    #region Queries

    /// <summary>
    /// Query to make sure that we always operate with valid cells when working with the grid or in the context of the grid. The <see cref="GridCell"/> is always a valid cell on the grid.
    /// </summary>
    OneOf<GridCell, CellOutOfBounds> TryGetCell(int row, int column);

    OneOf<bool, ColumnOutOfBounds> IsColumnFull(int column);

    #endregion
}

public struct InvalidSwap;
public struct ColumnOutOfBounds;
public struct CellAlreadyDeleted;
public struct ColumnIsFull;
public struct CellOutOfBounds;