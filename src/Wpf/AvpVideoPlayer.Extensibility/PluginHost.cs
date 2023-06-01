using AvpVideoPlayer.Api;
using System.Windows;

namespace AvpVideoPlayer.Extensibility;

public class PluginHost<T> : IPluginHost, IDisposable
{
    private readonly IEventHub _hostEventHub;
    private readonly IDisposable _hostEventSub;
    private readonly IEventHub _pluginEventHub;
    private readonly IDisposable _pluginEventSub;
    private readonly IEnumerable<T> _plugins;
    private bool _disposedValue;
    private readonly IList<Guid> _sentEvents = new List<Guid>();
    private readonly object _syncRoot = new ();

    internal PluginHost(IEventHub eventhub, IEventHub shadoweventhub, IEnumerable<T> plugins)
    {
        _hostEventHub = eventhub;
        _pluginEventHub = shadoweventhub;
        _plugins = plugins;

        _hostEventSub = _hostEventHub.Events.Subscribe(OnHostEvent);
        _pluginEventSub = _pluginEventHub.Events.Subscribe(OnPluginEvent);
    }

    private void OnHostEvent(EventBase e)
    {
        var copy = (EventBase)e.Clone();
        lock(_syncRoot)
            _sentEvents.Add(copy.Id);
        _pluginEventHub.Publish(copy);
    }

    private void OnPluginEvent(EventBase e)
    {
        lock (_syncRoot)
        {
            if (_sentEvents.Contains(e.Id))
            {
                _sentEvents.Remove(e.Id);
                return;
            }
        }
        var copy = (EventBase)e.Clone();
        try
        {
            _hostEventHub.Publish(copy);
        }catch (Exception ex) // plugins may crash
        {
            //
            MessageBox.Show($"Error executing plugin: {ex}");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _hostEventSub.Dispose();
                _pluginEventSub.Dispose();
                foreach (var plugin in _plugins)
                {
                    if (plugin is IDisposable id) id.Dispose();
                }
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
