using AvpVideoPlayer.Api;
using System.Reactive.Subjects;

namespace AvpVideoPlayer.EventHub;

/// <inheritdoc cref="IEventHub"/>
public class EventHub : IDisposable, IEventHub
{
    private bool _disposedValue;
    private readonly Subject<EventBase> _eventSource;

    /// <summary>
    /// Create a new instance of a <see cref="EventHub"/>
    /// </summary>
    public EventHub()
    {
        _eventSource = new();
    }

    /// <inheritdoc cref="IEventHub.Events"/>
    public IObservable<EventBase> Events => _eventSource;

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _eventSource.Dispose();
            }

            _disposedValue = true;
        }
    }

    /// <inheritdoc cref="IEventHub.Publish(EventBase)"/>
    public void Publish(EventBase publishedEvent)
    {
        try
        {
            if (publishedEvent == null)
            {
                throw new ArgumentNullException(nameof(publishedEvent));
            }
            Log(publishedEvent.ToString());

            _eventSource.OnNext(publishedEvent);
        }
        catch (Exception exception)
        {
            _eventSource.OnError(exception);
        }
    }

    static void Log(string s)
    {
        System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:T}] {s}");
    }
}
