using NSubstitute;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.Implementation;

namespace ThreeInARow.Infrastructure.Tests;

public class TestEvent : IEvent;

[TestFixture]
public class DefaultEventBusTests
{
    private DefaultEventBus _eventBus;

    [SetUp]
    public void Setup()
    {
        _eventBus = new DefaultEventBus();
    }

    [Test]
    public void GivenBusWithOneRegisteredHandler_WhenPublishingEventForThisHandler_ThenHandlerIsInvoked()
    {
        // Given
        var handler = Substitute.For<IEventHandler<TestEvent>>();
        _eventBus.Register(handler);

        // When
        var @event = new TestEvent();
        _eventBus.Publish(@event);

        // Then
        handler.Received(1).Handle(@event);
    }
}