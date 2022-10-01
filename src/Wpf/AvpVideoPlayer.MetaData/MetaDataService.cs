namespace AvpVideoPlayer.MetaData;

public class MetaDataService : IMetaDataService
{
    private readonly IMetaDataContext _context;

    public MetaDataService(IMetaDataContext context)
    {
        _context = context;
    }

    public FileMetaData GetMetadata(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
        }

        var fi = new FileInfo(path);
        var existing = _context.GetMetadata(fi);
        if (existing != null) return existing;
        return FileMetaData.Create(fi);
    }


    public IList<FileMetaData> GetMetadataForPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
        }

        return _context.GetMetadataForPath(path); 
    }

    public void SaveMetadata(FileMetaData metaData)
    {
        _context.SaveMetadata(metaData);
    }
}
