using AvpVideoPlayer.Api;
using AvpVideoPlayer.Utility;
using System.IO;

namespace AvpVideoPlayer.ViewModels;

public class VideoFileViewModel : FileViewModel
{
    public VideoFileViewModel(FileInfo file) : base(GextFileType(file))
    {
        Path = file.FullName;
        Name = file.Name;
    }

    private static FileTypes GextFileType(FileInfo file)
    {
        if (FileExtensions.IsSubtitleFile(file)) return FileTypes.Subtitles;
        return FileTypes.Video;
    }
}
