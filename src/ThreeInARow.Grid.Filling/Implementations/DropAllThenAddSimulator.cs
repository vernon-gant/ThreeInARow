using System.Diagnostics;
using OneOf;
using OneOf.Types;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Filling.ADT;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Filling.Implementations;

public class DropAllThenAddSimulator<TElement, TGrid>(IGenerator<TElement> generator) : BaseGenerativeSimulator<TElement, TGrid>(generator) where TElement : IEquatable<TElement> where TGrid : IFillableGrid<TElement>, IReadableGrid<TElement>
{
    public override OneOf<Success, HasNotStartedYet, SimulationComplete> ExecuteNextStep()
    {
        if (_grid == null)
            return new HasNotStartedYet();

        if (!HasMoreSteps.AsT0)
            return new SimulationComplete();

        foreach (var column in _grid.FillableColumns)
        {
            FullDropColumn(column);

            Debug.Assert(_generatedElements.ContainsKey(column), "Column should be present in the generated elements dictionary.");

            var elementsQueue = _generatedElements[column];

            Debug.Assert(elementsQueue.Count > 0, "There should be elements to drop and add in the column.");

            _grid.AddTop(column, elementsQueue.Dequeue());
        }

        return new Success();
    }

    private void FullDropColumn(GridColumn column)
    {
        Debug.Assert(_grid!.CanDrop(column).AsT0, "Column should be droppable.");

        while (_grid.CanDrop(column).AsT0)
        {
            _grid.Drop(column);
        }
    }
}