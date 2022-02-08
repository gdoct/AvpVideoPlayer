using System.Text.Json;

namespace AvpVideoPlayer.Installer.GenerateWix;

public class Settings
{
    private string _sourceFolder = "";
    private string _outputFile = "";

    const string SettingsFileName = "settings.json";
    public static Settings Load()
    {
        string settingsFullPath = Path.Combine(Application.ExePath, SettingsFileName);
        if (!File.Exists(settingsFullPath)) throw new FileNotFoundException($"Settings file '{settingsFullPath}' does not exist.");
        var file = File.ReadAllText(settingsFullPath);
        if (string.IsNullOrWhiteSpace(file)) throw new InvalidDataException($"Settings file '{settingsFullPath}' is empty.");
        return JsonSerializer.Deserialize<Settings>(file) ?? throw new InvalidDataException($"Settings file '{settingsFullPath}' contains incorrect data.");
    }

    public string SourceFolder
    {
        get
        {
            if (string.IsNullOrEmpty(_sourceFolder)) return "";
            if (Path.IsPathFullyQualified(_sourceFolder)) return _sourceFolder;
            return Path.GetFullPath(Path.Combine(Application.ExePath, _sourceFolder));
        }
        set
        {
            _sourceFolder = value;
        }
    }
    public string ProductName { get; set; } = "";
    public string ProductDescription { get; set; } = "";
    public string Version { get; set; } = "";
    public string MainExecutable { get; set; } = "";
    public string Manufacturer { get; set; } = "";
    public string RegistryPath { get; set; } = "";

    public string OutputFile 
    { 
        get
        {
            if (string.IsNullOrEmpty(_outputFile)) return "";
            if (Path.IsPathFullyQualified(_outputFile)) return _outputFile;
            return Path.GetFullPath(Path.Combine(Application.ExePath, _outputFile));
        }
        set => _outputFile = value; 
    }
    public string Template { get; set; } = "";
}
