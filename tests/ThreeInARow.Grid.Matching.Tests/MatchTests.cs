using FluentAssertions;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;
using ThreeInARow.TestingUtilities;
using static ThreeInARow.TestingUtilities.CellExtensions;

namespace ThreeInARow.Grid.Matching.Tests;

[TestFixture]
public abstract class MatchTests
{
    private IMatch<string> _match = null!;
    private IMatch<string> _otherMatch = null!;

    #region Count Property Tests

    private static IEnumerable<TestCaseData> CountingTestCases()
    {
        yield return new TestCaseData(new HashSet<Cell<string>>(), 0).SetName("Empty Match");
        yield return new TestCaseData(new HashSet<Cell<string>> { CreateCell("A", 0, 0) }, 1).SetName("Single Cell");
        yield return new TestCaseData(new HashSet<Cell<string>> { CreateCell("A", 0, 0), CreateCell("A", 0, 1) }, 2).SetName("Two Cells");
        yield return new TestCaseData(new HashSet<Cell<string>> { CreateCell("A", 0, 0), CreateCell("A", 1, 0), CreateCell("A", 2, 0) }, 3).SetName("Three Cells");
        yield return new TestCaseData(new HashSet<Cell<string>> { CreateCell("B", 1, 1), CreateCell("B", 1, 2), CreateCell("B", 1, 3), CreateCell("B", 1, 4), CreateCell("B", 1, 5) }, 5).SetName("Five Cells");
    }

    [Test, TestCaseSource(nameof(CountingTestCases))]
    public void GivenNCells_WhenCountingMatchCells_ThenCountIsN(HashSet<Cell<string>> cells, int expectedCount)
    {
        // Given
        _match = CreateMatch(cells);

        // When
        var result = _match.Count;

        // Then
        result.Should().Be(expectedCount);
    }

    #endregion

    #region Merge Method Tests

    [Test]
    public void GivenTwoNonIntersectingMatches_WhenMerging_ThenReturnsMatchDoesNotIntersect()
    {
        // Given
        var cells1 = new HashSet<Cell<string>> { CreateCell("A", 0, 0), CreateCell("A", 0, 1) };
        var cells2 = new HashSet<Cell<string>> { CreateCell("B", 2, 2), CreateCell("B", 2, 3) };
        _match = CreateMatch(cells1);
        _otherMatch = CreateMatch(cells2);

        // When
        var result = _match.Merge(_otherMatch);

        // Then
        result.ShouldBeOfTypeOneOf<MatchDoesNotIntersect>("matches should not intersect");
    }

    [Test]
    public void GivenTwoIntersectingMatches_WhenMerging_ThenReturnsMergedCellSet()
    {
        // Given - matches that share a cell at (1,1)
        var cells1 = new HashSet<Cell<string>> { CreateCell("A", 0, 1), CreateCell("A", 1, 1) };
        var cells2 = new HashSet<Cell<string>> { CreateCell("A", 1, 1), CreateCell("A", 2, 1) };
        _match = CreateMatch(cells1);
        _otherMatch = CreateMatch(cells2);

        // When
        var result = _match.Merge(_otherMatch);

        // Then
        result.ShouldBeOfTypeOneOf<HashSet<Cell<string>>>("matches should merge successfully");
        result.AsT0.Should().HaveCount(3, "merged set should contain all unique cells");
    }

    [Test]
    public void GivenIdenticalMatches_WhenMerging_ThenReturnsSameCellSet()
    {
        // Given
        var cells = new HashSet<Cell<string>> { CreateCell("A", 0, 0), CreateCell("A", 0, 1) };
        _match = CreateMatch(cells);
        _otherMatch = CreateMatch(cells);

        // When
        var result = _match.Merge(_otherMatch);

        // Then
        result.ShouldBeOfTypeOneOf<HashSet<Cell<string>>>("merging identical matches should return the same set");
        result.AsT0.Should().BeEquivalentTo(cells, "merged set should be identical to original cells");
    }

    [Test]
    public void GivenMatchAndEmptyMatch_WhenMerging_ThenReturnsMatchDoesNotIntersect()
    {
        // Given
        var cells = new HashSet<Cell<string>> { CreateCell("A", 0, 0) };
        _match = CreateMatch(cells);
        _otherMatch = CreateMatch([]);

        // When
        var result = _match.Merge(_otherMatch);

        // Then
        result.ShouldBeOfTypeOneOf<MatchDoesNotIntersect>("merging with an empty match should return MatchDoesNotIntersect");
    }

    [Test]
    public void GivenTwoEmptyMatches_WhenMerging_ThenReturnsMatchDoesNotIntersect()
    {
        // Given
        _match = CreateMatch([]);
        _otherMatch = CreateMatch([]);

        // When
        var result = _match.Merge(_otherMatch);

        // Then
        result.ShouldBeOfTypeOneOf<MatchDoesNotIntersect>("merging two empty matches should return MatchDoesNotIntersect");
    }

    #endregion

    protected abstract IMatch<string> CreateMatch(HashSet<Cell<string>> cells);
}

[TestFixture]
public class HorizontalMatchTests : MatchTests
{
    protected override IMatch<string> CreateMatch(HashSet<Cell<string>> cells) => new HorizontalMatch<string>(cells);
}

[TestFixture]
public class VerticalMatchTests : MatchTests
{
    protected override IMatch<string> CreateMatch(HashSet<Cell<string>> cells) => new VerticalMatch<string>(cells);
}

[TestFixture]
public class CrossMatchTests : MatchTests
{
    protected override IMatch<string> CreateMatch(HashSet<Cell<string>> cells) => new CrossMatch<string>(cells);
}

[TestFixture]
public class TMatchTests : MatchTests
{
    protected override IMatch<string> CreateMatch(HashSet<Cell<string>> cells) => new TMatch<string>(cells);
}

[TestFixture]
public class LMatchTests : MatchTests
{
    protected override IMatch<string> CreateMatch(HashSet<Cell<string>> cells) => new LMatch<string>(cells);
}