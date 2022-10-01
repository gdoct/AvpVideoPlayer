namespace AvpVideoPlayer.MetaData;

public interface IMetaDataContext
{
    void AddTag(string tag);
    void DeleteMetadata(FileMetaData metaData);
    void DeleteTag(string tag);
    FileMetaData? GetMetadata(FileInfo file);
    IEnumerable<FileMetaData> GetMetadata();
    IList<FileMetaData> GetMetadataForPath(string path);
    List<TagData> GetTags();
    void SaveMetadata(FileMetaData metaData);
}
