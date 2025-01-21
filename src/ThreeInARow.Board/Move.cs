namespace ThreeInARow.Core;

public readonly struct Move(Position from, Position to)
{
    public Position From => from;
    public Position To => to;

    public bool IsForAdjacent => from.IsAdjacentTo(to);

    public bool IsWithinBounds(int dimension) => from.IsWithinBounds(dimension) && to.IsWithinBounds(dimension);
}

public readonly record struct Position(int Row, int Column)
{
    public bool IsWithinBounds(int dimension) => Row >= 0 && Row < dimension && Column >= 0 && Column < dimension;

    public bool IsAdjacentTo(Position other) => Math.Abs(Row - other.Row) + Math.Abs(Column - other.Column) == 1;
}