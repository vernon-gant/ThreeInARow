using OneOf;

namespace ThreeInARow.Board;

public class ElementCell<TElement>(OneOf<TElement, EmptyCell> element, GridCell cell) where TElement : IEquatable<TElement>, IVisual
{
    private readonly OneOf<TElement, EmptyCell> _element = element;

    public OneOf<string, EmptyCell> Visual => _element.IsT0 ? _element.AsT0.Visual : _element.AsT1;

    public bool IsOccupied() => _element.IsT0;

    public bool IsInColumn(int column) => cell.Column == column;

    public bool IsInRow(int row) => cell.Row == row;

    public bool MatchesWith(ElementCell<TElement> other) => IsOccupied() && other.IsOccupied() && _element.AsT0.Equals(other._element.AsT0);
}

public struct EmptyCell;