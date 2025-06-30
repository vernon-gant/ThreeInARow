using FluentAssertions;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.Implementation;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Infrastructure.Tests;

[TestFixture]
public class SystemStopwatchTests
{
    private SystemStopwatchTimer _systemStopwatch;

    [SetUp]
    public void Setup() => _systemStopwatch = new SystemStopwatchTimer();

    [Test]
    public void GivenNewTimer_WhenElapsedQueried_ThenReturnsNotPossible()
    {
        // Given / When
        var result = _systemStopwatch.ElapsedGameTime;

        // Then
        result.ShouldBeOfTypeOneOf<NotPossible>();
    }

    [Test]
    public void GivenNewTimer_WhenStopGameCalled_ThenReturnsNotPossible()
    {
        // When
        var result = _systemStopwatch.StopGame();

        // Then
        result.ShouldBeOfTypeOneOf<NotPossible>();
    }

    [Test]
    public void GivenNewTimer_WhenStartGameCalled_ThenReturnsSuccess()
    {
        // When
        var result = _systemStopwatch.StartGame();

        // Then
        result.ShouldBeOfTypeOneOf<Success>();
    }

    [Test]
    public void GivenStartedTimer_WhenStartGameCalledAgain_ThenReturnsNotPossibleAndElapsedTimeIncreases()
    {
        // Given
        _systemStopwatch.StartGame();
        var firstElapsed = _systemStopwatch.ElapsedGameTime.AsT0;
        Thread.Sleep(20);

        // When
        var result = _systemStopwatch.StartGame();

        // Then
        result.ShouldBeOfTypeOneOf<NotPossible>();
        var secondElapsed = _systemStopwatch.ElapsedGameTime.AsT0;
        secondElapsed.Should().BeGreaterThan(firstElapsed);
    }

    [Test]
    public void GivenStartedTimer_WhenElapsedQueried_ThenReturnsTimeSpan()
    {
        // Given
        _systemStopwatch.StartGame();
        Thread.Sleep(20);

        // When
        var result = _systemStopwatch.ElapsedGameTime;

        // Then
        result.ShouldBeOfTypeOneOf<TimeSpan>();
        result.AsT0.Should().BeGreaterThan(TimeSpan.Zero);
    }

    [Test]
    public void GivenStartedTimer_WhenStopGameCalled_ThenReturnsSuccess()
    {
        // Given
        _systemStopwatch.StartGame();
        Thread.Sleep(20);

        // When
        var result = _systemStopwatch.StopGame();

        // Then
        result.ShouldBeOfTypeOneOf<Success>();
    }

    [Test]
    public void GivenStoppedTimer_WhenElapsedReadMultipleTimes_ThenValueIsFrozen()
    {
        // Given
        _systemStopwatch.StartGame();
        Thread.Sleep(20);
        _systemStopwatch.StopGame();
        var first = _systemStopwatch.ElapsedGameTime.AsT0;

        // When
        Thread.Sleep(20);
        var second = _systemStopwatch.ElapsedGameTime.AsT0;

        // Then
        second.Should().Be(first);
    }

    [Test]
    public void GivenStoppedTimer_WhenStopGameCalledAgain_ThenReturnsNotPossibleAndDoesNotChangeElapsedTime()
    {
        // Given
        _systemStopwatch.StartGame();
        Thread.Sleep(20);
        _systemStopwatch.StopGame();
        var firstElapsed = _systemStopwatch.ElapsedGameTime.AsT0;

        // When
        var result = _systemStopwatch.StopGame();

        // Then
        result.ShouldBeOfTypeOneOf<NotPossible>();
        var secondElapsed = _systemStopwatch.ElapsedGameTime.AsT0;
        secondElapsed.Should().Be(firstElapsed);
    }

    [Test]
    public void GivenStoppedTimer_WhenStartGameCalledAgain_ThenReturnsSuccessAndResetsElapsedTime()
    {
        // Given
        _systemStopwatch.StartGame();
        Thread.Sleep(100);
        var firstElapsed = _systemStopwatch.ElapsedGameTime.AsT0;
        _systemStopwatch.StopGame();

        // When
        var result = _systemStopwatch.StartGame();

        // Then
        result.ShouldBeOfTypeOneOf<Success>();
        var secondElapsed = _systemStopwatch.ElapsedGameTime.AsT0;
        secondElapsed.Should().BeLessThan(firstElapsed);
    }

    [Test]
    public void GivenRunningTimer_WhenElapsedReadTwice_ThenValueIncreases()
    {
        // Given
        _systemStopwatch.StartGame();
        Thread.Sleep(20);
        var first = _systemStopwatch.ElapsedGameTime.AsT0;

        // When
        Thread.Sleep(20);
        var second = _systemStopwatch.ElapsedGameTime.AsT0;

        // Then
        second.Should().BeGreaterThan(first);
    }
}