namespace AvpVideoPlayer.Installer.GenerateWix;

class Program
{
    /// <summary>
    /// this program generates a Product.wxs file for the installer
    /// </summary>
    /// <exception cref="FileNotFoundException"></exception>
    public static void Main()
    {
        var settings = Settings.Load();
        if (Environment.GetCommandLineArgs().Length > 1)
        {
            settings.SourceFolder = Environment.GetCommandLineArgs()[1];
        }
        var generator = new WixProductGenerator(settings);
        var parser = new WixSourceParser(settings);
        var foldercontent = parser.ParseSourceFolder();
        generator.Generate(foldercontent);
    }
}