
namespace AvpVideoPlayer.MetaData
{
    public interface ITaggingService
    {
        void Add(string tag);
        IList<string> GetTags();
        void Remove(string tag);
    }
}