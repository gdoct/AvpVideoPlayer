using AvpVideoPlayer.Api;
using System.IO;

namespace AvpVideoPlayer.ViewModels.IO;

public class PlayListViewModel : MultipleFileViewModel
{

    public PlayListViewModel(string path) : base(path, new FileInfo(path).Name, FileTypes.Playlist)
    {
    }
}