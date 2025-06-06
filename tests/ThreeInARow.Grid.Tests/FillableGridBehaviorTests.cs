using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Implementations;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Tests;

[TestFixture]
public abstract class FillableGridBehaviorTests<TGrid> where TGrid : IFillableGrid<Element>
{
    private TGrid _grid = default!;

    protected abstract TGrid CreateGrid(Element?[,] gridData);

    #region Helper Methods

    private Element?[,] EmptyGrid(int rows, int columns) => new Element?[rows, columns];

    #endregion

    #region AddTop Command Tests

    [Test]
    public void AddTop_GivenColumnWithSpaceAtTop_WhenAddingElement_ThenReturnsSuccess()
    {
        // Given a grid where column 1 has space at the top
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, new("B"), null, null }
        };
        _grid = CreateGrid(gridData);
        var element = new Element("A");
        var column = new GridColumn(1);

        // When adding element to top of column 1
        var result = _grid.AddTop(column, element);

        // Then operation succeeds
        Assert.That(result.IsT0, Is.True, "AddTop should return Success");
    }

    [Test]
    public void AddTop_GivenFullColumn_WhenAddingElement_ThenReturnsColumnIsFull()
    {
        // Given a grid where column 2 is completely full
        var gridData = new Element?[,]
        {
            { null, null, new("A"), null },
            { null, null, new("B"), null },
            { null, null, new("C"), null },
            { null, null, new("D"), null }
        };
        _grid = CreateGrid(gridData);
        var element = new Element("E");
        var column = new GridColumn(2);

        // When attempting to add another element to the full column
        var result = _grid.AddTop(column, element);

        // Then operation fails with ColumnIsFull
        Assert.That(result.IsT1, Is.True, "AddTop should return ColumnIsFull");
        Assert.That(result.AsT1, Is.TypeOf<ColumnIsFull>(), "Should return ColumnIsFull error");
    }

    [Test]
    public void AddTop_GivenColumnWithTopOccupied_WhenAddingElement_ThenReturnsColumnIsFull()
    {
        // Given a grid where only the top row of column 3 is occupied
        var gridData = new Element?[,]
        {
            { null, null, null, new("Existing") },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var element = new Element("New");
        var column = new GridColumn(3);

        // When attempting to add another element
        var result = _grid.AddTop(column, element);

        // Then operation fails with ColumnIsFull
        Assert.That(result.IsT1, Is.True, "AddTop should return ColumnIsFull when top is occupied");
        Assert.That(result.AsT1, Is.TypeOf<ColumnIsFull>(), "Should return ColumnIsFull error");
    }

    [Test]
    public void AddTop_GivenEmptyColumn_WhenAddingElement_ThenReturnsSuccess()
    {
        // Given a grid where column 0 is completely empty
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var element = new Element("First");
        var column = new GridColumn(0);

        // When adding element to empty column
        var result = _grid.AddTop(column, element);

        // Then operation succeeds
        Assert.That(result.IsT0, Is.True, "AddTop should return Success for empty column");

        // Verify element is placed if grid is also readable
        if (_grid is IReadableGrid<Element> readableGrid)
        {
            var topCell = readableGrid.TryGetCell(0, 0).AsT0;
            Assert.That(topCell.IsOccupied(), Is.True, "Top cell should be occupied after adding to empty column");
        }
    }

    #endregion

    #region ShiftDown Command Tests

    [Test]
    public void ShiftDown_GivenColumnWithGapInMiddle_WhenShifting_ThenReturnsSuccess()
    {
        // Given a grid where column 1 has elements with a gap in the middle
        var gridData = new Element?[,]
        {
            { null, new("Top"), null, null },
            { null, null, null, null },  // Gap at row 1
            { null, new("Bottom"), null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(1);

        // When shifting column 1 down
        var result = _grid.ShiftDown(column);

        // Then operation succeeds
        Assert.That(result.IsT0, Is.True, "ShiftDown should return Success");
    }

    [Test]
    public void ShiftDown_GivenFullColumnWithNoGaps_WhenShifting_ThenReturnsColumnIsFull()
    {
        // Given a grid where column 0 is completely full with no gaps
        var gridData = new Element?[,]
        {
            { new("A"), null, null, null },
            { new("B"), null, null, null },
            { new("C"), null, null, null },
            { new("D"), null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(0);

        // When attempting to shift down the full column
        var result = _grid.ShiftDown(column);

        // Then operation fails with ColumnIsFull
        Assert.That(result.IsT1, Is.True, "ShiftDown should return ColumnIsFull");
        Assert.That(result.AsT1, Is.TypeOf<ColumnIsFull>(), "Should return ColumnIsFull error");
    }

    [Test]
    public void ShiftDown_GivenEmptyColumn_WhenShifting_ThenReturnsSuccess()
    {
        // Given a grid where column 2 is completely empty
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var column = new GridColumn(2);

        // When attempting to shift down the empty column
        var result = _grid.ShiftDown(column);

        // Then operation succeeds (nothing to shift, but operation is valid)
        Assert.That(result.IsT0, Is.True, "ShiftDown should return Success for empty column");
    }

    [Test]
    public void ShiftDown_GivenColumnWithMultipleGaps_WhenShifting_ThenReturnsSuccess()
    {
        // Given a grid where column 3 has multiple gaps
        var gridData = new Element?[,]
        {
            { null, null, null, new("Top") },
            { null, null, null, null },        // Gap
            { null, null, null, new("Middle") },
            { null, null, null, null }         // Gap
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(3);

        // When shifting column down
        var result = _grid.ShiftDown(column);

        // Then operation succeeds
        Assert.That(result.IsT0, Is.True, "ShiftDown should return Success for column with multiple gaps");
    }

    [Test]
    public void ShiftDown_GivenColumnWithElementsAtBottom_WhenShifting_ThenReturnsSuccess()
    {
        // Given a grid where column has elements only at the bottom
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { new("X"), null, null, null },
            { new("Y"), null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(0);

        // When shifting column down
        var result = _grid.ShiftDown(column);

        // Then operation succeeds (elements are already at bottom)
        Assert.That(result.IsT0, Is.True, "ShiftDown should return Success when elements are already at bottom");
    }

    [Test]
    public void ShiftDown_GivenColumnWithSingleElementAtTop_WhenShifting_ThenReturnsSuccess()
    {
        // Given a grid where column has a single element at the top
        var gridData = new Element?[,]
        {
            { null, null, new("Lonely"), null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(2);

        // When shifting column down
        var result = _grid.ShiftDown(column);

        // Then operation succeeds
        Assert.That(result.IsT0, Is.True, "ShiftDown should return Success for single element");
    }

    #endregion

    #region IsColumnFull Query Tests

    [Test]
    public void IsColumnFull_GivenCompletelyFullColumn_WhenChecking_ThenReturnsTrue()
    {
        // Given a grid where column 1 is completely full
        var gridData = new Element?[,]
        {
            { null, new("A"), null, null },
            { null, new("B"), null, null },
            { null, new("C"), null, null },
            { null, new("D"), null, null }
        };
        _grid = CreateGrid(gridData);

        // When checking if column 1 is full
        var result = _grid.IsColumnFull(new GridColumn(1));

        // Then operation succeeds and returns true
        Assert.That(result, Is.True, "Column should be reported as full");
    }

    [Test]
    public void IsColumnFull_GivenPartiallyFullColumn_WhenChecking_ThenReturnsFalse()
    {
        // Given a grid where column 2 is partially full
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, new("A"), null },
            { null, null, null, null },
            { null, null, new("B"), null }
        };
        _grid = CreateGrid(gridData);

        // When checking if column 2 is full
        var result = _grid.IsColumnFull(new GridColumn(2));

        // Then operation succeeds and returns false
        Assert.That(result, Is.False, "Partially full column should be reported as not full");
    }

    [Test]
    public void IsColumnFull_GivenEmptyColumn_WhenChecking_ThenReturnsFalse()
    {
        // Given a grid where column 3 is completely empty
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);

        // When checking if column 3 is full
        var result = _grid.IsColumnFull(new GridColumn(3));

        // Then operation succeeds and returns false
        Assert.That(result, Is.False, "Empty column should be reported as not full");
    }

    #endregion
}