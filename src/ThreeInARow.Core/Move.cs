namespace ThreeInARow.Core;

public readonly struct Move(Position from, Position to)
{
    public Position From => from;
    public Position To => to;

    public bool IsForAdjacent => from.IsAdjacentTo(to);

    public bool IsWithinBounds(int width, int height) => from.IsWithinBounds(width, height) && to.IsWithinBounds(width, height);
}

public readonly record struct Position(int X, int Y)
{
    public bool IsWithinBounds(int width, int height) => X >= 0 && X < width && Y >= 0 && Y < height;

    public bool IsAdjacentTo(Position other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y) == 1;
}