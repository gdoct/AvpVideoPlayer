using AvpVideoPlayer.Api;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AvpVideoPlayer.Extensibility;

public class PluginHostBuilder<T> : IPluginHostBuilder<T>
{
    private readonly IList<string> _paths = new List<string>();
    private readonly IEventHub _eventhub;

    public PluginHostBuilder(IEventHub eventhub)
    {
        _eventhub = eventhub;
    }

    public IPluginHostBuilder<T> IncludePath(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException(path);
        }

        _paths.Add(path);
        return this;
    }
    public IPluginHost Build()
    {
        var pluginmetas = ScanPluginFolders().ToList();
        IEventHub shadoweventhub = new EventHub.EventHub();
        var plugins = InitializePlugins(pluginmetas, shadoweventhub);
        return new PluginHost<T>(_eventhub, shadoweventhub, plugins);
    }

    private static IEnumerable<T> InitializePlugins(IEnumerable<PluginMeta> pluginmetas, IEventHub eh) =>
        // create instances and inject the event hub
        pluginmetas.Select(plugin => plugin.Type.GetConstructor(new[] { typeof(IEventHub) }))
                   .Where(ctor => ctor != null)
                   .Select(ctor => (T)ctor!.Invoke(new[] { eh }));

    private IEnumerable<PluginMeta> ScanPluginFolders() =>
        // scan all dlls for public implementations of IPlugin
        _paths.SelectMany(path => ScanAssemblies(LoadAssembliesInFolder(path)));

    private static IEnumerable<PluginMeta> ScanAssemblies(IEnumerable<Assembly> assembly) => 
        assembly.SelectMany(a => a.DefinedTypes)
                .Where(x => x != typeof(T))
                .Where(x => typeof(T).IsAssignableFrom(x))
                .Select(x => new PluginMeta(x.Assembly.Location, x));

    private static IEnumerable<Assembly> LoadAssembliesInFolder(string folder)
    {
        IList<Assembly> result = new List<Assembly>();
        foreach (var file in Directory.GetFiles(folder, "*.dll"))
        {
            try
            {
                var ass = Assembly.LoadFile(file);
                result.Add(ass);
            }
            catch {
                Debug.WriteLine("OOOOOOOPS");
            }
        }
        return result;
    }

    private sealed class PluginMeta
    {
        public PluginMeta(string path, Type t)
        {
            Path = path;
            Type = t;
        }
        public string Path { get; }
        public Type Type { get; }
    }
}
