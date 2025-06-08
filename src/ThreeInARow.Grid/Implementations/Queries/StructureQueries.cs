using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Implementations.Queries;

public static class StructureQueries
{
    public static IEnumerable<IGrouping<int, ElementCell<TElement>>> GroupByRow<TElement>(this IEnumerable<ElementCell<TElement>> cells, bool onlyOccupied = true) =>
        cells.Where(cell => !onlyOccupied || cell.IsOccupied()).GroupBy(cell => cell.RowIndex);

    public static IEnumerable<IGrouping<int, ElementCell<TElement>>> GroupByColumn<TElement>(this IEnumerable<ElementCell<TElement>> cells, bool onlyOccupied = true) =>
        cells.Where(cell => !onlyOccupied || cell.IsOccupied()).GroupBy(cell => cell.ColumnIndex);

    public static Dictionary<int, List<ElementCell<TElement>>> ToRowDictionary<TElement>(this IEnumerable<ElementCell<TElement>> cells, bool onlyOccupied = true) =>
        cells.GroupByRow(onlyOccupied).ToDictionary(group => group.Key, group => group.OrderBy(cell => cell.ColumnIndex).ToList());

    public static Dictionary<int, List<ElementCell<TElement>>> ToColumnDictionary<TElement>(this IEnumerable<ElementCell<TElement>> cells, bool onlyOccupied = true) =>
        cells.GroupByColumn(onlyOccupied).ToDictionary(group => group.Key, group => group.OrderBy(cell => cell.RowIndex).ToList());

    public static IEnumerable<IEnumerable<ElementCell<TElement>>> ToNLengthSequences<TElement>(this IEnumerable<ElementCell<TElement>> cells, int sequenceLength)
    {
        var cellList = cells.ToList();
        return Enumerable.Range(0, cellList.Count - sequenceLength + 1).Select(startIndex => cellList.Skip(startIndex).Take(sequenceLength));
    }
}