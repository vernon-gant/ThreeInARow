using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Implementations.Queries;

public static class PresenceQueries
{
    public static bool IsEmpty<TElement>(this IEnumerable<ElementCell<TElement>> cells) => cells.All(cell => !cell.IsOccupied());
}