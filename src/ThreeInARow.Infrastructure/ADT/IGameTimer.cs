using OneOf;
using OneOf.Types;

namespace ThreeInARow.Infrastructure.ADT;

public interface IGameTimer
{
    OneOf<Success, GameRunning> StartGame();

    OneOf<Success, GameNotRunning> StopGame();

    bool IsGameRunning { get; }

    bool HasGameEnded  { get; }

    OneOf<TimeSpan, GameNotRunning> ElapsedGameTime { get; }
}

public struct GameRunning;

public struct GameNotRunning;