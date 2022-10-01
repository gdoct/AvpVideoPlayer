using AvpVideoPlayer.Api;
using AvpVideoPlayer.MetaData;

namespace AvpVideoPlayer.ViewModels.Events;

public class MetaDataUpdatedEvent : EventBase
{
    public MetaDataUpdatedEvent(FileMetaData metaData)
    {
        MetaData = metaData;
    }

    public FileMetaData MetaData { get; private set; }
}
