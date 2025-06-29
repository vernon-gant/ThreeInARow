using FluentAssertions;
using NSubstitute;
using OneOf.Types;
using ThreeInARow.Infrastructure.ValueObjects;
using ThreeInARow.Progression.Statistics.ADT;
using ThreeInARow.Progression.Statistics.Implementation.Statistics.General;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Progression.Statistics.Tests;

[TestFixture]
public class TotalScoreTests
{
    private IScoreTracker _scoreTracker;
    private TotalScore _totalScore;

    [SetUp]
    public void SetUp()
    {
        _scoreTracker = Substitute.For<IScoreTracker>();
        _totalScore = new TotalScore(_scoreTracker);
    }

    #region Name Property Tests

    [Test]
    public void GivenTotalScore_WhenGettingName_ThenReturnsTotalScore()
    {
        // Given
        // When
        var name = _totalScore.Name;

        // Then
        name.Should().Be("Total Score");
    }

    #endregion

    #region Description Property Tests

    [Test]
    public void GivenTotalScore_WhenGettingDescription_ThenReturnsNone()
    {
        // Given
        // When
        var description = _totalScore.Description;

        // Then
        description.ShouldBeOfTypeOneOf<None>();
    }

    #endregion

    #region Unit Property Tests

    [Test]
    public void GivenTotalScore_WhenGettingUnit_ThenReturnsNone()
    {
        // Given
        // When
        var unit = _totalScore.Unit;

        // Then
        unit.ShouldBeOfTypeOneOf<None>();
    }

    #endregion

    #region Value Property Tests

    [Test]
    [TestCase(0)]
    [TestCase(12345)]
    [TestCase(999999)]
    public void GivenScoreTracker_WhenGettingValue_ThenReturnsScoreTrackerValueAsString(int value)
    {
        // Given
        _scoreTracker.CurrentScore.Returns(new PositiveInt(value));

        // When
        var result = _totalScore.Value;

        // Then
        result.ShouldBeOfTypeOneOf<string>();
        result.AsT0.Should().Be(value.ToString(), "because the value should be formatted as a string");
    }

    #endregion
}