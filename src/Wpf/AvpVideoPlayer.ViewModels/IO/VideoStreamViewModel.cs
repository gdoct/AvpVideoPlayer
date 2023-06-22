using AvpVideoPlayer.Api;
using AvpVideoPlayer.Utility;
using System;
using System.IO;

namespace AvpVideoPlayer.ViewModels.IO;

public class VideoStreamViewModel : FileViewModel
{
    public VideoStreamViewModel(string name, Uri uri) : base(FileTypes.VideoStream)
    {
        Path = uri.ToString();
        Name = name;
    }
}
