using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Infrastructure.Implementation;

public class OneShotStopwatchGameTimer(IOneShotStopwatch stopwatch) : IGameTimer
{
    public OneOf<Success, GameRunning> StartGame()
    {
        return stopwatch.Start().Match<OneOf<Success, GameRunning>>(
            success => success,
            _ => new GameRunning()
        );
    }

    public OneOf<Success, GameNotRunning> StopGame()
    {
        return stopwatch.Stop().Match<OneOf<Success, GameNotRunning>>(
            success => success,
            _ => new GameNotRunning()
        );
    }

    public bool IsGameRunning => stopwatch.IsRunning;

    public bool HasGameEnded => stopwatch.FinishedFullCycle;

    public OneOf<TimeSpan, GameNotRunning> ElapsedGameTime => stopwatch.Elapsed.Match<OneOf<TimeSpan, GameNotRunning>>(
        elapsed => elapsed,
        _ => new GameNotRunning()
    );
}