using System.Diagnostics;
using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Infrastructure.Implementation;

public class SystemStopwatch : IStopwatch
{
    private readonly Stopwatch _stopwatch = new();

    public OneOf<Success, AlreadyRunning> Start()
    {
        if (_stopwatch.IsRunning)
            return new AlreadyRunning();

        _stopwatch.Start();
        return new Success();
    }

    public OneOf<Success, HasNotStartedYet> Stop()
    {
        if (!_stopwatch.IsRunning)
            return new HasNotStartedYet();

        _stopwatch.Stop();
        return new Success();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public bool IsRunning  => _stopwatch.IsRunning;

    public OneOf<TimeSpan, HasNotStartedYet> Elapsed => _stopwatch.IsRunning ? _stopwatch.Elapsed : new HasNotStartedYet();
}