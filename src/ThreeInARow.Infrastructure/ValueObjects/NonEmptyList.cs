using OneOf;
using OneOf.Types;

namespace ThreeInARow.Infrastructure.ValueObjects;

public class NonEmptyList<T> : List<T>
{
    private NonEmptyList(IEnumerable<T> items) : base(items) { }

    public static OneOf<NonEmptyList<T>, Error> From(IEnumerable<T> items)
    {
        var list = items.ToList();

        if (list.Count == 0)
            return new Error();

        return new NonEmptyList<T>(list);
    }

    public static OneOf<NonEmptyList<T>, Error> From(params T[] items) => From(items.AsEnumerable());

    public new PositiveInt Count => new(base.Count);
}

public static class NonEmptyListExtensions
{
    public static NonEmptyList<T> ToNonEmptyList<T>(this IEnumerable<T> items)
    {
        return NonEmptyList<T>.From(items).Match(
            nonEmpty => nonEmpty,
            _ => throw new ArgumentException("Collection cannot be empty", nameof(items))
        );
    }
}