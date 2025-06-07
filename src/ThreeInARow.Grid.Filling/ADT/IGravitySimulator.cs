using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.ADT;

namespace ThreeInARow.Grid.Filling.ADT;

public interface IGravitySimulator<TElement, TGrid> where TGrid : IFillableGrid<TElement>, IReadableGrid<TElement>
{
    // Commands
    void Start(TGrid grid);

    OneOf<Success, HasNotStartedYet, SimulationComplete> ExecuteNextStep();

    void Reset();

    // Queries
    OneOf<bool, HasNotStartedYet> HasMoreSteps { get; }
}

public struct HasNotStartedYet;

public struct SimulationComplete;