namespace AvpVideoPlayer.MetaData;

public class TaggingService : ITaggingService
{
    private readonly IMetaDataContext _context;

    public IList<string> GetTags() =>
        _context.GetTags().Select(x => x.Name).OrderBy(x => x).ToList();

    public TaggingService(IMetaDataContext context)
    {
        _context = context;
    }

    internal void SetupTags()
    {
        _context.AddTag("Special");
        _context.AddTag("Regular");
        _context.AddTag("Boring");
        _context.AddTag("Remove");
    }

    public void Add(string tag) => _context.AddTag(tag);

    public void Remove(string tag) => _context.DeleteTag(tag);
}
