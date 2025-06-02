namespace ThreeInARow.Board;

/// <summary>
/// Represents an element in the grid with a visual representation.
/// </summary>
public record Element(string Visual) : IVisual;