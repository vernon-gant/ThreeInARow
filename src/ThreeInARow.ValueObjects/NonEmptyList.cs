namespace ThreeInARow.ValueObjects;

public class NonEmptyList<T>
{
    public NonEmptyList(IEnumerable<T> collection)
    {
        var list = collection.ToList();

        if (list.Count == 0)
            throw new ArgumentException("Collection cannot be empty", nameof(collection));

        Items = list.AsReadOnly();
    }

    public NonEmptyList(params T[] items) : this(items.AsEnumerable()) { }

    public IReadOnlyList<T> Items { get; }

    public PositiveInt Count => new(Items.Count);
}

public static class NonEmptyListExtensions
{
    public static NonEmptyList<T> ToNonEmptyList<T>(this IEnumerable<T> collection) where T : notnull => new(collection);

    public static NonEmptyList<T> ToNonEmptyList<T>(params T[] items) where T : notnull => new(items);
}