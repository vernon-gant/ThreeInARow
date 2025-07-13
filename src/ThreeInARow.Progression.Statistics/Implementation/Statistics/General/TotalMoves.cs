using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Progression.Events;
using ThreeInARow.Progression.Statistics.ADT;
using ThreeInARow.ValueObjects;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics.General;

public class TotalMoves : IStatistic, IEventHandler<MoveMade>
{
    private int _totalMoves;

    public NonEmptyString Name => "Total Moves".ToNonEmptyString();

    public Optional<NonEmptyString> Description => "Only counts moves that could be placed on the board".ToNonEmptyString();

    public Optional<NonEmptyString> Unit => new None();

    public OneOf<NonEmptyString, NotEnoughData> Value => _totalMoves.ToNonEmptyString();

    public void Handle(MoveMade notification) => _totalMoves++;
}