namespace AvpVideoPlayer.Subtitles;

public interface ISubtitleContextFactory
{
    /// <summary>
    /// Create an empty set of subtitles 
    /// </summary>
    /// <returns></returns>
    ISubtitleContext Empty();

    /// <summary>
    /// Load from a supported subtitle file type
    /// </summary>
    /// <param name="filename"></param>
    /// <returns>a subtitle context</returns>
    ISubtitleContext FromFile(string filename);

    /// <summary>
    /// Lazy-load all embedded subtitles
    /// </summary>
    /// <param name="filename"></param>
    /// <returns>all embedded subtitles in the file as lazy-loading contexts</returns>
    IEnumerable<ISubtitleContext> FromVideofile(string filename);
}
