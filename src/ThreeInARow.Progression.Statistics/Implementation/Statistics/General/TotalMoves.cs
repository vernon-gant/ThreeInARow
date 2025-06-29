using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Progression.Events.Events;
using ThreeInARow.Progression.Statistics.ADT;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics.General;

public class TotalMoves : IStatistic, IEventHandler<MoveMade>
{
    private int _totalMoves;

    public string Name => "Total Moves";

    public OneOf<string, None> Description => "Only counts moves that could be placed on the board";

    public OneOf<string, None> Unit => new None();

    public OneOf<string, NotEnoughData> Value => _totalMoves.ToString();

    public void Handle(MoveMade notification)
    {
        _totalMoves++;
    }
}