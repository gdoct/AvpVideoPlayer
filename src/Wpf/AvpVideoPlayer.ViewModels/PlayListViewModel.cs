using AvpVideoPlayer.Api;
using System.IO;

namespace AvpVideoPlayer.ViewModels;

public class PlayListViewModel : FileViewModel
{
    private bool _isSelected;

    public PlayListViewModel(string path) : this(path, new FileInfo(path).Name)
    {
    }

    public PlayListViewModel(string path, string name) : base(FileTypes.Playlist)
    {
        Path = path;
        Name = name;
    }

    public bool IsSelected
    {
        get => _isSelected; set
        {
            SetProperty(ref _isSelected, value);
            RaisePropertyChanged(nameof(Icon));
        }
    }

    public DirectoryInfo Directory
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Path)) return DriveInfo.GetDrives()[0].RootDirectory;
            return new DirectoryInfo(Path) ?? throw new System.NullReferenceException(nameof(Path));
        }
    }

    public int Indent
    {
        get
        {
            return ExtractIndent();
        }
    }

    private int ExtractIndent()
    {
        int indent = 0;
        if (string.IsNullOrWhiteSpace(Path)) return indent;
        var firstslash = Path.IndexOf(System.IO.Path.DirectorySeparatorChar);
        if (firstslash == -1) return indent;

        var slash = Path.IndexOf(System.IO.Path.DirectorySeparatorChar, firstslash);
        while (slash > 0)
        {
            indent++;
            slash = Path.IndexOf(System.IO.Path.DirectorySeparatorChar, slash + 1);
        }
        return indent * 10;
    }

    public string Icon
    {
        get
        {
            var path = this.Directory;
            if (path == null)
                return "/Images/harddisk.png";
            if (path.Root.FullName == path.FullName)
                return "/Images/harddisk.png";
            if (IsSelected)
                return "/Images/openfolderyellow.png";
            return "/Images/openfolderyellow.png";
        }
    }

}