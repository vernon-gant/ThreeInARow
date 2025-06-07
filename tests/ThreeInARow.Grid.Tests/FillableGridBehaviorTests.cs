using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.ValueObjects;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Grid.Tests;

[TestFixture]
public abstract class FillableGridBehaviorTests<TGrid> : GridTestsBase<IFillableGrid<string>>, MGridTestUtility
{
    #region Adding Elements to Columns

    [Test]
    public void GivenAColumnWithAvailableSpace_WhenAddingAnElementToTheTop_ThenTheElementIsSuccessfullyAdded()
    {
        // Given a grid where column 1 has available space
        var gridData = new[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, "B", null, null }
        };
        _grid = CreateGrid(gridData);
        var element = "A";
        var column = new GridColumn(1);

        // When adding an element to the top of the column
        var result = _grid.AddTop(column, element);

        // Then the element is successfully added
        Assert.That(result.IsT0, Is.True, "Element should be successfully added to available space");
    }

    [Test]
    public void GivenACompletelyFullColumn_WhenAttemptingToAddAnElement_ThenTheOperationIsRejected()
    {
        // Given a grid where column 2 is completely full
        var gridData = new[,]
        {
            { null, null, "A", null },
            { null, null, "B", null },
            { null, null, "C", null },
            { null, null, "D", null }
        };
        _grid = CreateGrid(gridData);
        var element = "E";
        var column = new GridColumn(2);

        // When attempting to add another element
        var result = _grid.AddTop(column, element);

        // Then the operation is rejected because the column is full
        Assert.That(result.IsT1, Is.True, "Operation should be rejected when column is full");
        Assert.That(result.AsT1, Is.TypeOf<ColumnIsFull>(), "Should return ColumnIsFull error");
    }

    [Test]
    public void GivenAColumnWithOnlyTheTopSpaceOccupied_WhenAttemptingToAddAnElement_ThenTheOperationIsRejected()
    {
        // Given a grid where only the top position of column 3 is occupied
        var gridData = new[,]
        {
            { null, null, null, "." },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var element = "New";
        var column = new GridColumn(3);

        // When attempting to add another element to the top
        var result = _grid.AddTop(column, element);

        // Then the operation is rejected because there's no space at the top
        Assert.That(result.IsT1, Is.True, "Operation should be rejected when top position is occupied");
    }

    [Test]
    public void GivenAnEmptyColumn_WhenAddingTheFirstElement_ThenTheElementIsSuccessfullyAdded()
    {
        // Given a grid where column 0 is completely empty
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var element = "First";
        var column = new GridColumn(0);

        // When adding the first element to the empty column
        var result = _grid.AddTop(column, element);

        // Then the element is successfully added
        Assert.That(result.IsT0, Is.True, "First element should be successfully added to empty column");
    }

    #endregion

    #region Shifting Elements Down

    [Test]
    public void GivenAColumnWithGapsInTheMiddle_WhenShiftingElementsDown_ThenElementsDropToFillGaps()
    {
        // Given a grid where column 1 has elements with gaps in the middle
        var gridData = new[,]
        {
            { null, ".", null, null },
            { null, null, null, null },
            { null, ".", null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(1);

        // When shifting elements down in the column
        var result = _grid.ShiftDown(column);

        // Then elements drop to fill the gaps
        Assert.That(result.IsT0, Is.True, "Elements should successfully drop to fill gaps");
    }

    [Test]
    public void GivenAFullColumnWithNoGaps_WhenAttemptingToShiftDown_ThenNoShiftingOccurs()
    {
        // Given a grid where column 0 is completely full with no gaps
        var gridData = new[,]
        {
            { ".", null, null, null },
            { ".", null, null, null },
            { ".", null, null, null },
            { ".", null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(0);

        // When attempting to shift elements down
        var result = _grid.ShiftDown(column);

        // Then no shifting occurs because the column is already full
        Assert.That(result.IsT1, Is.True, "No shifting should occur in a full column");
    }

    [Test]
    public void GivenAnEmptyColumn_WhenAttemptingToShiftDown_ThenNoShiftingIsPossible()
    {
        // Given a grid where column 2 is completely empty
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var column = new GridColumn(2);

        // When attempting to shift elements down
        var result = _grid.ShiftDown(column);

        // Then no shifting is possible because there are no elements
        Assert.That(result.IsT2, Is.True, "No shifting should be possible in an empty column");
    }

    [Test]
    public void GivenAColumnWithMultipleGaps_WhenShiftingElementsDown_ThenAllElementsDropAppropriately()
    {
        // Given a grid where column 3 has multiple gaps between elements
        var gridData = new[,]
        {
            { null, null, null, "." },
            { null, null, null, null },
            { null, null, null, "." },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(3);

        // When shifting elements down
        var result = _grid.ShiftDown(column);

        // Then all elements drop to fill available spaces
        Assert.That(result.IsT0, Is.True, "All elements should drop to fill available spaces");
    }

    [Test]
    public void GivenAColumnWithElementsAlreadyAtBottom_WhenAttemptingToShiftDown_ThenNoShiftingIsNeeded()
    {
        // Given a grid where column has elements only at the bottom positions
        var gridData = new[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { ".", null, null, null },
            { ".", null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(0);

        // When attempting to shift elements down
        var result = _grid.ShiftDown(column);

        // Then no shifting is needed because elements are already at the bottom
        Assert.That(result.IsT2, Is.True, "No shifting should be needed when elements are already at bottom");
    }

    [Test]
    public void GivenAColumnWithASingleElementAtTop_WhenShiftingDown_ThenTheElementDropsToBottom()
    {
        // Given a grid where column has a single element at the top position
        var gridData = new[,]
        {
            { null, null, ".", null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(2);

        // When shifting elements down
        var result = _grid.ShiftDown(column);

        // Then the single element drops to the bottom
        Assert.That(result.IsT0, Is.True, "Single element should drop to the bottom");
    }

    #endregion

    #region Checking Column Capacity

    [Test]
    public void GivenACompletelyFullColumn_WhenCheckingIfColumnIsFull_ThenItReturnsTrue()
    {
        // Given a grid where column 1 has no empty spaces
        var gridData = new[,]
        {
            { null, ".", null, null },
            { null, ".", null, null },
            { null, ".", null, null },
            { null, ".", null, null }
        };
        _grid = CreateGrid(gridData);

        // When checking if the column is full
        var result = _grid.IsColumnFull(new GridColumn(1));

        // Then it returns true
        Assert.That(result, Is.True, "A completely full column should be reported as full");
    }

    [Test]
    public void GivenAPartiallyFilledColumn_WhenCheckingIfColumnIsFull_ThenItReturnsFalse()
    {
        // Given a grid where column 2 has some empty spaces
        var gridData = new[,]
        {
            { null, null, null, null },
            { null, null, ".", null },
            { null, null, null, null },
            { null, null, ".", null }
        };
        _grid = CreateGrid(gridData);

        // When checking if the column is full
        var result = _grid.IsColumnFull(new GridColumn(2));

        // Then it returns false
        Assert.That(result, Is.False, "A partially filled column should not be reported as full");
    }

    [Test]
    public void GivenAnEmptyColumn_WhenCheckingIfColumnIsFull_ThenItReturnsFalse()
    {
        // Given a grid where column 3 has no elements
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);

        // When checking if the column is full
        var result = _grid.IsColumnFull(new GridColumn(3));

        // Then it returns false
        Assert.That(result, Is.False, "An empty column should not be reported as full");
    }

    #endregion

    #region Finding Available Columns

    [Test]
    public void GivenAGridWithMixedColumnStates_WhenQueryingFillableColumns_ThenOnlyNonFullColumnsAreReturned()
    {
        // Given a grid where column 1 is full but others have available space
        var gridData = new[,]
        {
            { null, ".", null, null },
            { null, ".", null, null },
            { ".", ".", null, null },
            { null, ".", null, null }
        };
        _grid = CreateGrid(gridData);

        // When querying for fillable columns
        var fillableColumns = _grid.FillableColumns;

        // Then only columns with available space are returned
        Assert.That(fillableColumns.Count, Is.EqualTo(3), "Should return 3 fillable columns");
        Assert.That(fillableColumns.Any(col => col.Index == 1), Is.False, "Full column should not be included");
        Assert.That(fillableColumns.Any(col => col.Index == 0), Is.True, "Column 0 should be fillable");
        Assert.That(fillableColumns.Any(col => col.Index == 2), Is.True, "Column 2 should be fillable");
        Assert.That(fillableColumns.Any(col => col.Index == 3), Is.True, "Column 3 should be fillable");
    }

    [Test]
    public void GivenAGridWhereAllColumnsAreFull_WhenQueryingFillableColumns_ThenNoColumnsAreReturned()
    {
        // Given a grid where all columns are completely full
        var gridData = new[,]
        {
            { ".", ".", ".", "." },
            { ".", ".", ".", "." },
            { ".", ".", ".", "." },
            { ".", ".", ".", "." }
        };
        _grid = CreateGrid(gridData);

        // When querying for fillable columns
        var fillableColumns = _grid.FillableColumns;

        // Then no columns are returned
        Assert.That(fillableColumns.Count, Is.EqualTo(0), "Should return no fillable columns when all are full");
    }

    [Test]
    public void GivenACompletelyEmptyGrid_WhenQueryingFillableColumns_ThenAllColumnsAreReturned()
    {
        // Given a grid with no elements in any column
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);

        // When querying for fillable columns
        var fillableColumns = _grid.FillableColumns;

        // Then all columns are returned as fillable
        Assert.That(fillableColumns.Count, Is.EqualTo(4), "Should return all columns when grid is empty");
    }

    #endregion

    #region Checking Shift Possibility

    [Test]
    public void GivenAFullColumnWithNoGaps_WhenCheckingIfShiftDownIsPossible_ThenItReturnsFalseWithColumnFullError()
    {
        // Given a grid where column 0 is completely full with no gaps
        var gridData = new[,]
        {
            { ".", null, null, null },
            { ".", null, null, null },
            { ".", null, null, null },
            { ".", null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(0);

        // When checking if shifting down is possible
        var result = _grid.CanShiftDown(column);

        // Then it returns false with a column full error
        Assert.That(result.IsT1, Is.True, "Should indicate column is full and cannot shift");
    }

    [Test]
    public void GivenAColumnWithElementsOnlyAtBottom_WhenCheckingIfShiftDownIsPossible_ThenItReturnsFalse()
    {
        // Given a grid where column has elements only at the bottom positions
        var gridData = new[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { ".", null, null, null },
            { ".", null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(0);

        // When checking if shifting down is possible
        var result = _grid.CanShiftDown(column);

        // Then it returns false because elements are already at the bottom
        Assert.That(result.AsT0, Is.False, "Should return false when elements are already at bottom");
    }

    [Test]
    public void GivenAnEmptyColumn_WhenCheckingIfShiftDownIsPossible_ThenItReturnsFalse()
    {
        // Given a grid where column 2 has no elements
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var column = new GridColumn(2);

        // When checking if shifting down is possible
        var result = _grid.CanShiftDown(column);

        // Then it returns false because there are no elements to shift
        Assert.That(result.AsT0, Is.False, "Should return false when there are no elements to shift");
    }

    [Test]
    public void GivenAColumnWithGapsBetweenElements_WhenCheckingIfShiftDownIsPossible_ThenItReturnsTrue()
    {
        // Given a grid where column 3 has elements with gaps between them
        var gridData = new[,]
        {
            { null, null, null, "." },
            { null, null, null, null },
            { null, null, null, "." },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(3);

        // When checking if shifting down is possible
        var result = _grid.CanShiftDown(column);

        // Then it returns true because elements can drop into gaps
        Assert.That(result.AsT0, Is.True, "Should return true when elements can drop into gaps");
    }

    [Test]
    public void GivenAColumnWithALargeGapInTheMiddle_WhenCheckingIfShiftDownIsPossible_ThenItReturnsTrue()
    {
        // Given a grid where column 3 has a large gap between elements
        var gridData = new[,]
        {
            { null, null, null, "." },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, "." }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(3);

        // When checking if shifting down is possible
        var result = _grid.CanShiftDown(column);

        // Then it returns true because the top element can drop into the gap
        Assert.That(result.AsT0, Is.True, "Should return true when elements can drop into large gaps");
    }

    [Test]
    public void GivenAColumnWithASingleElementAtTop_WhenCheckingIfShiftDownIsPossible_ThenItReturnsTrue()
    {
        // Given a grid where column has only one element at the top position
        var gridData = new[,]
        {
            { null, null, ".", null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var column = new GridColumn(2);

        // When checking if shifting down is possible
        var result = _grid.CanShiftDown(column);

        // Then it returns true because the element can drop to the bottom
        Assert.That(result.AsT0, Is.True, "Should return true when single element can drop to bottom");
    }

    #endregion
}