using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.ValueObjects;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Grid.Tests;

[TestFixture]
public abstract class ManageableGridBehaviorTests<TGrid> : GridTestsBase<IManageableGrid<string>>, MGridTestUtility
{
    #region Removing Elements from Grid

    [Test]
    public void GivenACellContainingAnElement_WhenPlayerDeletesTheElement_ThenTheCellBecomesEmpty()
    {
        // Given a grid with an element at position (1,1)
        var gridData = new[,]
        {
            { null, null, null, null },
            { null, "A", null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        var readableGrid = this.CreateTestReadableGrid(gridData);
        _grid = CreateGrid(gridData);
        var cell = Cell<string>.FromGrid(readableGrid, 1, 1).AsT0;

        // When the player deletes the element
        var result = _grid.Delete(cell);

        // Then the operation succeeds and the cell becomes empty
        Assert.That(result.IsT0, Is.True, "Element should be successfully deleted");
    }

    [Test]
    public void GivenAnEmptyCell_WhenPlayerAttemptsToDeleteFromIt_ThenTheOperationIsRejected()
    {
        // Given a grid where position (1,1) is already empty
        var gridData = EmptyGrid(4, 4);
        _grid = CreateGrid(gridData);
        var readableGrid = this.CreateTestReadableGrid(gridData);
        var cell = Cell<string>.FromGrid(readableGrid, 1, 1).AsT0;

        // When the player attempts to delete from the empty cell
        var result = _grid.Delete(cell);

        // Then the operation is rejected because there's nothing to delete
        Assert.That(result.IsT1, Is.True, "Deletion should be rejected for empty cells");
    }

    [Test]
    public void GivenACellThatWasPreviouslyDeleted_WhenPlayerTriesToDeleteAgain_ThenTheOperationIsRejected()
    {
        // Given a grid with an element that has already been deleted
        var gridData = new[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, null, "A", null },
            { null, null, null, null }
        };
        _grid = CreateGrid(gridData);
        var readableGrid = this.CreateTestReadableGrid(gridData);
        var cell = Cell<string>.FromGrid(readableGrid, 2, 2).AsT0;

        _grid.Delete(cell); // First deletion

        // When the player attempts to delete from the same cell again
        var result = _grid.Delete(cell);

        // Then the operation is rejected because the cell is already empty
        Assert.That(result.IsT1, Is.True, "Second deletion should be rejected");
    }

    #endregion
}