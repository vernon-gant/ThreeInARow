using OneOf;
using OneOf.Types;

namespace ThreeInARow.Infrastructure.ValueObjects;

public readonly record struct NonEmptyString
{
    public string Value { get; init; }

    private NonEmptyString(string value) => Value = value;

    public static OneOf<NonEmptyString, Error> From(string value) => string.IsNullOrWhiteSpace(value) ? new Error() : new NonEmptyString(value);

    public static implicit operator string(NonEmptyString nonEmptyString) => nonEmptyString.Value;

    public override string ToString() => Value;
}

public static class NonEmptyStringExtensions
{
    public static NonEmptyString ToNonEmptyString<T>(this T value) where T : notnull
    {
        return NonEmptyString.From(value.ToString() ?? string.Empty).Match(
            nonEmpty => nonEmpty,
            _ => throw new ArgumentException("Value cannot be null or empty", nameof(value))
        );
    }
}