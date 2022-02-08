namespace AvpVideoPlayer.ViewModels.Tests;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels.Events;
using Moq;
using System;
using Xunit;

public class EventBasedViewModelTests
{
    private readonly IEventHub _eventhub;

    public EventBasedViewModelTests()
    {
        _eventhub = Mock.Of<IEventHub>();
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new EventBasedViewModel(_eventhub);
        Assert.NotNull(instance);
    }

    [Fact]
    public void CannotConstructWithNullEventhub()
    {
        Assert.Throws<ArgumentNullException>(() => new EventBasedViewModel(default));
    }

    public class EventBasedViewModelImpl : EventBasedViewModel
    {
        public EventBasedViewModelImpl(IEventHub eh) : base(eh)
        {

        }
        public void AddSub<T>(Action<T> action)
        {
            base.Subscribe(action);
        }

        public void PublishEvent(EventBase e) => base.Publish(e);
        public void AddSub(IDisposable d) => base.AddSubscription(d);
    }

    [Fact]
    public void CanSubscribe()
    {
        var eh = new Mock<IEventHub>();
        var disp = new Mock<IDisposable>();
        disp.Setup(d => d.Dispose());
        eh.Setup(e => e.Events.Subscribe(It.IsAny<IObserver<EventBase>>())).Returns(disp.Object);
        using (var instance = new EventBasedViewModelImpl(eh.Object))
        {
            instance.AddSub<FullScreenEvent>((_) => { });
            eh.Verify();
        }
        disp.Verify();
    }

    [Fact]
    public void CanPublishe()
    {
        var eh = new Mock<IEventHub>();
        eh.Setup(e => e.Publish(It.IsAny<EventBase>()));
        using var instance = new EventBasedViewModelImpl(eh.Object);
        instance.PublishEvent(new FullScreenEvent(true));
        eh.Verify();
    }

    [Fact]
    public void CanAddSub()
    {
        var eh = new Mock<IEventHub>();
        var disp = new Mock<IDisposable>();
        disp.Setup(d => d.Dispose());
        using (var instance = new EventBasedViewModelImpl(eh.Object))
        {
            instance.AddSub(disp.Object);
        }
        eh.Verify();
        disp.Verify();
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
