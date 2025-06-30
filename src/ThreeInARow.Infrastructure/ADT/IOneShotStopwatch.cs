using OneOf;
using OneOf.Types;

namespace ThreeInARow.Infrastructure.ADT;

public interface IOneShotStopwatch
{
    // Commands
    OneOf<Success, AlreadyRunning> Start();

    OneOf<Success, NeverStarted> Stop();

    // Queries
    bool IsRunning { get; }

    bool FinishedFullCycle { get; }

    OneOf<TimeSpan, NeverStarted> Elapsed { get; }
}

public struct AlreadyRunning;

public struct NeverStarted;