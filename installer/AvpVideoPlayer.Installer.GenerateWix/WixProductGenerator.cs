using System.Xml;

namespace AvpVideoPlayer.Installer.GenerateWix;

internal class WixProductGenerator
{
    private readonly Settings _settings;

    public WixProductGenerator(Settings settings)
    {
        _settings = settings;
    }

    public void Generate(InstallerContent folderContents)
    {
        var templatepath = (Path.IsPathFullyQualified(_settings.Template)) ? _settings.Template : Path.Combine(Application.ExePath, _settings.Template);
        if (!File.Exists(templatepath)) throw new FileNotFoundException(nameof(_settings.Template));
        if (File.Exists(_settings.OutputFile)) File.Delete(_settings.OutputFile);
        var sourcefolder = _settings.SourceFolder;
        if (!sourcefolder.EndsWith(@"\")) sourcefolder += @"\";
        var template = File.ReadAllText(templatepath);
        var result = template.Replace("{rootdir}", sourcefolder)
            .Replace("{productname}", _settings.ProductName)
            .Replace("{productdescription}", _settings.ProductDescription)
            .Replace("{version}", _settings.Version)
            .Replace("{manufacturer}", _settings.Manufacturer)
            .Replace("<!-- productfeature -->", folderContents.FeatureSection)
            .Replace("{registrypath}", _settings.RegistryPath)
            .Replace("{mainexecutable}", _settings.MainExecutable)
            .Replace("<!-- productcontents -->", folderContents.ContentsSection);

        var xml = new XmlDocument();
        xml.LoadXml(result);
        xml.Save(_settings.OutputFile);
    }
}
