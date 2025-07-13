using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.ValueObjects;

namespace ThreeInARow.Progression.Events;

public record ElementsCleared<TElement> : IEvent
{
    public required NonEmptyList<TElement> Elements { get; init; }
}