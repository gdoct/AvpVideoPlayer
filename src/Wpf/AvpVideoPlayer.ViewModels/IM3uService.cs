using AvpVideoPlayer.Utility;
using System.Collections.Generic;

namespace AvpVideoPlayer.ViewModels;

public interface IM3UService
{
    List<M3UParser.ChannelInfo> ParsePlaylist(string path);
}
