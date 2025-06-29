using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Progression.Events.Events;

public record ElementsCleared<TElement> : IEvent
{
    public required IReadOnlyList<TElement> Elements { get; init; }
}