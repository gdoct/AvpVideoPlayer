using System;
using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class SelectVideoEvent : EventBase<FileViewModel>
{
    public SelectVideoEvent(FileViewModel data) : base(data)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));
        Name = data.Name ?? string.Empty;
        IsStream = data is VideoStreamViewModel;
    }

    public bool IsStream { get; }

    public string Name { get;  }
}
