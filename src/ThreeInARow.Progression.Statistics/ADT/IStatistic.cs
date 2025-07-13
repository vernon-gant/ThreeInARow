using OneOf;
using ThreeInARow.ValueObjects;

namespace ThreeInARow.Progression.Statistics.ADT;

public interface IStatistic
{
    public NonEmptyString Name { get; }

    public Optional<NonEmptyString> Description { get; }

    public Optional<NonEmptyString> Unit { get; }

    public OneOf<NonEmptyString, NotEnoughData> Value { get; }
}

public struct NotEnoughData;