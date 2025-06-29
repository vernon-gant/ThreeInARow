using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Progression.Events.Events;
using ThreeInARow.Progression.Statistics.ADT;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics.General;

public class TotalMoves : IStatistic, IEventHandler<MoveMade>
{
    public string Name { get; }

    public OneOf<string, None> Description { get; }

    public OneOf<string, None> Unit { get; }

    public OneOf<string, NotEnoughData> Value { get; }

    public void Handle(MoveMade notification)
    {
        throw new NotImplementedException();
    }
}