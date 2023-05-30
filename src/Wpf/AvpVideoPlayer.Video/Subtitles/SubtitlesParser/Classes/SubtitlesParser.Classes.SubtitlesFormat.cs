// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.SubtitlesFormat
namespace SubtitlesParser.Classes;

public class SubtitlesFormat
{
	public static SubtitlesFormat SubRipFormat = new SubtitlesFormat
	{
		Name = "SubRip",
		Extension = "\\.srt"
	};

	public static SubtitlesFormat MicroDvdFormat = new SubtitlesFormat
	{
		Name = "MicroDvd",
		Extension = "\\.sub"
	};

	public static SubtitlesFormat SubViewerFormat = new SubtitlesFormat
	{
		Name = "SubViewer",
		Extension = "\\.sub"
	};

	public static SubtitlesFormat SubStationAlphaFormat = new SubtitlesFormat
	{
		Name = "SubStationAlpha",
		Extension = "\\.ssa"
	};

	public static SubtitlesFormat TtmlFormat = new SubtitlesFormat
	{
		Name = "TTML",
		Extension = "\\.ttml"
	};

	public static SubtitlesFormat WebVttFormat = new SubtitlesFormat
	{
		Name = "WebVTT",
		Extension = "\\.vtt"
	};

	public static SubtitlesFormat YoutubeXmlFormat = new SubtitlesFormat
	{
		Name = "YoutubeXml"
	};

	public static List<SubtitlesFormat> SupportedSubtitlesFormats = new List<SubtitlesFormat> { SubRipFormat, MicroDvdFormat, SubViewerFormat, SubStationAlphaFormat, TtmlFormat, WebVttFormat, YoutubeXmlFormat };

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
