using AvpVideoPlayer.Api;
using AvpVideoPlayer.Subtitles.SubtitlesParser.Classes.Parsers;

namespace AvpVideoPlayer.Subtitles;

/// <inheritdoc cref="ISubtitleContext"/>
internal class EmbeddedSubtitleContext
{
    public static ISubtitleContext? ExtractEmbeddedSubtitle(SubtitleInfo subtitleInfo)
    {
        var parser = new SubParser();
        using var fileStream = File.OpenRead(subtitleInfo.VideoFilename);
        var items = parser.ParseStream(fileStream); // parse the subtitles from the mkv stream
        if (items.Count > subtitleInfo.Index)
        {
            return new StringSubtitleContext(subtitleInfo.SubtitleName, string.Join(Environment.NewLine, items[subtitleInfo.Index].Lines));
        }
        return null;
    }


    public static IEnumerable<SubtitleInfo> ListEmbeddedSubtitles(string videofile)
    {
        var parser = new SubParser();
        using var fileStream = File.OpenRead(videofile);
        var items = parser.ParseStream(fileStream); // parse the subtitles from the mkv stream
        var id = 0;
        foreach (var item in items)
        {
            yield return new SubtitleInfo
            {
                VideoFilename = videofile,
                Index = id,
                SubtitleName = $"subtitle {++id}",
                StreamInfo = $"0:0"
            };
        }

    }
}
