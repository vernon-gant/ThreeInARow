using FluentAssertions;
using OneOf;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.TestingUtilities;
using static ThreeInARow.TestingUtilities.CellExtensions;

namespace ThreeInARow.Grid.Matching.Tests;

[TestFixture]
public abstract class MatchTests
{
    private IMatch<string> _match = null!;
    private IMatch<string> _otherMatch = null!;

    private static IEnumerable<TestCaseData> ValidMatchCells()
    {
        yield return new TestCaseData(TestDistinctCells<string>.Create([CreateCell("A", 0, 0)])).SetName("Single Cell");
        yield return new TestCaseData(TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 0, 1)])).SetName("Two Cells");
        yield return new TestCaseData(TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 1, 0), CreateCell("A", 2, 0)])).SetName("Three Cells");
        yield return new TestCaseData(TestDistinctCells<string>.Create([CreateCell("B", 1, 1), CreateCell("B", 1, 2), CreateCell("B", 1, 3), CreateCell("B", 1, 4), CreateCell("B", 1, 5)])).SetName("Five Cells");
    }

    private static IEnumerable<TestCaseData> DifferentContentCells()
    {
        yield return new TestCaseData(TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("B", 0, 1)])).SetName("Two Cells with Different Content");
        yield return new TestCaseData(TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("B", 0, 1), CreateCell("B", 0, 2)])).SetName("Two Cells with Different Content and One Extra");
        yield return new TestCaseData(TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("B", 1, 1), CreateCell("C", 2, 2)])).SetName("Three Cells with Different Content");
    }

    #region Static factory Tests

    [Test, TestCaseSource(nameof(DifferentContentCells))]
    public void GivenCellsWithDifferentContent_WhenCreatingMatch_ThenThrowsArgumentException(DistinctCells<string> cells)
    {
        // Given
        // When
        var result = CreateMatch(cells);

        // Then
        result.ShouldBeOfTypeOneOf<DifferentContentFound>();
    }

    [Test, TestCaseSource(nameof(ValidMatchCells))]
    public void GivenNCells_WhenCreatingMatch_ThenNewMatchIsCreated(DistinctCells<string> cells)
    {
        // Given
        // When
        _match = CreateMatch(cells).AsT0;

        // Then
        _match.Should().NotBeNull("a match should be successfully created with the provided cells");
    }

    #endregion

    #region Count Property Tests

    [Test, TestCaseSource(nameof(ValidMatchCells))]
    public void GivenNCells_WhenCountingMatchCells_ThenCountIsN(DistinctCells<string> cells)
    {
        // Given
        _match = CreateMatch(cells).AsT0;

        // When
        var result = _match.Count;

        // Then
        result.Should().Be(cells.Count, "count should match the number of cells in the match");
    }

    #endregion

    #region Merge Method Tests

    [Test]
    public void GivenTwoNonIntersectingMatches_WhenMerging_ThenReturnsMatchDoesNotIntersect()
    {
        // Given
        var cells1 = TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 0, 1)]);
        var cells2 = TestDistinctCells<string>.Create([CreateCell("A", 2, 2), CreateCell("A", 2, 3)]);
        _match = CreateMatch(cells1).AsT0;
        _otherMatch = CreateMatch(cells2).AsT0;

        // When
        var result = _match.Merge(_otherMatch);

        // Then
        result.ShouldBeOfTypeOneOf<DoesNotIntersect>("matches should not intersect");
    }

    [Test]
    public void GivenTwoIntersectingMatches_WhenMerging_ThenReturnsMergedCellSet()
    {
        // Given - matches that share a cell at (1,1)
        var cells1 = TestDistinctCells<string>.Create([CreateCell("A", 0, 1), CreateCell("A", 1, 1)]);
        var cells2 = TestDistinctCells<string>.Create([CreateCell("A", 1, 1), CreateCell("A", 2, 1)]);
        _match = CreateMatch(cells1).AsT0;
        _otherMatch = CreateMatch(cells2).AsT0;

        // When
        var result = _match.Merge(_otherMatch);

        // Then
        result.ShouldBeOfTypeOneOf<DistinctCells<string>>("matches should merge successfully");
        result.AsT0.Count.Should().Be(3, "merged set should contain all unique cells");
    }

    [Test]
    public void GivenIdenticalMatches_WhenMerging_ThenReturnsSameCellSet()
    {
        // Given
        var cells = TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 0, 1)]);
        _match = CreateMatch(cells).AsT0;
        _otherMatch = CreateMatch(cells).AsT0;

        // When
        var result = _match.Merge(_otherMatch);

        // Then
        result.ShouldBeOfTypeOneOf<DistinctCells<string>>("merging identical matches should return the same set");
        result.AsT0.Count.Should().Be(2, "merged set should be identical to original cells");
        result.AsT0.Should().BeEquivalentTo(cells, "merged set should contain same cells");
    }

    #endregion

    #region Intersects Method Tests

    [Test]
    public void GivenTwoNonIntersectingMatches_WhenCheckingIntersection_ThenReturnsFalse()
    {
        // Given
        var cells1 = TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 0, 1)]);
        var cells2 = TestDistinctCells<string>.Create([CreateCell("A", 2, 2), CreateCell("A", 2, 3)]);
        _match = CreateMatch(cells1).AsT0;
        _otherMatch = CreateMatch(cells2).AsT0;

        // When
        var result = _match.Intersects(_otherMatch);

        // Then
        result.Should().BeFalse("matches with no common cells should not intersect");
    }

    [Test]
    public void GivenTwoIntersectingMatches_WhenCheckingIntersection_ThenReturnsTrue()
    {
        // Given - matches that share a cell at (1,1)
        var cells1 = TestDistinctCells<string>.Create([CreateCell("A", 0, 1), CreateCell("A", 1, 1)]);
        var cells2 = TestDistinctCells<string>.Create([CreateCell("A", 1, 1), CreateCell("A", 2, 1)]);
        _match = CreateMatch(cells1).AsT0;
        _otherMatch = CreateMatch(cells2).AsT0;

        // When
        var result = _match.Intersects(_otherMatch);

        // Then
        result.Should().BeTrue("matches sharing at least one cell should intersect");
    }

    [Test]
    public void GivenIdenticalMatches_WhenCheckingIntersection_ThenReturnsTrue()
    {
        // Given
        var cells = TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 0, 1)]);
        _match = CreateMatch(cells).AsT0;
        _otherMatch = CreateMatch(cells).AsT0;

        // When
        var result = _match.Intersects(_otherMatch);

        // Then
        result.Should().BeTrue("identical matches should intersect");
    }

    [Test]
    public void GivenMatchAndItself_WhenCheckingIntersection_ThenReturnsTrue()
    {
        // Given
        var cells = TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 0, 1)]);
        _match = CreateMatch(cells).AsT0;

        // When
        var result = _match.Intersects(_match);

        // Then
        result.Should().BeTrue("a match should always intersect with itself");
    }

    [Test]
    public void GivenMatchesWithAdjacentCells_WhenCheckingIntersection_ThenReturnsFalse()
    {
        // Given - matches that are adjacent but don't share cells
        var cells1 = TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 0, 1)]);
        var cells2 = TestDistinctCells<string>.Create([CreateCell("A", 0, 2), CreateCell("A", 0, 3)]);
        _match = CreateMatch(cells1).AsT0;
        _otherMatch = CreateMatch(cells2).AsT0;

        // When
        var result = _match.Intersects(_otherMatch);

        // Then
        result.Should().BeFalse("adjacent matches without shared cells should not intersect");
    }

    [Test]
    public void GivenLargeMatchAndSmallSubsetMatch_WhenCheckingIntersection_ThenReturnsTrue()
    {
        // Given - one match contains a subset of the other's cells
        var cells1 = TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 0, 1), CreateCell("A", 0, 2), CreateCell("A", 0, 3)]);
        var cells2 = TestDistinctCells<string>.Create([CreateCell("A", 0, 1), CreateCell("A", 0, 2)]);
        _match = CreateMatch(cells1).AsT0;
        _otherMatch = CreateMatch(cells2).AsT0;

        // When
        var result = _match.Intersects(_otherMatch);

        // Then
        result.Should().BeTrue("matches where one is a subset of the other should intersect");
    }

    [Test]
    public void GivenMatchesWithSingleSharedCell_WhenCheckingIntersection_ThenReturnsTrue()
    {
        // Given - matches that share exactly one cell
        var cells1 = TestDistinctCells<string>.Create([CreateCell("A", 0, 0), CreateCell("A", 0, 1), CreateCell("A", 0, 2)]);
        var cells2 = TestDistinctCells<string>.Create([CreateCell("A", 0, 2), CreateCell("A", 1, 2), CreateCell("A", 2, 2)]);
        _match = CreateMatch(cells1).AsT0;
        _otherMatch = CreateMatch(cells2).AsT0;

        // When
        var result = _match.Intersects(_otherMatch);

        // Then
        result.Should().BeTrue("matches sharing even a single cell should intersect");
    }

    #endregion

    protected abstract OneOf<DistinctCellsMatch<string>, DifferentContentFound> CreateMatch(DistinctCells<string> cells);
}

