namespace ThreeInARow.Core;

public readonly record struct MultiplyFactor
{
    public int Value { get; init; }

    public MultiplyFactor() => Value = 1;

    public MultiplyFactor(int value)
    {
        if (Value < 1)
            throw new ArgumentOutOfRangeException(nameof(Value), "Value must be greater than or equal to 1.");

        Value = value;
    }
}