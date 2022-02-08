namespace AvpVideoPlayer.Api;

/// <summary>
/// A simple event bus based on System.Reactive
/// </summary>
public interface IEventHub : IDisposable
{
    /// <summary>
    /// The collection of application-wide events to subscribe to
    /// </summary>
    IObservable<EventBase> Events { get; }

    /// <summary>
    /// Publish an application wide event
    /// </summary>
    /// <param name="publishedEvent">the event to publish</param>
    void Publish(EventBase publishedEvent);
}
