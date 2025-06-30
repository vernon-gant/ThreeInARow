using FluentAssertions;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.Implementation;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Infrastructure.Tests;

[TestFixture]
public class SystemStopwatchTests
{
    private SystemStopwatch _systemStopwatch;

    [SetUp]
    public void Setup()
    {
        _systemStopwatch = new SystemStopwatch();
    }

    [Test]
    public void GivenNotStartedStopwatch_WhenStateIsQueried_ThenDefaultsAreCorrect()
    {
        // Given / When

        // Then
        _systemStopwatch.IsRunning.Should().BeFalse();
        _systemStopwatch.FinishedFullCycle.Should().BeFalse("no start/stop cycle has ever completed");
        _systemStopwatch.Elapsed.ShouldBeOfTypeOneOf<HasNotStartedYet>();
        _systemStopwatch.Stop().ShouldBeOfTypeOneOf<HasNotStartedYet>();
    }

    [Test]
    public void GivenStartedStopwatch_WhenStateIsQueried_ThenRunningAndNoFullCycle()
    {
        // Given
        _systemStopwatch.Start();

        // When / Then
        _systemStopwatch.IsRunning.Should().BeTrue();
        _systemStopwatch.FinishedFullCycle.Should().BeFalse("we haven’t stopped yet");
        _systemStopwatch.Elapsed.ShouldBeOfTypeOneOf<TimeSpan>();
    }

    [Test]
    public void GivenStartedStopwatch_WhenStartIsCalledAgain_ThenReportsAlreadyRunningAndDoesNotReset()
    {
        // Given
        _systemStopwatch.Start();
        Thread.Sleep(250);
        var initial = _systemStopwatch.Elapsed.AsT0;

        // When
        var result = _systemStopwatch.Start();

        // Then
        result.ShouldBeOfTypeOneOf<AlreadyRunning>();
        _systemStopwatch.IsRunning.Should().BeTrue();
        _systemStopwatch.FinishedFullCycle.Should().BeFalse("we still haven’t completed a stop");
        _systemStopwatch.Elapsed.AsT0.Should().BeGreaterThan(initial);
    }

    [Test]
    public void GivenStartedStopwatch_WhenStopIsCalled_ThenStopsAndMarksFullCycle()
    {
        // Given
        _systemStopwatch.Start();

        // When
        var result = _systemStopwatch.Stop();

        // Then
        result.ShouldBeOfTypeOneOf<Success>();
        _systemStopwatch.IsRunning.Should().BeFalse();
        _systemStopwatch.FinishedFullCycle.Should().BeTrue("we just completed a full start/stop cycle");
    }

    [Test]
    public void GivenStoppedStopwatch_WhenElapsedIsReadMultipleTimes_ThenValueStaysConstant()
    {
        // Given
        _systemStopwatch.Start();
        Thread.Sleep(100);
        _systemStopwatch.Stop();

        // When / Then
        _systemStopwatch.Elapsed.ShouldBeOfTypeOneOf<TimeSpan>();
        var first = _systemStopwatch.Elapsed.AsT0;

        _systemStopwatch.Elapsed.ShouldBeOfTypeOneOf<TimeSpan>();
        var second = _systemStopwatch.Elapsed.AsT0;

        first.Should().Be(second);
        _systemStopwatch.FinishedFullCycle.Should().BeTrue("once stopped it stays in full-cycle state until Reset()");
    }
}