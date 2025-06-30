using OneOf;
using OneOf.Types;

namespace ThreeInARow.Infrastructure.ADT;

public interface IStopwatch
{
    // Commands
    OneOf<Success, AlreadyRunning> Start();

    OneOf<Success, HasNotStartedYet> Stop();

    void Reset();

    // Queries
    bool IsRunning { get; }

    bool FinishedFullCycle { get; }

OneOf<TimeSpan, HasNotStartedYet> Elapsed { get; }
}

public struct AlreadyRunning;

public struct HasNotStartedYet;