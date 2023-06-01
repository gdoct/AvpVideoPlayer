// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.SubParser
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitlesParser.Classes.Parsers;

[ExcludeFromCodeCoverage]
public class SubParser
{
	private readonly Dictionary<SubtitlesFormat, ISubtitlesParser> _subFormatToParser = new Dictionary<SubtitlesFormat, ISubtitlesParser>
	{
		{
			SubtitlesFormat.SubRipFormat,
			new SrtParser()
		},
		{
			SubtitlesFormat.MicroDvdFormat,
			new MicroDvdParser()
		},
		{
			SubtitlesFormat.SubViewerFormat,
			new SubViewerParser()
		},
		{
			SubtitlesFormat.SubStationAlphaFormat,
			new SsaParser()
		},
		{
			SubtitlesFormat.TtmlFormat,
			new TtmlParser()
		},
		{
			SubtitlesFormat.WebVttFormat,
			new VttParser()
		},
		{
			SubtitlesFormat.YoutubeXmlFormat,
			new YtXmlFormatParser()
		}
	};

	public static SubtitlesFormat GetMostLikelyFormat(string fileName)
	{
		string extension = Path.GetExtension(fileName);
		if (!string.IsNullOrEmpty(extension))
		{
			foreach (SubtitlesFormat supportedSubtitlesFormat in SubtitlesFormat.SupportedSubtitlesFormats)
			{
				if (supportedSubtitlesFormat.Extension != null && Regex.IsMatch(extension, supportedSubtitlesFormat.Extension, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
				{
					return supportedSubtitlesFormat;
				}
			}
		}
#pragma warning disable CS8603 // Possible null reference return.
		return null;
#pragma warning restore CS8603 // Possible null reference return.
	}

	public List<SubtitleItem> ParseStream(Stream stream)
	{
		return ParseStream(stream, Encoding.UTF8);
	}

	public List<SubtitleItem> ParseStream(Stream stream, Encoding encoding, SubtitlesFormat? subFormat = null)
	{
		Dictionary<SubtitlesFormat, ISubtitlesParser> subFormatDictionary = ((subFormat != null) ? _subFormatToParser.OrderBy((KeyValuePair<SubtitlesFormat, ISubtitlesParser> dic) => Math.Abs(string.Compare(dic.Key.Name, subFormat.Name, StringComparison.Ordinal))).ToDictionary((KeyValuePair<SubtitlesFormat, ISubtitlesParser> entry) => entry.Key, (KeyValuePair<SubtitlesFormat, ISubtitlesParser> entry) => entry.Value) : _subFormatToParser);
		return ParseStream(stream, encoding, subFormatDictionary);
	}

	public List<SubtitleItem> ParseStream(Stream stream, Encoding encoding, Dictionary<SubtitlesFormat, ISubtitlesParser> subFormatDictionary)
	{
		if (!stream.CanRead)
		{
			throw new ArgumentException("Cannot parse a non-readable stream");
		}
		Stream stream2 = stream;
		if (!stream.CanSeek)
		{
			stream2 = StreamHelpers.CopyStream(stream);
			stream2.Seek(0L, SeekOrigin.Begin);
		}
		subFormatDictionary = subFormatDictionary ?? _subFormatToParser;
		foreach (KeyValuePair<SubtitlesFormat, ISubtitlesParser> item in subFormatDictionary)
		{
			try
			{
				ISubtitlesParser value = item.Value;
				return value.ParseStream(stream2, encoding);
			}
			catch (Exception)
			{
			}
		}
		string arg = LogFirstCharactersOfStream(stream, 500, encoding);
		string message = $"All the subtitles parsers failed to parse the following stream:{arg}";
		throw new ArgumentException(message);
	}

	private static string LogFirstCharactersOfStream(Stream stream, int nbOfCharactersToPrint, Encoding encoding)
	{
#pragma warning disable CS0219 // Variable is assigned but its value is never used
		string text = "";
#pragma warning restore CS0219 // Variable is assigned but its value is never used
		if (stream.CanRead)
		{
			if (stream.CanSeek)
			{
				stream.Position = 0L;
			}
			StreamReader streamReader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true);
			char[] array = new char[nbOfCharactersToPrint];
			streamReader.ReadBlock(array, 0, nbOfCharactersToPrint);
			return string.Format("Parsing of subtitle stream failed. Beginning of sub stream:\n{0}", string.Join("", array));
		}
		return $"Tried to log the first {nbOfCharactersToPrint} characters of a closed stream";
	}
}
