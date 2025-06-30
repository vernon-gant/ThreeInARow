using FluentAssertions;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.Implementation;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Infrastructure.Tests;

[TestFixture]
public class SystemOneShotStopwatchTests
{
    private SystemOneShotStopwatch _systemOneShotStopwatch;

    [SetUp]
    public void Setup() => _systemOneShotStopwatch = new SystemOneShotStopwatch();

    [Test]
    public void GivenNotStartedStopwatch_WhenStateIsQueried_ThenDefaultsAreCorrect()
    {
        // Given / When

        // Then
        _systemOneShotStopwatch.IsRunning.Should().BeFalse();
        _systemOneShotStopwatch.FinishedFullCycle.Should().BeFalse();
        _systemOneShotStopwatch.Elapsed.ShouldBeOfTypeOneOf<NeverStarted>();
        _systemOneShotStopwatch.Stop().ShouldBeOfTypeOneOf<NeverStarted>();
    }

    [Test]
    public void GivenNotStartedStopwatch_WhenStartIsCalled_ThenReturnsSuccessAndIsRunningAndNoFullCycle()
    {
        // When
        var result = _systemOneShotStopwatch.Start();

        // Then
        result.ShouldBeOfTypeOneOf<Success>();
        _systemOneShotStopwatch.IsRunning.Should().BeTrue();
        _systemOneShotStopwatch.FinishedFullCycle.Should().BeFalse();
    }

    [Test]
    public void GivenStartedStopwatch_WhenStateIsQueried_ThenRunningAndNoFullCycle()
    {
        // Given
        _systemOneShotStopwatch.Start();

        // When / Then
        _systemOneShotStopwatch.IsRunning.Should().BeTrue();
        _systemOneShotStopwatch.FinishedFullCycle.Should().BeFalse();
        _systemOneShotStopwatch.Elapsed.ShouldBeOfTypeOneOf<TimeSpan>();
    }

    [Test]
    public void GivenStartedStopwatch_WhenStartIsCalledAgain_ThenReportsAlreadyRunningAndElapsedContinuesAndNoFullCycle()
    {
        // Given
        _systemOneShotStopwatch.Start();
        Thread.Sleep(50);
        var initial = _systemOneShotStopwatch.Elapsed.AsT0;

        // When
        var result = _systemOneShotStopwatch.Start();

        // Then
        result.ShouldBeOfTypeOneOf<AlreadyRunning>();
        _systemOneShotStopwatch.IsRunning.Should().BeTrue();
        _systemOneShotStopwatch.FinishedFullCycle.Should().BeFalse();
        _systemOneShotStopwatch.Elapsed.AsT0.Should().BeGreaterThan(initial);
    }

    [Test]
    public void GivenStartedStopwatch_WhenStopIsCalled_ThenReturnsSuccessAndStopsAndMarksFullCycle()
    {
        // Given
        _systemOneShotStopwatch.Start();

        // When
        var result = _systemOneShotStopwatch.Stop();

        // Then
        result.ShouldBeOfTypeOneOf<Success>();
        _systemOneShotStopwatch.IsRunning.Should().BeFalse();
        _systemOneShotStopwatch.FinishedFullCycle.Should().BeTrue();
    }

    [Test]
    public void GivenStoppedStopwatch_WhenElapsedIsReadMultipleTimes_ThenValueStaysConstantAndFullCycleStaysTrue()
    {
        // Given
        _systemOneShotStopwatch.Start();
        Thread.Sleep(50);
        _systemOneShotStopwatch.Stop();

        // When / Then
        _systemOneShotStopwatch.Elapsed.ShouldBeOfTypeOneOf<TimeSpan>();
        var first = _systemOneShotStopwatch.Elapsed.AsT0;
        _systemOneShotStopwatch.Elapsed.ShouldBeOfTypeOneOf<TimeSpan>();
        var second = _systemOneShotStopwatch.Elapsed.AsT0;

        first.Should().Be(second);
        _systemOneShotStopwatch.FinishedFullCycle.Should().BeTrue();
    }

    [Test]
    public void GivenStoppedStopwatch_WhenStopIsCalledAgain_ThenReportsNeverStartedAndElapsedUnchangedAndFullCycleStaysTrue()
    {
        // Given
        _systemOneShotStopwatch.Start();
        _systemOneShotStopwatch.Stop();
        var frozen = _systemOneShotStopwatch.Elapsed.AsT0;

        // When
        var result = _systemOneShotStopwatch.Stop();

        // Then
        result.ShouldBeOfTypeOneOf<NeverStarted>();
        _systemOneShotStopwatch.IsRunning.Should().BeFalse();
        _systemOneShotStopwatch.FinishedFullCycle.Should().BeTrue();
        _systemOneShotStopwatch.Elapsed.AsT0.Should().Be(frozen);
    }

    [Test]
    public void GivenRunningStopwatch_WhenElapsedReadTwice_ThenValueIncreasesAndNoFullCycle()
    {
        // Given
        _systemOneShotStopwatch.Start();
        Thread.Sleep(50);

        // When
        var first  = _systemOneShotStopwatch.Elapsed.AsT0;
        Thread.Sleep(50);
        var second = _systemOneShotStopwatch.Elapsed.AsT0;

        // Then
        second.Should().BeGreaterThan(first);
        _systemOneShotStopwatch.FinishedFullCycle.Should().BeFalse();
    }

    [Test]
    public void GivenStartedStopwatch_WhenStopCalledThenElapsedRead_ThenElapsedGreaterThanZeroAndFullCycleTrue()
    {
        // Given
        _systemOneShotStopwatch.Start();
        Thread.Sleep(50);

        // When
        _systemOneShotStopwatch.Stop();

        // Then
        _systemOneShotStopwatch.Elapsed.AsT0.Should().BeGreaterThan(TimeSpan.Zero);
        _systemOneShotStopwatch.FinishedFullCycle.Should().BeTrue();
    }
}