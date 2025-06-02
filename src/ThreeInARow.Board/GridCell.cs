namespace ThreeInARow.Board;

/// <summary>
/// This ADT represents a cell in a grid, defined by its row and column indices which is only meant to be created by the grid itself because it is used to identify the valid cell in the grid.
/// </summary>
public record GridCell(int Row, int Column) : Position(Row, Column);