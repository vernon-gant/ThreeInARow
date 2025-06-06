namespace ThreeInARow.Grid.ValueObjects;

/// <summary>
/// This ADT represents a column in a grid, defined by its index. It is not meant to be created by the user,
/// but rather used internally by the grid in different queries to make sure that the column is valid when working with the grid.
/// </summary>
public record GridColumn(int Index)
{
    public static implicit operator int(GridColumn column) => column.Index;

    public static implicit operator GridColumn(int index) => new(index);
}