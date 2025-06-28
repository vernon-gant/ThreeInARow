using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Infrastructure.Implementation;

public class DefaultEventBus : IEventBus
{
    private readonly Dictionary<Type, List<object>> _handlers = new();

    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        if (!_handlers.TryGetValue(typeof(TEvent), out var handlers)) return;

        foreach (var handler in handlers.Cast<IEventHandler<TEvent>>())
        {
            handler.Handle(@event);
        }
    }

    public void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
    {
        if (_handlers.ContainsKey(typeof (TEvent)))
            _handlers[typeof(TEvent)].Add(handler);
        else
            _handlers[typeof(TEvent)] = [handler];
    }
}