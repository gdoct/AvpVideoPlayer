using System.Text;
using System.Xml.Serialization;

namespace AvpVideoPlayer.Installer.GenerateWix;

/// <summary>
/// this class converts a directory tree (in _settings.SourceFolder) into a wix structure
/// so we can publish the application and generate the installer and don't worry about the files
/// </summary>
public class WixSourceParser
{
    const string INSTALL_FOLDER_VAR = "INSTALLFOLDER";
    const string PRODUCT_COMPONENTS = "ProductComponents";

    private readonly Settings _settings;

    private readonly IDictionary<string, DirectoryInfo> _folders = new Dictionary<string, DirectoryInfo>();
    private readonly string _existing;

    public WixSourceParser(Settings settings)
    {
        _settings = settings;
        _existing = !string.IsNullOrWhiteSpace(settings.OutputFile) && File.Exists(settings.OutputFile)
            ? File.ReadAllText(settings.OutputFile)
            : string.Empty;

    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wix/2006/wi")]
    public class WixDocumentStub
    {
        public Fragment[]? Fragments { get; set; } 
        public WixProductFeature? Feature { get; set; }
    }

    /// <summary>
    /// Parses the provided source folder and returns xml fragments for the detected contents
    /// </summary>
    /// <returns>a tuple of xml fragments 
    /// the first part contains the feature section
    /// the second part contains the list of fragments for files and folder section
    /// </returns>
    public InstallerContent ParseSourceFolder()
    {
        var folders = GenerateFolderList();
        var files = GenerateFileList().ToList();
        var combinedList = new List<Fragment>
        {
            folders
        };
        combinedList.AddRange(files);
        var combinedArray = combinedList.ToArray();
        var componentgroupref = 
            _folders.Keys.Select(k => 
                new WixProductFeatureComponentGroupRef 
                { 
                    Id = (k== INSTALL_FOLDER_VAR) ? PRODUCT_COMPONENTS : $"{k}_files" 
                }
            ).ToArray();
        var feature = new WixProductFeature
        {
            Id = "ProductFeature",
            Title = _settings.ProductName,
            Level = "1",
            ComponentGroupRef = componentgroupref,
        };
        WixDocumentStub fileswix = new()
        {
            Fragments = combinedArray,
            Feature = feature
        };
        var data = ConvertXmlToString(fileswix);
        var featurepart = ExtractSubstring(data, "<Feature", "</Feature>", false);
        featurepart = featurepart[(featurepart.IndexOf(">")+1)..];
        var filepart = ExtractSubstring(data, "<Fragment>", "</Fragments>", false);
        return new (featurepart, filepart);
    }

    private string ExtractSubstring(string data, string start, string end, bool includeEnd)
    {
        var startindex = data.IndexOf(start);
        if (startindex < 0) return string.Empty;
        var endindex = data.LastIndexOf(end);
        if (endindex < 0) return data[startindex..];

        if (includeEnd) endindex += end.Length;
        return data[startindex..endindex];
    }

    private static string ConvertXmlToString(WixDocumentStub document)
    {
        StringBuilder sb = new();
        using (var tw = new StringWriter(sb))
            new XmlSerializer(typeof(WixDocumentStub)).Serialize(tw, document);
        return sb.ToString();
    }

    private IEnumerable<Fragment> GenerateFileList()
    {
        foreach (var folder in _folders.Keys)
        {
            var fragment = new Fragment();
            var path = _folders[folder];
            var folderkey = (folder == INSTALL_FOLDER_VAR) ? PRODUCT_COMPONENTS : $"{folder}_files";
            var filekey = (folder == INSTALL_FOLDER_VAR) ? "" : $"{folder}_";
            var filesInFolder = path.GetFiles();
            var componentgroup = new WixFragmentComponentGroup()
            {
                Id = folderkey,
                Directory = (folderkey == PRODUCT_COMPONENTS) ? INSTALL_FOLDER_VAR : folder,
                Component = new WixFragmentComponentGroupComponent[filesInFolder.Length]
            };
            var relativepath = GetRelativePath(path.FullName, _settings.SourceFolder);
            var search = $@"<ComponentGroup Id=""{folderkey}"" ";
            var existing_idx = _existing.IndexOf(search);
            for (int i = 0; i < filesInFolder.Length; i++)
            {
                FileInfo file = filesInFolder[i];
                var file_key = $"{filekey}{file.Name}";
                var guid = FindFileGuid(existing_idx, file_key);
                var component = new WixFragmentComponentGroupComponent 
                {
                    Id=file_key, 
                    Guid = guid.ToString(),
                    File = new WixFragmentComponentGroupComponentFile[1]
                    {
                        new WixFragmentComponentGroupComponentFile
                        {
                            Id=file_key,
                            Name = file.Name,
                            Source = $"$(var.RootDir){relativepath}{file.Name}"
                        }
                    }
                };
                componentgroup.Component[i] = component;
            }
            fragment.ComponentGroup = new WixFragmentComponentGroup[1] { componentgroup };
            yield return fragment;
        }
    }

    private Guid FindFileGuid(int start_existing_idx, string name)
    {
        /*
<ComponentGroup Id="ProductComponents" Directory="INSTALLFOLDER">
<Component Id="app.ico" Guid="b29bf796-207a-4718-8259-6ab2bbc5e971">
<File Id="app.ico" Name="app.ico" Source="$(var.RootDir)app.ico" />
</Component>
</ComponentGroup
         */
        if (start_existing_idx < 0) return Guid.NewGuid();
        var until = _existing.IndexOf("</ComponentGroup", start_existing_idx);
        if (until < 0) return Guid.NewGuid();

        var sub = _existing[start_existing_idx..until];
        var pattern = @$"<Component Id=""{name}"" Guid=""";
        var fileidx = sub.IndexOf(pattern);
        if (fileidx < 0) return Guid.NewGuid();
        var guid_proto = sub.Substring(fileidx + pattern.Length, 36);
        if (Guid.TryParse(guid_proto, out Guid guid)) return guid;
        return Guid.NewGuid();

    }

    string GetRelativePath(string path, string root)
    {
        if (string.Compare(root, path, StringComparison.OrdinalIgnoreCase) == 0) return string.Empty;
        var result = path[root.Length..];
        if (result.Length > 0)
        {
            if (result.StartsWith(@"\")) 
                result = result[1..];
            if (!result.EndsWith(@"\"))
                result += @"\";
        }
        return result;
    }

    private Fragment GenerateFolderList() 
    {
        _folders.Clear();
        var dir = GetFolder(_settings.SourceFolder);
        var folders = new Directory() 
        { 
            Id = "TARGETDIR", 
            Name = "SourceDir",
            Directory1 = new Directory[2]
            {
                new Directory()
                { 
                    Id="ProgramMenuFolder",
                    Directory1 = new Directory[1]
                    {
                        new Directory
                        {
                            Id="ApplicationProgramsFolder",
                            Name=_settings.ProductName
                        }
                    }
                },
                new Directory()
                {
                    Id = "ProgramFilesFolder",
                    Directory1 = new Directory[1]
                    {
                        dir
                    }
                }
            }
        };
        var fragment = new Fragment
        {
            Directory = new Directory[1] { folders },
            Property = new WixFragmentProperty[1]
            {
                new WixFragmentProperty
                {
                    Id = "WIXUI_INSTALLDIR",
                    Value = INSTALL_FOLDER_VAR
                }
            }
        };
        return fragment;
    }

    private Directory GetFolder(string path, string prefix = INSTALL_FOLDER_VAR)
    {
        var dirinfo = new DirectoryInfo(path);
        bool isRootFolder = (prefix == INSTALL_FOLDER_VAR);
        var result = new Directory { Id = $"{prefix}", Name = isRootFolder ? _settings.ProductName: dirinfo.Name };
        _folders.Add(prefix, dirinfo);
        var children = dirinfo.GetDirectories();
        result.Directory1 = new Directory[children.Length];
        for (int num = 0; num < children.Length; num++)
        {
            DirectoryInfo? child = children[num];
            if (child != null)
            {
                var newprefix = isRootFolder ? child.Name.Replace("-", "_") : $"{prefix}_{child.Name.Replace("-", "_")}";
                var subdir = GetFolder(child.FullName, newprefix);
                result.Directory1[num] = subdir;
            }
        }
        return result;
    }
}