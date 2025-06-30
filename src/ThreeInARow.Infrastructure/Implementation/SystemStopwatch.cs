using System.Diagnostics;
using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Infrastructure.Implementation;

public class SystemStopwatchTimer : IGameTimer
{
    private readonly Stopwatch _stopwatch = new();
    private TimeSpan? _endTime;

    public OneOf<Success, NotPossible> StartGame()
    {
        if (_stopwatch.IsRunning)
            return new NotPossible();

        _stopwatch.Reset();
        _endTime = null;

        _stopwatch.Start();
        return new Success();
    }

    public OneOf<Success, NotPossible> StopGame()
    {
        if (!_stopwatch.IsRunning)
            return new NotPossible();

        _stopwatch.Stop();
        _endTime = _stopwatch.Elapsed;
        return new Success();
    }

    public OneOf<TimeSpan, NotPossible> ElapsedGameTime
    {
        get
        {
            if (_stopwatch.IsRunning)
                return _stopwatch.Elapsed;
            if (_endTime.HasValue)
                return _endTime.Value;
            return new NotPossible();
        }
    }
}