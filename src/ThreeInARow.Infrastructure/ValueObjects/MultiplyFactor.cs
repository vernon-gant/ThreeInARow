namespace ThreeInARow.Infrastructure.ValueObjects;

public readonly record struct MultiplyFactor
{
    public MultiplyFactor() => Value = 1;

    public MultiplyFactor(int value)
    {
        if (Value < 1)
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than or equal to 1.");

        Value = value;
    }

    public int Value { get; }
}