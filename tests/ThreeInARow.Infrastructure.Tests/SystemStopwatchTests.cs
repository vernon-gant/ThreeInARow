using FluentAssertions;
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
    public void GivenNotStartedStopwatch_WhenTimeRelatedQueriesAreCalled_ThenNotRunningIsReturned()
    {
        // Given
        // When
        // Then
        _systemStopwatch.IsRunning.Should().BeFalse();
        _systemStopwatch.Elapsed.ShouldBeOfTypeOneOf<IsNotRunning>();
    }
}