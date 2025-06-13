using System;

public interface IEventAggregator
{
    void Clear();
    void Publish<T>(T eventData);
    void Subscribe<T>(Action<T> handler);
}