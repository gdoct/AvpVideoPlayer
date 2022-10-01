namespace AvpVideoPlayer.MetaData;

public interface IMetaDataService
{
    FileMetaData GetMetadata(string path);
    IList<FileMetaData> GetMetadataForPath(string path);
    void SaveMetadata(FileMetaData metaData);
}
