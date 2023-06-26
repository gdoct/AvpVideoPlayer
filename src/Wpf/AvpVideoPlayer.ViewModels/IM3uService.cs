using AvpVideoPlayer.Api;
using System.Collections.Generic;

namespace AvpVideoPlayer.ViewModels;

public interface IM3UService
{
    List<ChannelInfo> ParsePlaylist(string path);
}
