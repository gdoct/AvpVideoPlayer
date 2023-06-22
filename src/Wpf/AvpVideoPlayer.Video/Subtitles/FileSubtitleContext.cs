using AvpVideoPlayer.Api;
using SubtitlesParser.Classes;
using SubtitlesParser.Classes.Parsers;
using System.IO;
using System.Text.RegularExpressions;

namespace AvpVideoPlayer.Video.Subtitles;

/// <inheritdoc cref="ISubtitleContext"/>
internal class FileSubtitleContext : ISubtitleContext
{
    private readonly IList<SubtitleData> _subtitles;

    public FileSubtitleContext(string filename)
    {
        _subtitles = LoadSubtitlesFromFile(filename);
        SubtitleInfo = new SubtitleInfo
        {
            VideoFilename = filename,
            SubtitleName = "default",
            Index = 0,
            StreamInfo = string.Empty
        };
    }

    public SubtitleInfo SubtitleInfo { get; }

    /// <inheritdoc cref="ISubtitleContext.GetSubtitleForTime(TimeSpan)"/>
    public SubtitleData? GetSubtitleForTime(TimeSpan time) => _subtitles.FirstOrDefault(c => c.StartTime <= time.TotalMilliseconds && c.EndTime >= time.TotalMilliseconds);

    /// <inheritdoc cref="ISubtitleContext.GetNextSubtitleForTime(TimeSpan)"/>
    public SubtitleData? GetNextSubtitleForTime(TimeSpan time) => _subtitles.FirstOrDefault(c => c.StartTime >= time.TotalMilliseconds);

    private static IList<SubtitleData> LoadSubtitlesFromFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
        {
            return new List<SubtitleData>();
        }
        using var stream = File.OpenRead(filename);
        return new SubParser().ParseStream(stream)
                              .OrderBy(s => s.StartTime)
                              .Select(ConvertToSubtitleData)
                              .ToList();
    }

    private static SubtitleData ConvertToSubtitleData(SubtitleItem subtitle)
    {
        return new SubtitleData
        (
            subtitle.StartTime,
            subtitle.EndTime,
            ConvertLines(subtitle.Lines).ToList()
        );
    }

    private static IEnumerable<SubtitleData.SubtitleLine> ConvertLines(List<string> lines) =>
        lines.Select(line => new SubtitleData.SubtitleLine
        {
            Italic = HasTag(line, "<i>"),
            Bold = HasTag(line, "<b>"),
            Underline = HasTag(line, "<u>"),
            Text = Regex.Replace(line.Trim(), "<.*?>", string.Empty, RegexOptions.None, TimeSpan.FromMilliseconds(250))
                                  .Replace(@"{\an7}", "")
                                  .Replace(@"\h", "")
        });

    private static bool HasTag(string line, string tag)
    {
        var closetag = "</" + tag[1..];
        if (line.Contains(tag) || line.Contains(closetag))
        {
            return true;
        }
        return false;
    }
}
