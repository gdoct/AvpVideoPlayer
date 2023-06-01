using AvpVideoPlayer.Utility;
using System.Collections.Generic;

namespace AvpVideoPlayer.ViewModels;

public interface IM3uService
{
    List<M3UParser.ChannelInfo> ParsePlaylist(string path);
}
