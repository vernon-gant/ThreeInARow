using System.Diagnostics;
using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Infrastructure.Implementation;

public class SystemOneShotStopwatch : IOneShotStopwatch
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

    public OneOf<Success, NeverStarted> Stop()
    {
        if (!_stopwatch.IsRunning)
            return new NeverStarted();

        _endTime = _stopwatch.Elapsed;
        _stopwatch.Stop();
        return new Success();
    }

    public bool IsRunning  => _stopwatch.IsRunning;

    public OneOf<TimeSpan, NeverStarted> Elapsed
    {
        get
        {
            if (_stopwatch.IsRunning)
                return _stopwatch.Elapsed;

            if (_endTime.HasValue)
                return _endTime.Value;

            return new NeverStarted();
        }
    }
}