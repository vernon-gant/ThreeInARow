using FluentAssertions;
using NSubstitute;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Progression.Statistics.ADT;
using ThreeInARow.Progression.Statistics.Implementation.Statistics.General;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Progression.Statistics.Tests;

[TestFixture]
public class GameDurationTests
{
    private IGameTimer _gameTimer;
    private GameDuration _gameDuration;

    [SetUp]
    public void SetUp()
    {
        _gameTimer = Substitute.For<IGameTimer>();
        _gameDuration = new GameDuration(_gameTimer);
    }

    #region Basic Property Tests

    [Test]
    public void GivenGameDuration_WhenGettingName_ThenReturnsCorrectName()
    {
        // Given / When
        var name = _gameDuration.Name;

        // Then
        name.Should().Be("Game Duration");
    }

    [Test]
    public void GivenGameDuration_WhenGettingDescription_ThenReturnsCorrectDescription()
    {
        // Given / When
        var desc = _gameDuration.Description;

        // Then
        desc.ShouldBeOfTypeOneOf<string>();
        desc.AsT0.Should().Be("Total time from start to end of the game");
    }

    [Test]
    public void GivenGameDuration_WhenGettingUnit_ThenReturnsCorrectUnit()
    {
        // Given / When
        var unit = _gameDuration.Unit;

        // Then
        unit.ShouldBeOfTypeOneOf<string>();
        unit.AsT0.Should().Be("mm:ss");
    }

    #endregion

    #region Value Property Tests

    [Test]
    public void GivenNotPossibleElapsedTime_WhenValueIsQueried_ThenReturnsNotEnoughData()
    {
        // Given
        _gameTimer.ElapsedGameTime.Returns(new NotPossible());

        // When
        var value = _gameDuration.Value;

        // Then
        value.ShouldBeOfTypeOneOf<NotEnoughData>();
    }

    [Test]
    public void GivenNormalElapsedTime_WhenValueIsQueried_ThenReturnsFormattedDuration()
    {
        // Given
        _gameTimer.ElapsedGameTime.Returns(TimeSpan.FromSeconds(123));

        // When
        var value = _gameDuration.Value;

        // Then
        value.ShouldBeOfTypeOneOf<string>();
        value.AsT0.Should().Be("02:03", "because the elapsed time should be formatted as mm:ss");
    }

    #endregion
}