[TestFixture]
public class HorizontalMatchTests : MatchTests
{
    protected override OneOf<DistinctCellsMatch<string>, DifferentContentFound> CreateMatch(DistinctCells<string> cells)
        => HorizontalMatch<string>.Create(cells).Match<OneOf<DistinctCellsMatch<string>, DifferentContentFound>>(
            horizontal => horizontal,
            error => error
        );
}

[TestFixture]
public class VerticalMatchTests : MatchTests
{
    protected override OneOf<DistinctCellsMatch<string>, DifferentContentFound> CreateMatch(DistinctCells<string> cells)
        => VerticalMatch<string>.Create(cells).Match<OneOf<DistinctCellsMatch<string>, DifferentContentFound>>(
            horizontal => horizontal,
            error => error
        );
}

[TestFixture]
public class CrossMatchTests : MatchTests
{
    protected override OneOf<DistinctCellsMatch<string>, DifferentContentFound> CreateMatch(DistinctCells<string> cells)
        => CrossMatch<string>.Create(cells).Match<OneOf<DistinctCellsMatch<string>, DifferentContentFound>>(
            horizontal => horizontal,
            error => error
        );
}

[TestFixture]
public class TMatchTests : MatchTests
{
    protected override OneOf<DistinctCellsMatch<string>, DifferentContentFound> CreateMatch(DistinctCells<string> cells)
        => TMatch<string>.Create(cells).Match<OneOf<DistinctCellsMatch<string>, DifferentContentFound>>(
            horizontal => horizontal,
            error => error
        );
}

[TestFixture]
public class LMatchTests : MatchTests
{
    protected override OneOf<DistinctCellsMatch<string>, DifferentContentFound> CreateMatch(DistinctCells<string> cells)
        => LMatch<string>.Create(cells).Match<OneOf<DistinctCellsMatch<string>, DifferentContentFound>>(
            horizontal => horizontal,
            error => error
        );
}