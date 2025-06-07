using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.ADT;

/// <summary>
///
/// </summary>
/// <typeparam name="TElement"></typeparam>
public interface IFillableGrid<TElement>
{
    // Commands

    /// <remarks>Precondition: The column is not full.</remarks>
    /// <remarks>Postcondition: The elements in the column are shifted down by one row, and the topmost cell is empty.</remarks>
    OneOf<Success, ColumnIsFull, CanNotShiftDown> ShiftDown(GridColumn column);

    /// <remarks>Precondition: The column is not full.</remarks>
    /// <remarks>Postcondition: The element is added to the top of the column.</remarks>
    OneOf<Success, ColumnIsFull> AddTop(GridColumn column, TElement element);


    // Queries

    bool IsColumnFull(GridColumn column);

    OneOf<bool, ColumnIsFull> CanShiftDown(GridColumn column);

    List<GridColumn> FillableColumns { get; }
}

public struct CanNotShiftDown;