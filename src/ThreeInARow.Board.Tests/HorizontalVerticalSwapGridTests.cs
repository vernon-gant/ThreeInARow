namespace ThreeInARow.Board.Tests;

[TestFixture]
public class HorizontalVerticalSwapGridTests : BaseGridBehaviorTests<HorizontalVerticalSwapGrid<Element>>
{
    protected override HorizontalVerticalSwapGrid<Element> CreateGrid(Element?[,] gridData) => new(gridData);

    #region Swap Command Tests - Horizontal Adjacent

    [Test]
    public void Swap_GivenTwoHorizontallyAdjacentCellsWithElements_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with two horizontally adjacent cells containing different elements
        var gridData = new Element?[,]
        {
            { new("X"), new("Y"), null, null },
            { null,     null,     null, null },
            { null,     null,     null, null },
            { null,     null,     null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(0, 0).AsT0;
        var cell2 = _grid.TryGetCell(0, 1).AsT0;

        // When swapping the horizontally adjacent cells
        var result = _grid.Swap(cell1, cell2);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for horizontally adjacent cells");

        var cell1After = _grid.Single(c => c.IsInRow(cell1.Row) && c.IsInColumn(cell1.Column));
        var cell2After = _grid.Single(c => c.IsInRow(cell2.Row) && c.IsInColumn(cell2.Column));

        Assert.That(cell1After.MatchesWith(new ElementCell<Element>(new Element("Y"), new GridCell(0, 0))), Is.True, "Cell1 should now contain element Y");
        Assert.That(cell2After.MatchesWith(new ElementCell<Element>(new Element("X"), new GridCell(0, 1))), Is.True, "Cell2 should now contain element X");
    }

    [Test]
    public void Swap_GivenHorizontallyAdjacentCellsAtRightEdge_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with horizontally adjacent cells at the right edge
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, new("Edge1"), new("Edge2") },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(1, 2).AsT0;  // Second to last column
        var cell2 = _grid.TryGetCell(1, 3).AsT0;  // Last column (horizontally adjacent)

        // When swapping cells at grid edge
        var result = _grid.Swap(cell1, cell2);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for adjacent cells at grid edge");

        var cell1After = _grid.Single(c => c.IsInRow(cell1.Row) && c.IsInColumn(cell1.Column));
        var cell2After = _grid.Single(c => c.IsInRow(cell2.Row) && c.IsInColumn(cell2.Column));

