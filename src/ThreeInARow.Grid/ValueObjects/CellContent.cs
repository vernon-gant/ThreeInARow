using OneOf;

namespace ThreeInARow.Grid.ValueObjects;

public class CellContent<TElement> : OneOfBase<TElement, EmptyCell>
{
    protected CellContent(OneOf<TElement, EmptyCell> input) : base(input) { }

    public static implicit operator CellContent<TElement>(TElement value) => new(value);

    public static implicit operator CellContent<TElement>(EmptyCell value) => new(value);

    public bool IsEmpty => IsT1;

    public bool IsOccupied => IsT0;
}

public struct EmptyCell;