namespace ThreeInARow.Infrastructure.ADT;

public interface IEventBus
{
    void Publish<TEvent>(TEvent @event) where TEvent : IEvent;

    void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;
}