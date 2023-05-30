using AvpVideoPlayer.Utility;
using System.Collections.Generic;

namespace AvpVideoPlayer.ViewModels;

public interface IM3uService
{
    List<M3uParser.ChannelInfo> ParsePlaylist(string path);
}
