namespace ThreeInARow.ValueObjects;

public readonly record struct NonEmptyString
{
    public NonEmptyString(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or empty", nameof(value));

        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(NonEmptyString nonEmptyString) => nonEmptyString.Value;

    public static implicit operator NonEmptyString(string value) => new(value);

    public override string ToString() => Value;
}

public static class NonEmptyStringExtensions
{
    public static NonEmptyString ToNonEmptyString<T>(this T value) where T : notnull => new(value.ToString());
}