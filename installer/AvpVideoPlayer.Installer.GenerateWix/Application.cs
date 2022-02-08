namespace AvpVideoPlayer.Installer.GenerateWix;

public static class Application
{
    public static string ExePath { get; } = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName ?? throw new Exception("runtime error");
}
