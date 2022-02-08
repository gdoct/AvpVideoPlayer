namespace AvpVideoPlayer.Uwp.Api {

/// <summary>
/// Enables logging application events
/// </summary>
public interface IEventLogger
{
    /// <summary>
    /// Dump an event to debug log
    /// </summary>
    /// <param name="publishedEvent">The event to dump</param>
    void DumpEvent(EventBase publishedEvent);
}
}