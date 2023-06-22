using AvpVideoPlayer.Api;
using AvpVideoPlayer.Utility;
using System.IO;

namespace AvpVideoPlayer.ViewModels.IO;

public class VideoFileViewModel : FileViewModel
{
    public VideoFileViewModel(FileInfo file) : base(GetFileType(file))
    {
        Path = file.FullName;
        Name = file.Name;
    }

    private static FileTypes GetFileType(FileInfo file)
    {
        if (FileExtensions.IsSubtitleFile(file)) return FileTypes.Subtitles;
        else if (FileExtensions.IsPlaylist(file)) return FileTypes.Playlist;
        else return FileTypes.Video;
    }
}
