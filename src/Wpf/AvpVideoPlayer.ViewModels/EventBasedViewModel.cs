using AvpVideoPlayer.Api;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;

namespace AvpVideoPlayer.ViewModels;

public class EventBasedViewModel : BaseViewModel, IDisposable
{
    protected readonly IEventHub _eventHub;
    private readonly IList<IDisposable> _subscriptions = new List<IDisposable>();
    private bool disposedValue;

    public EventBasedViewModel(IEventHub eventhub)
    {
        _eventHub = eventhub ?? throw new ArgumentNullException(nameof(eventhub));
    }

    protected void AddSubscription(IDisposable disposable)
    {
        _subscriptions.Add(disposable);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                foreach (var sub in _subscriptions)
                    sub.Dispose();
                _eventHub.Dispose();
            }

            disposedValue = true;
        }
    }
    protected void Publish(EventBase @event)
    {
        _eventHub.Publish(@event);
    }

    protected void Subscribe<T>(Action<T> handler)
    {
        var events = _eventHub.Events.OfType<T>();
        if (Application.Current?.Dispatcher != null && System.Threading.SynchronizationContext.Current != null)
        {
            events = events.ObserveOn(System.Threading.SynchronizationContext.Current);
        }
        _subscriptions.Add(events.Subscribe(handler));
    }

    protected IObservable<T> EventsOfType<T>()
    {
        var events = _eventHub.Events.OfType<T>();
        if (Application.Current?.Dispatcher != null && System.Threading.SynchronizationContext.Current != null)
        {
            return events.ObserveOn(System.Threading.SynchronizationContext.Current);
        }
        else return events;
    }
}
