using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.ADT;

/// <summary>
/// This ADT is responsible just for the grid structure manipulations without any game logic. To abstract away matching and other game logic but still be able
/// to analyze the state the enumerable with <see cref="ElementCell{TElement}"/> is used to iterate over the grid cells in a read-only manner. The <see cref="GridCell"/> is returned solely for the purpose of identifying the cell in the grid
/// and is only meant to be created by the grid itself.
/// </summary>
/// <typeparam name="TElement"></typeparam>
public interface IManageableGrid<TElement>
{
    // Commands

    /// <remarks>Postcondition: The two cells are swapped.</remarks>
    OneOf<Success, InvalidSwap> Swap(GridRow firstRow, GridColumn firstColumn, GridRow secondRow, GridColumn secondColumn);

    /// <remarks>Precondition: The cell must not be already deleted.</remarks>
    /// <remarks>Postcondition: The cell is deleted and can not be used anymore.</remarks>
    OneOf<Success, CellAlreadyDeleted> Delete(GridRow row, GridColumn column);
}

public struct InvalidSwap;
public struct CellAlreadyDeleted;
public struct ColumnIsFull;
public struct CellOutOfBounds;