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
    OneOf<Success, ColumnIndexOutOfBounds, CanNotShiftDown> ShiftDown(int columnIndex);

    /// <remarks>Precondition: The column is not full.</remarks>
    /// <remarks>Postcondition: The element is added to the top of the column.</remarks>
    OneOf<Success, ColumnIndexOutOfBounds, CanNotAddTop> AddTop(int columnIndex, TElement element);


    // Queries

    OneOf<bool, ColumnIndexOutOfBounds> IsColumnFull(int columnIndex);

    OneOf<bool, ColumnIndexOutOfBounds> CanShiftDown(int columnIndex);

    List<int> FillableColumns { get; }
}

public struct CanNotShiftDown;

public struct CanNotAddTop;

public struct ColumnIndexOutOfBounds;