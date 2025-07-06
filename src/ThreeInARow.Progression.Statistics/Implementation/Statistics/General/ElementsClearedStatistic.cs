using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.ValueObjects;
using ThreeInARow.Progression.Events.Events;
using ThreeInARow.Progression.Statistics.ADT;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics.General;

public class ElementsClearedStatistic<T> : IStatistic, IEventHandler<ElementsCleared<T>>
{
    private Optional<PositiveInt> _value = new None();

    public NonEmptyString Name => NonEmptyString.From("Elements Cleared").AsT0;

    public Optional<NonEmptyString> Description => NonEmptyString.From("Total number of elements cleared during the game").AsT0;

    public Optional<NonEmptyString> Unit => new None();

    public OneOf<NonEmptyString, NotEnoughData> Value => _value.Match<OneOf<NonEmptyString, NotEnoughData>>(value => value.ToNonEmptyString(), _ => new NotEnoughData());

    public void Handle(ElementsCleared<T> notification)
    {
        _value = _value.Match<Optional<PositiveInt>>(
            currentValue => currentValue + notification.Elements.Count,
            _ => notification.Elements.Count
        );
    }
}