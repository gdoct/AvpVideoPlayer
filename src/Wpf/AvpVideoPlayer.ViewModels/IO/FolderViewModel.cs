using AvpVideoPlayer.Api;
using System.IO;

namespace AvpVideoPlayer.ViewModels.IO;

public class FolderViewModel : MultipleFileViewModel
{
    public FolderViewModel(string path) : this(path, new DirectoryInfo(path).Name)
    {
    }

    public FolderViewModel(string path, string name) : base(path, name, FileTypes.Folder)
    {
    }
}