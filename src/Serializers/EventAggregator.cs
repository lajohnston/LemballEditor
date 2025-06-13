using System;
using System.Collections.Generic;

public class EventAggregator : IEventAggregator
{
    private readonly Dictionary<Type, List<Delegate>> subscribers = new Dictionary<Type, List<Delegate>>();

    public void Subscribe<T>(Action<T> handler)
    {
        if (!this.subscribers.TryGetValue(typeof(T), out var handlers))
        {
            handlers = new List<Delegate>();
            this.subscribers[typeof(T)] = handlers;
        }

        handlers.Add(handler);
    }

    public void Publish<T>(T eventData)
    {
        if (this.subscribers.TryGetValue(typeof(T), out var handlers))
        {
            foreach (var handler in handlers)
            {
                ((Action<T>)handler)(eventData);
            }
        }
    }

    public void Clear()
    {
        this.subscribers.Clear();
    }
}