        Assert.That(cell1After.MatchesWith(new ElementCell<Element>(new Element("Edge2"), new GridCell(1, 2))), Is.True, "Cell1 should now contain Edge2");
        Assert.That(cell2After.MatchesWith(new ElementCell<Element>(new Element("Edge1"), new GridCell(1, 3))), Is.True, "Cell2 should now contain Edge1");
    }

    [Test]
    public void Swap_GivenHorizontallyAdjacentCellsAtLeftEdge_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with horizontally adjacent cells at the left edge
        var gridData = new Element?[,]
        {
            { null,         null,         null, null },
            { null,         null,         null, null },
            { new("Left1"), new("Left2"), null, null },
            { null,         null,         null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(2, 0).AsT0;
        var cell2 = _grid.TryGetCell(2, 1).AsT0;

        // When swapping cells at left edge
        var result = _grid.Swap(cell1, cell2);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for adjacent cells at left edge");

        var cell1After = _grid.Single(c => c.IsInRow(cell1.Row) && c.IsInColumn(cell1.Column));
        var cell2After = _grid.Single(c => c.IsInRow(cell2.Row) && c.IsInColumn(cell2.Column));

        Assert.That(cell1After.MatchesWith(new ElementCell<Element>(new Element("Left2"), new GridCell(2, 0))), Is.True, "Cell1 should now contain Left2");
        Assert.That(cell2After.MatchesWith(new ElementCell<Element>(new Element("Left1"), new GridCell(2, 1))), Is.True, "Cell2 should now contain Left1");
    }

    #endregion

    #region Swap Command Tests - Vertical Adjacent

    [Test]
    public void Swap_GivenTwoVerticallyAdjacentCellsWithElements_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with two vertically adjacent cells containing different elements
        var gridData = new Element?[,]
        {
            { null, new("A"), null, null },
            { null, new("B"), null, null },
            { null, null,     null, null },
            { null, null,     null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(0, 1).AsT0;  // Top cell with A
        var cell2 = _grid.TryGetCell(1, 1).AsT0;  // Bottom cell with B (vertically adjacent)

        // When swapping the vertically adjacent cells
        var result = _grid.Swap(cell1, cell2);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for vertically adjacent cells");

        var cell1After = _grid.Single(c => c.IsInRow(cell1.Row) && c.IsInColumn(cell1.Column));
        var cell2After = _grid.Single(c => c.IsInRow(cell2.Row) && c.IsInColumn(cell2.Column));

        Assert.That(cell1After.MatchesWith(new ElementCell<Element>(new Element("B"), new GridCell(0, 1))), Is.True, "Top cell should now contain element B");
        Assert.That(cell2After.MatchesWith(new ElementCell<Element>(new Element("A"), new GridCell(1, 1))), Is.True, "Bottom cell should now contain element A");
    }

    [Test]
    public void Swap_GivenVerticallyAdjacentCellsAtBottomEdge_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with vertically adjacent cells at the bottom edge
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, new("Bottom1"), null, null },
            { null, new("Bottom2"), null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(2, 1).AsT0;  // Second to last row
        var cell2 = _grid.TryGetCell(3, 1).AsT0;  // Last row (vertically adjacent)

        // When swapping cells at bottom edge
        var result = _grid.Swap(cell1, cell2);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for adjacent cells at bottom edge");

        var cell1After = _grid.Single(c => c.IsInRow(cell1.Row) && c.IsInColumn(cell1.Column));
        var cell2After = _grid.Single(c => c.IsInRow(cell2.Row) && c.IsInColumn(cell2.Column));

        Assert.That(cell1After.MatchesWith(new ElementCell<Element>(new Element("Bottom2"), new GridCell(2, 1))), Is.True, "Cell1 should now contain Bottom2");
        Assert.That(cell2After.MatchesWith(new ElementCell<Element>(new Element("Bottom1"), new GridCell(3, 1))), Is.True, "Cell2 should now contain Bottom1");
    }

    [Test]
    public void Swap_GivenVerticallyAdjacentCellsAtTopEdge_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with vertically adjacent cells at the top edge
        var gridData = new Element?[,]
        {
            { null, null, new("Top1"), null },
            { null, null, new("Top2"), null },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(0, 2).AsT0;  // First row
        var cell2 = _grid.TryGetCell(1, 2).AsT0;  // Second row (vertically adjacent)

        // When swapping cells at top edge
        var result = _grid.Swap(cell1, cell2);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for adjacent cells at top edge");

        var cell1After = _grid.Single(c => c.IsInRow(cell1.Row) && c.IsInColumn(cell1.Column));
        var cell2After = _grid.Single(c => c.IsInRow(cell2.Row) && c.IsInColumn(cell2.Column));

        Assert.That(cell1After.MatchesWith(new ElementCell<Element>(new Element("Top2"), new GridCell(0, 2))), Is.True, "Cell1 should now contain Top2");
        Assert.That(cell2After.MatchesWith(new ElementCell<Element>(new Element("Top1"), new GridCell(1, 2))), Is.True, "Cell2 should now contain Top1");
    }

    #endregion

    #region Swap Command Tests - Occupied/Empty Combinations

    [Test]
    public void Swap_GivenOneEmptyAndOneOccupiedAdjacentCells_WhenSwapping_ThenReturnsSuccessAndSwapsContent()
    {
        // Given a grid with one occupied and one empty adjacent cell
        var gridData = new Element?[,]
        {
            { null, new("Z"), null, null },
            { null, null,     null, null },
            { null, null,     null, null },
            { null, null,     null, null }
        };
        var _grid = CreateGrid(gridData);

        var occupiedCell = _grid.TryGetCell(0, 1).AsT0;  // Has element Z
        var emptyCell = _grid.TryGetCell(0, 2).AsT0;     // Empty, horizontally adjacent

        // When swapping occupied and empty cells
        var result = _grid.Swap(occupiedCell, emptyCell);

        // Then operation succeeds and content is swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for occupied and empty adjacent cells");

        var occupiedCellAfter = _grid.Single(c => c.IsInRow(occupiedCell.Row) && c.IsInColumn(occupiedCell.Column));
        var emptyCellAfter = _grid.Single(c => c.IsInRow(emptyCell.Row) && c.IsInColumn(emptyCell.Column));

        Assert.That(occupiedCellAfter.IsOccupied(), Is.False, "Previously occupied cell should now be empty");
        Assert.That(emptyCellAfter.MatchesWith(new ElementCell<Element>(new Element("Z"), new GridCell(0, 2))), Is.True, "Previously empty cell should now contain the element");
    }

    [Test]
    public void Swap_GivenTwoEmptyAdjacentCells_WhenSwapping_ThenReturnsSuccessAndCellsRemainEmpty()
    {
        // Given a grid with two empty adjacent cells
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = CreateGrid(gridData);

        var emptyCell1 = _grid.TryGetCell(2, 1).AsT0;
        var emptyCell2 = _grid.TryGetCell(2, 2).AsT0;  // Horizontally adjacent

        // When swapping two empty cells
        var result = _grid.Swap(emptyCell1, emptyCell2);

        // Then operation succeeds and cells remain empty
        Assert.That(result.IsT0, Is.True, "Swap should return Success for two empty adjacent cells");

        var cell1After = _grid.Single(c => c.IsInRow(emptyCell1.Row) && c.IsInColumn(emptyCell1.Column));
        var cell2After = _grid.Single(c => c.IsInRow(emptyCell2.Row) && c.IsInColumn(emptyCell2.Column));

        Assert.That(cell1After.IsOccupied(), Is.False, "First cell should remain empty");
        Assert.That(cell2After.IsOccupied(), Is.False, "Second cell should remain empty");
    }

    #endregion

    #region Swap Command Tests - Invalid Cases

    [Test]
    public void Swap_GivenTwoDiagonallyAdjacentCells_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with two diagonally adjacent cells
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, new("Center"), null, null },
            { null, null, new("Diagonal"), null },
            { null, null, null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(1, 1).AsT0;  // Center
        var cell2 = _grid.TryGetCell(2, 2).AsT0;  // Diagonally adjacent

        // When attempting to swap diagonally adjacent cells
        var result = _grid.Swap(cell1, cell2);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for diagonally adjacent cells");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenTwoNonAdjacentCells_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with two non-adjacent cells
        var gridData = new Element?[,]
        {
            { new("TopLeft"), null, null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, new("BottomRight") }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(0, 0).AsT0;  // Top-left corner
        var cell2 = _grid.TryGetCell(3, 3).AsT0;  // Bottom-right corner

        // When attempting to swap non-adjacent cells
        var result = _grid.Swap(cell1, cell2);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for non-adjacent cells");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenSameCell_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with the same cell for both swap positions
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, null, new("Same"), null },
            { null, null, null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell = _grid.TryGetCell(2, 2).AsT0;

        // When attempting to swap a cell with itself
        var result = _grid.Swap(cell, cell);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap when swapping cell with itself");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenTwoCellsWithTwoRowsDifference_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with two cells in the same column but with 2 rows difference
        var gridData = new Element?[,]
        {
            { null, null, new("Top"), null },
            { null, null, null, null },
            { null, null, new("Bottom"), null },
            { null, null, null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(0, 2).AsT0;
        var cell2 = _grid.TryGetCell(2, 2).AsT0;  // Same column, 2 rows apart

        // When attempting to swap cells with 2 rows difference
        var result = _grid.Swap(cell1, cell2);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for cells 2 rows apart");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenTwoCellsWithTwoColumnsDifference_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with two cells in the same row but with 2 columns difference
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { new("Left"), null, new("Right"), null },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(1, 0).AsT0;
        var cell2 = _grid.TryGetCell(1, 2).AsT0;  // Same row, 2 columns apart

        // When attempting to swap cells with 2 columns difference
        var result = _grid.Swap(cell1, cell2);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for cells 2 columns apart");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenCellsAtOppositeCorners_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with cells at opposite corners
        var gridData = new Element?[,]
        {
            { new("TopLeft"), null, null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, new("BottomRight") }
        };
        var _grid = CreateGrid(gridData);

        var topLeftCell = _grid.TryGetCell(0, 0).AsT0;
        var bottomRightCell = _grid.TryGetCell(3, 3).AsT0;

        // When attempting to swap opposite corner cells
        var result = _grid.Swap(topLeftCell, bottomRightCell);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for opposite corner cells");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenCellsInSameRowButNotAdjacent_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with cells in the same row but not adjacent
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { new("SameRow1"), null, null, new("SameRow2") },
            { null, null, null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(2, 0).AsT0;  // First column
        var cell2 = _grid.TryGetCell(2, 3).AsT0;  // Last column (same row, not adjacent)

        // When attempting to swap non-adjacent cells in same row
        var result = _grid.Swap(cell1, cell2);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for non-adjacent cells in same row");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenCellsInSameColumnButNotAdjacent_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with cells in the same column but not adjacent
        var gridData = new Element?[,]
        {
            { null, new("SameCol1"), null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, new("SameCol2"), null, null }
        };
        var _grid = CreateGrid(gridData);

        var cell1 = _grid.TryGetCell(0, 1).AsT0;  // First row
        var cell2 = _grid.TryGetCell(3, 1).AsT0;  // Last row (same column, not adjacent)

        // When attempting to swap non-adjacent cells in same column
        var result = _grid.Swap(cell1, cell2);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for non-adjacent cells in same column");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    #endregion
}