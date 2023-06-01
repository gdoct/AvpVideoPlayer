using AvpVideoPlayer.Utility;
using static AvpVideoPlayer.Utility.M3UParser;
using System.Collections.Generic;
using System.IO;

namespace AvpVideoPlayer.ViewModels;

public class M3UService : IM3UService 
{
    private string _path = string.Empty;

    private List<ChannelInfo> _channels { get; set; } = new List<ChannelInfo>();

    public M3UService()
    {
    }

    public List<ChannelInfo> ParsePlaylist(string path)
    {
        if (string.Compare(path, _path, System.StringComparison.OrdinalIgnoreCase) != 0 && File.Exists(path))
        {
            _channels = new M3UParser(path).ParsePlaylist();
            _path = path;
        }
        return _channels;
    }
}
