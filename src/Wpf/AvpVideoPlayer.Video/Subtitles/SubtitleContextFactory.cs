using System.IO;
namespace AvpVideoPlayer.Video.Subtitles;

public class SubtitleContextFactory : ISubtitleContextFactory
{
    public ISubtitleContext FromFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new FileNotFoundException($"'{nameof(filename)}' cannot be null or whitespace.");
        }
        if (!File.Exists(filename)) throw new FileNotFoundException(nameof(filename));
        return new FileSubtitleContext(filename);
    }

    public IEnumerable<ISubtitleContext> FromVideofile(string filename)
    {
        return EmbeddedSubtitleContext.ListEmbeddedSubtitles(filename)
                                      .Select(s => new AvailableEmbeddedSubtitleContext(s));
    }

    public ISubtitleContext Empty()
    {
        return new EmptySubtitleContext();
    }
}
