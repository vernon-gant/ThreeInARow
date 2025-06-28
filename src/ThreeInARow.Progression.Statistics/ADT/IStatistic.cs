using OneOf;
using OneOf.Types;

namespace ThreeInARow.Progression.Statistics.ADT;

public interface IStatistic
{
    public string Name { get; }

    public OneOf<string, None> Description { get; }

    public string Value { get; }

    public string Unit { get; }
}