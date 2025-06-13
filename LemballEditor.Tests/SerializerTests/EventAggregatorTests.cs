using System;
using FluentAssertions;

[TestClass]
public class EventAggregatorTests
{
    private EventAggregator aggregator;

    [TestInitialize]
    public void Setup()
    {
        aggregator = new EventAggregator();
    }

    [TestMethod]
    public void SubscribeAndPublish_ShouldInvokeHandler()
    {
        int received = 0;
        aggregator.Subscribe<int>(x => received = x);

        aggregator.Publish(42);

        received.Should().Be(42);
    }

    [TestMethod]
    public void PublishWithoutSubscribers_ShouldNotThrow()
    {
        var act = () => aggregator.Publish("no subscribers");
        act.Should().NotThrow();
    }

    [TestMethod]
    public void MultipleSubscribers_ShouldAllBeInvoked()
    {
        int callCount = 0;
        aggregator.Subscribe<string>(s => callCount++);
        aggregator.Subscribe<string>(s => callCount++);

        aggregator.Publish("test");

        callCount.Should().Be(2);
    }

    [TestMethod]
    public void Clear_ShouldRemoveAllSubscribers()
    {
        bool called = false;
        aggregator.Subscribe<int>(x => called = true);

        aggregator.Clear();
        aggregator.Publish(1);

        Assert.IsFalse(called);
    }

    [TestMethod]
    public void SubscribeAndPublish_ShouldSeparateDifferentTypes()
    {
        int intValue = 0;
        string stringValue = null;
        aggregator.Subscribe<int>(x => intValue = x);
        aggregator.Subscribe<string>(s => stringValue = s);

        aggregator.Publish(5);
        aggregator.Publish("hello");

        intValue.Should().Be(5);
        stringValue.Should().Be("hello");
    }
}
