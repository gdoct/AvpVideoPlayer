namespace AvpVideoPlayer.EventHub.Tests;

using AvpVideoPlayer.Api;
using System;
using System.Reactive.Linq;
using Xunit;
using EventHubImpl = EventHub;

public partial class EventHubTests : IDisposable
{
    private readonly EventHubImpl _eventHub;
    private bool disposedValue;

    public EventHubTests()
    {
        _eventHub = new EventHubImpl();
    }

    [Fact]
    public void CanConstruct()
    {
        using var instance = new EventHubImpl();
        Assert.NotNull(instance);
    }

    [Fact]
    public void CanCallPublish()
    {
        var unitTestEvent = new UnitTestEvent();
        bool isHit = false;
        var callback = (UnitTestEvent e) =>
        {
            isHit = true;
        };
        using (_eventHub.Events.OfType<UnitTestEvent>().Subscribe(callback))
        {
            _eventHub.Publish(unitTestEvent);
            Assert.True(isHit);
        }
    }

    [Fact]
    public void CanProcessEvents()
    {
        var events = new EventBase[] {
                     new UnitTestEvent(),
                     new UnitTestEvent(),
                     new UnitTestEvent(),
        };
        bool isHit = false;
        int hitcount = 0;
        var callback = (EventBase e) =>
        {
            isHit = true;
            hitcount++;
        };

        using (_eventHub.Events.Subscribe(callback))
        {
            foreach (var appevent in events)
            {
                isHit = false;
                _eventHub.Publish(appevent);
                Assert.True(isHit);
            }
        }

        Assert.Equal(events.Length, hitcount);
    }

    [Fact]
    public void CannotCallPublishWithNullPublishedEvent()
    {
        UnitTestEvent? unitTestEvent = null;
        var callback = (UnitTestEvent e) =>
        {
        };
        using (_eventHub.Events.OfType<UnitTestEvent>().Subscribe(callback))
        {
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.Throws<ArgumentNullException>(() => _eventHub.Publish(unitTestEvent));
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }

    [Fact]
    public void ExceptionsInHandlersAreHandled()
    {
        UnitTestEvent unitTestEvent = new();
        var callback = (UnitTestEvent e) =>
        {
            throw new ArgumentException("bah");
        };
        using (_eventHub.Events.OfType<UnitTestEvent>().Subscribe(callback))
        {
            _eventHub.Publish(unitTestEvent);
        }
    }

    [Fact]
    public void CheckIsFullscreen()
    {
        var hit = false;
        UnitTestEvent unitTestEvent = new();
        var callback = (UnitTestEvent e) =>
        {
            hit = true;
        };
        using (_eventHub.Events.OfType<UnitTestEvent>().Subscribe(callback))
        {
            _eventHub.Publish(unitTestEvent);
        }
        Assert.True(hit);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _eventHub.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    [Fact]
    public void CanCloneEvents()
    {
        var evt = new UnitTestEvent();
        var clone = (UnitTestEvent)evt.Clone();
        Assert.Equal(evt.Id, clone.Id);
        Assert.False(evt.Equals(clone));
    }
    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~EventHubTests()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
