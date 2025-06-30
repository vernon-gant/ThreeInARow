using OneOf;
using OneOf.Types;

namespace ThreeInARow.Infrastructure.ADT;

public interface IGameTimer
{
    OneOf<Success, NotPossible> StartGame();

    OneOf<Success, NotPossible> StopGame();

    OneOf<TimeSpan, NotPossible> ElapsedGameTime { get; }
}

public struct NotPossible;