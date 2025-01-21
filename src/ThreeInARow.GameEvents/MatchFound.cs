using MediatR;

using ThreeInARow.Core;
using ThreeInARow.Matching;

namespace ThreeInARow.GameEvents;

public readonly struct MatchFound(Match foundMatch) : INotification
{
    public Match FoundMatch { get; } = foundMatch;
}