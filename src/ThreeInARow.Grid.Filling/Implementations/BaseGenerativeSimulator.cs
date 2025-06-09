using System.Diagnostics;
using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Filling.ADT;

namespace ThreeInARow.Grid.Filling.Implementations;

public abstract class BaseGenerativeSimulator<TElement, TGrid>(IGenerator<TElement> generator) : IGravitySimulator<TElement, TGrid> where TElement : IEquatable<TElement> where TGrid : IFillableGrid<TElement>, IReadableGrid<TElement>
{
    protected Dictionary<int, Queue<TElement>> _generatedElements = new();
    protected TGrid? _grid;

    public void Start(TGrid grid)
    {
        _grid = grid;
        generator.Generate(grid);

        var fillableColumns = grid.FillableColumns;

        foreach (var column in fillableColumns)
        {
            var columnElementsResult = generator.ForColumn(column);

            Debug.Assert(columnElementsResult.IsT0);

            _generatedElements[column] = columnElementsResult.AsT0;
        }
    }

    public abstract OneOf<Success, HasNotStartedYet, SimulationComplete> ExecuteNextStep();

    public void Reset()
    {
        _generatedElements.Clear();
        generator.Reset();
        _grid = default;
    }

    public OneOf<bool, HasNotStartedYet> HasMoreSteps
    {
        get
        {
            if (_generatedElements.Count == 0)
                return new HasNotStartedYet();

            return _generatedElements.Values.Any(queue => queue.Count > 0);
        }
    }
}