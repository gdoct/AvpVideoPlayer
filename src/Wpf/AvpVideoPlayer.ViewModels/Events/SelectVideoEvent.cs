using System;
using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels.IO;

namespace AvpVideoPlayer.ViewModels.Events;

public class SelectVideoEvent : EventBase<FileViewModel>
{
    public SelectVideoEvent(FileViewModel data) : base(data)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));
        Name = data.Name ?? string.Empty;
        IsStream = data is VideoStreamViewModel;
        IsCategory = data is VideoStreamCategoryViewModel;
    }

    public bool IsStream { get; }
    
    public bool IsCategory { get; }

    public string Name { get;  }
}
