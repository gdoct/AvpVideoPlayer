using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.Plugins.WebHost;

public class WebHostPlugin : IAvpPlugin
{
    private readonly IEventHub _eventhub;

    public WebHostPlugin(IEventHub eventHub)
    {
        _eventhub = eventHub;
        _eventhub.Events.Subscribe(OnHostEvent);
    }

    private void OnHostEvent(EventBase e)
    {
        System.Diagnostics.Debug.WriteLine($"PLUGIN {e}");
    }
}
