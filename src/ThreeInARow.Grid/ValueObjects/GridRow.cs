namespace ThreeInARow.Grid.ValueObjects;

/// <summary>
/// This ADT represents a row in a grid, defined by its index. It is not meant to be created by the user,
/// but rather used internally by the grid in different queries to make sure that the column is valid when working with the grid.
/// </summary>
public record GridRow(int Index)
{
    public static implicit operator int(GridRow row) => row.Index;

    public static implicit operator GridRow(int index) => new(index);
}