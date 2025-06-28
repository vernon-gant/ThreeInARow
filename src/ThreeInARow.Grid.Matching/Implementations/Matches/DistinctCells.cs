using System.Collections;
using OneOf;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Matching.Implementations.Matches;

public class DistinctCells<TElement> : IEnumerable<Cell<TElement>>
{
    private readonly HashSet<Cell<TElement>> _cells;

    protected DistinctCells(HashSet<Cell<TElement>> cells)
    {
        _cells = cells;
    }

    public static OneOf<DistinctCells<TElement>, EmptyList, SameCellsPresent> Create(IEnumerable<Cell<TElement>> cells)
    {
        var cellsList = cells.ToList();

        if (cellsList.Count == 0)
            return new EmptyList();

        var distinctCells = cellsList.ToHashSet();

        if (cellsList.Count != distinctCells.Count)
            return new SameCellsPresent();

        return new DistinctCells<TElement>(distinctCells);
    }

    public int Count => _cells.Count;

    public bool Intersects(DistinctCells<TElement> other) => _cells.Overlaps(other._cells);

    public OneOf<DistinctCells<TElement>, DoesNotIntersect> Merge(DistinctCells<TElement> other)
    {
        if (!Intersects(other))
            return new DoesNotIntersect();

        var onlyUniqueCells = new HashSet<Cell<TElement>>(_cells);

        onlyUniqueCells.UnionWith(other._cells);

        return new DistinctCells<TElement>(onlyUniqueCells);
    }

    public void IntersectWith(DistinctCells<TElement> other) => _cells.IntersectWith(other._cells);

    public IEnumerator<Cell<TElement>> GetEnumerator() => _cells.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public struct SameCellsPresent;

public struct EmptyList;