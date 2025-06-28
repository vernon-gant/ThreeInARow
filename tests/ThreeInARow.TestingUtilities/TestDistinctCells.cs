using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.TestingUtilities;

public class TestDistinctCells<T> : DistinctCells<T> where T : IEquatable<T>
{
    private TestDistinctCells(HashSet<Cell<T>> cells) : base(cells) { }

    public static DistinctCells<T> Create(List<Cell<T>> cells)
    {
        var result = DistinctCells<T>.Create(cells);
        if (result.IsT0)
            return result.AsT0;

        throw new ArgumentException($"Cannot create DistinctCells: {result}");
    }
}