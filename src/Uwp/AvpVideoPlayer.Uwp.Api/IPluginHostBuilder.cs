namespace AvpVideoPlayer.Uwp.Api
{
    public interface IPluginHostBuilder<T>
    {
        IPluginHost<T> Build();
        IPluginHostBuilder<T> IncludePath(string path);
    }
}