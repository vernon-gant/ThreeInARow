using OneOf;
using OneOf.Types;

namespace ThreeInARow.Progression.Statistics.ADT;

public interface IStatistic
{
    public string Name { get; }

    public OneOf<string, None> Description { get; }

    public OneOf<string, None> Unit { get; }

    public OneOf<string, NotEnoughData> Value { get; }
}

public struct NotEnoughData;