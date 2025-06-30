using System.Diagnostics;
using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Infrastructure.Implementation;

public class SystemStopwatch : IStopwatch
{
    private readonly Stopwatch _stopwatch = new();

    public OneOf<Success, IsRunning> Start()
    {
        throw new NotImplementedException();
    }

    public OneOf<Success, IsNotRunning> Stop()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public bool IsRunning  => _stopwatch.IsRunning;

    public OneOf<TimeSpan, IsNotRunning> Elapsed => _stopwatch.IsRunning ? _stopwatch.Elapsed : new IsNotRunning();
}