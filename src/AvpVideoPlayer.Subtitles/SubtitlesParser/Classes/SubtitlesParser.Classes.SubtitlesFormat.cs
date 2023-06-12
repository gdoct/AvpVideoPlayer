// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.SubtitlesFormat
using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.Subtitles.SubtitlesParser.Classes;

[ExcludeFromCodeCoverage]
public class SubtitlesFormat
{
    public static readonly SubtitlesFormat SubRipFormat = new()
    {
        Name = "SubRip",
        Extension = "\\.srt"
    };

    public static readonly SubtitlesFormat MicroDvdFormat = new()
    {
        Name = "MicroDvd",
        Extension = "\\.sub"
    };

    public static readonly SubtitlesFormat SubViewerFormat = new()
    {
        Name = "SubViewer",
        Extension = "\\.sub"
    };

    public static readonly SubtitlesFormat SubStationAlphaFormat = new()
    {
        Name = "SubStationAlpha",
        Extension = "\\.ssa"
    };

    public static readonly SubtitlesFormat TtmlFormat = new()
    {
        Name = "TTML",
        Extension = "\\.ttml"
    };

    public static readonly SubtitlesFormat WebVttFormat = new()
    {
        Name = "WebVTT",
        Extension = "\\.vtt"
    };

    public static readonly SubtitlesFormat YoutubeXmlFormat = new()
    {
        Name = "YoutubeXml"
    };

    public static readonly List<SubtitlesFormat> SupportedSubtitlesFormats = new List<SubtitlesFormat> { SubRipFormat, MicroDvdFormat, SubViewerFormat, SubStationAlphaFormat, TtmlFormat, WebVttFormat, YoutubeXmlFormat };

    public string Name { get; set; }

    public string Extension { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private SubtitlesFormat()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }
}
