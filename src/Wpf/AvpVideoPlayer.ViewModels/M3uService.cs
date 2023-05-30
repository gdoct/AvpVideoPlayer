using AvpVideoPlayer.Utility;
using static AvpVideoPlayer.Utility.M3uParser;
using System.Collections.Generic;
using System.IO;

namespace AvpVideoPlayer.ViewModels;

public class M3uService : IM3uService 
{
    private string _path = string.Empty;

    private List<ChannelInfo> _channels { get; set; } = new List<ChannelInfo>();

    public M3uService()
    {
    }

    public List<ChannelInfo> ParsePlaylist(string path)
    {
        if (string.Compare(path, _path, System.StringComparison.OrdinalIgnoreCase) != 0 && File.Exists(path))
        {
            _channels = new M3uParser(path).ParsePlaylist();
            _path = path;
        }
        return _channels;
    }
}
