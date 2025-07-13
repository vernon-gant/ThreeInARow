using FluentAssertions;
using NSubstitute;
using OneOf.Types;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Progression.Events;
using ThreeInARow.Progression.Statistics.Implementation.Statistics.General;
using ThreeInARow.TestingUtilities;
using ThreeInARow.ValueObjects;

namespace ThreeInARow.Progression.Statistics.Tests;

[TestFixture]
public class TotalMatchesTests
{
    private TotalMatches<string> _totalMatches;
    private IMatch<string> _mockMatch;

    [SetUp]
    public void SetUp()
    {
        _totalMatches = new TotalMatches<string>();
        _mockMatch = Substitute.For<IMatch<string>>();
    }

    #region Basic Property Tests

    [Test]
    public void GivenTotalMatches_WhenGettingName_ThenReturnsCorrectName()
    {
        // Given
        // When
        var name = _totalMatches.Name;

        // Then
        name.Value.Should().Be("Total Matches");
    }

    [Test]
    public void GivenTotalMatches_WhenGettingDescription_ThenReturnsCorrectDescription()
    {
        // Given
        // When
        var description = _totalMatches.Description;

        // Then
        description.ShouldBeOfTypeOneOf<NonEmptyString>();
        description.AsT0.Value.Should().Be("Counts all matches including cascades");
    }

    [Test]
    public void GivenTotalMatches_WhenGettingUnit_ThenReturnsNone()
    {
        // Given
        // When
        var unit = _totalMatches.Unit;

        // Then
        unit.ShouldBeOfTypeOneOf<None>();
    }

    #endregion

    #region Value Property Tests

    [Test]
    public void GivenNoMatchesHandled_WhenGettingValue_ThenReturnsZero()
    {
        // Given
        // When
        var value = _totalMatches.Value;

        // Then
        value.ShouldBeOfTypeOneOf<NonEmptyString>();
        value.AsT0.Value.Should().Be("0");
    }

    [Test]
    public void GivenOneDirectMatchFound_WhenGettingValue_ThenReturnsOne()
    {
        // Given
        var matchEvent = new MatchFound<string>
        {
            Match = _mockMatch,
            IsCascade = false
        };
        _totalMatches.Handle(matchEvent);

        // When
        var value = _totalMatches.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Value.Should().Be("1");
    }

    [Test]
    public void GivenOneCascadeMatchFound_WhenGettingValue_ThenReturnsOne()
    {
        // Given
        var matchEvent = new MatchFound<string>
        {
            Match = _mockMatch,
            IsCascade = true
        };
        _totalMatches.Handle(matchEvent);

        // When
        var value = _totalMatches.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Value.Should().Be("1");
    }

    [Test]
    public void GivenMultipleDirectMatches_WhenGettingValue_ThenReturnsCorrectCount()
    {
        // Given
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = false });
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = false });
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = false });

        // When
        var value = _totalMatches.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Value.Should().Be("3");
    }

    [Test]
    public void GivenMultipleCascadeMatches_WhenGettingValue_ThenReturnsCorrectCount()
    {
        // Given
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = true });
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = true });
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = true });

        // When
        var value = _totalMatches.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Value.Should().Be("3");
    }

    [Test]
    public void GivenMixedDirectAndCascadeMatches_WhenGettingValue_ThenReturnsCorrectTotalCount()
    {
        // Given
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = false });
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = true });
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = false });
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = true });
        _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = true });

        // When
        var value = _totalMatches.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Value.Should().Be("5", "should count all matches regardless of cascade status");
    }

    #endregion

    #region Event Handling Tests

    [Test]
    public void GivenMatchFoundEventDirect_WhenHandling_ThenIncrementsCounter()
    {
        // Given
        var initialValue = int.Parse(_totalMatches.Value.AsT0);
        var matchEvent = new MatchFound<string>
        {
            Match = _mockMatch,
            IsCascade = false
        };

        // When
        _totalMatches.Handle(matchEvent);

        // Then
        var newValue = int.Parse(_totalMatches.Value.AsT0.Value);
        newValue.Should().Be(initialValue + 1);
    }

    [Test]
    public void GivenMatchFoundEventCascade_WhenHandling_ThenIncrementsCounter()
    {
        // Given
        var initialValue = int.Parse(_totalMatches.Value.AsT0);
        var matchEvent = new MatchFound<string>
        {
            Match = _mockMatch,
            IsCascade = true
        };

        // When
        _totalMatches.Handle(matchEvent);

        // Then
        var newValue = int.Parse(_totalMatches.Value.AsT0.Value);
        newValue.Should().Be(initialValue + 1);
    }

    [Test]
    public void GivenSequenceOfMatches_WhenHandling_ThenCountsAllMatchesRegardlessOfCascadeStatus()
    {
        // Given
        var matches = new[]
        {
            new MatchFound<string> { Match = _mockMatch, IsCascade = false },
            new MatchFound<string> { Match = _mockMatch, IsCascade = true },
            new MatchFound<string> { Match = _mockMatch, IsCascade = true },
            new MatchFound<string> { Match = _mockMatch, IsCascade = false },
            new MatchFound<string> { Match = _mockMatch, IsCascade = true },
            new MatchFound<string> { Match = _mockMatch, IsCascade = false },
            new MatchFound<string> { Match = _mockMatch, IsCascade = false }
        };

        // When
        foreach (var match in matches)
        {
            _totalMatches.Handle(match);
        }

        // Then
        _totalMatches.Value.AsT0.Value.Should().Be("7", "should count all matches in the sequence");
    }

    [Test]
    public void GivenLargeNumberOfMatches_WhenHandling_ThenMaintainsAccurateCount()
    {
        // Given
        const int totalMatchCount = 1000;
        const int cascadeMatches = 400;

        // When
        for (int i = 0; i < cascadeMatches; i++)
        {
            _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = true });
        }

        for (int i = 0; i < (totalMatchCount - cascadeMatches); i++)
        {
            _totalMatches.Handle(new MatchFound<string> { Match = _mockMatch, IsCascade = false });
        }

        // Then
        _totalMatches.Value.AsT0.Value.Should().Be(totalMatchCount.ToString());
    }

    [Test]
    public void GivenDifferentMatchTypes_WhenHandling_ThenCountsAllMatches()
    {
        // Given
        var horizontalMatch = Substitute.For<IMatch<string>>();
        var verticalMatch = Substitute.For<IMatch<string>>();
        var tShapeMatch = Substitute.For<IMatch<string>>();

        // When
        _totalMatches.Handle(new MatchFound<string> { Match = horizontalMatch, IsCascade = false });
        _totalMatches.Handle(new MatchFound<string> { Match = verticalMatch, IsCascade = true });
        _totalMatches.Handle(new MatchFound<string> { Match = tShapeMatch, IsCascade = false });

        // Then
        _totalMatches.Value.AsT0.Value.Should().Be("3", "should count all match types");
    }

    #endregion
}