using System.Diagnostics;
using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Infrastructure.Implementation;

public class SystemStopwatch : IStopwatch
{
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan? _endTime;

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

        _endTime = _stopwatch.Elapsed;
        _stopwatch.Stop();
        return new Success();
    }

    public void Reset()
    {
        _stopwatch.Reset();
        _endTime = null;
    }

    public bool IsRunning  => _stopwatch.IsRunning;

    public bool FinishedFullCycle => _endTime.HasValue && !_stopwatch.IsRunning;

    public OneOf<TimeSpan, HasNotStartedYet> Elapsed
    {
        get
        {
            if (_stopwatch.IsRunning)
                return _stopwatch.Elapsed;

            if (_endTime.HasValue)
                return _endTime.Value;

            return new HasNotStartedYet();
        }
    }
}