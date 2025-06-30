using OneOf;
using OneOf.Types;

namespace ThreeInARow.Infrastructure.ADT;

public interface IStopwatch
{
    // Commands
    OneOf<Success, IsRunning> Start();

    OneOf<Success, IsNotRunning> Stop();

    void Reset();

    // Queries
    bool IsRunning { get; }

    OneOf<TimeSpan, IsNotRunning> Elapsed { get; }
}

public struct IsRunning;

public struct IsNotRunning;