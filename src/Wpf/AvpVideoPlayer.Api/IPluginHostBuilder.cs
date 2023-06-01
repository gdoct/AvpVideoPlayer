namespace AvpVideoPlayer.Api;
public interface IPluginHostBuilder<T>
{
    IPluginHost Build();
    IPluginHostBuilder<T> IncludePath(string path);
}