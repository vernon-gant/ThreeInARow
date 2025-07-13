using OneOf;
using OneOf.Types;

namespace ThreeInARow.ValueObjects;

public readonly record struct PositiveInt
{
    public PositiveInt(int value)
    {
        if (Value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than or equal to 0.");

        Value = value;
    }

    public int Value { get; }

    public PositiveInt Multiply(MultiplyFactor factor) => new (Value * factor.Value);

    public OneOf<PositiveInt, Error> Subtract(PositiveInt other)
    {
        if (Value < other.Value)
            return new Error();

        return new PositiveInt(Value - other.Value);
    }

    public static PositiveInt operator +(PositiveInt left, PositiveInt right) => new (left.Value + right.Value);

    public override string ToString() => Value.ToString();
}