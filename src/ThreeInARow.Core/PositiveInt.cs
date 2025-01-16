namespace ThreeInARow.Core;

public readonly record struct PositiveInt
{
    public int Value { get; init; }

    public PositiveInt() => Value = 0;

    public PositiveInt(int value)
    {
        if (Value < 0)
            throw new ArgumentOutOfRangeException(nameof(Value), "Value must be greater than or equal to 0.");

        Value = value;
    }

    public PositiveInt Multiply(MultiplyFactor factor) => new (Value * factor.Value);

    public static PositiveInt operator +(PositiveInt left, PositiveInt right) => new (left.Value + right.Value);
}