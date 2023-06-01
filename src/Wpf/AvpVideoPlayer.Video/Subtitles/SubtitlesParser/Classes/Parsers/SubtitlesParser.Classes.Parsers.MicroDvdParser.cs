// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.MicroDvdParser
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitlesParser.Classes.Parsers;

[ExcludeFromCodeCoverage]
public class MicroDvdParser : ISubtitlesParser
{
	private readonly float defaultFrameRate = 25f;

	private readonly char[] _lineSeparators = new char[1] { '|' };

	public MicroDvdParser()
	{
	}

	public MicroDvdParser(float defaultFrameRate)
	{
		this.defaultFrameRate = defaultFrameRate;
	}

	public List<SubtitleItem> ParseStream(Stream subStream, Encoding encoding)
	{
		if (!subStream.CanRead || !subStream.CanSeek)
		{
			string message = $"Stream must be seekable and readable in a subtitles parser. Operation interrupted; isSeekable: {subStream.CanSeek} - isReadable: {subStream.CanSeek}";
			throw new ArgumentException(message);
		}
		subStream.Position = 0L;
		using StreamReader streamReader = new StreamReader(subStream, encoding, detectEncodingFromByteOrderMarks: true);
		List<SubtitleItem> list = new List<SubtitleItem>();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
		string text = streamReader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
		while (text != null && !IsMicroDvdLine(text))
		{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			text = streamReader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
		}
		if (text != null)
		{
			SubtitleItem subtitleItem = ParseLine(text, defaultFrameRate);
			float frameRate;
			if (subtitleItem.Lines != null && subtitleItem.Lines.Any())
			{
				if (!TryExtractFrameRate(subtitleItem.Lines[0], out frameRate))
				{
					Console.WriteLine("Couldn't extract frame rate of sub file with first line {0}. We use the default frame rate: {1}", text, defaultFrameRate);
					frameRate = defaultFrameRate;
					list.Add(subtitleItem);
				}
			}
			else
			{
				frameRate = defaultFrameRate;
			}
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			for (text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
			{
				if (!string.IsNullOrEmpty(text))
				{
					SubtitleItem item = ParseLine(text, frameRate);
					list.Add(item);
				}
			}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
		}
		if (list.Any())
		{
			return list;
		}
		throw new ArgumentException("Stream is not in a valid MicroDVD format");
	}

	private static bool IsMicroDvdLine(string line)
	{
		return Regex.IsMatch(line, "^[{\\[](-?\\d+)[}\\]][{\\[](-?\\d+)[}\\]](.*)", RegexOptions.None, TimeSpan.FromMilliseconds(250));
	}

	private SubtitleItem ParseLine(string line, float frameRate)
	{
		Match match = Regex.Match(line, "^[{\\[](-?\\d+)[}\\]][{\\[](-?\\d+)[}\\]](.*)", RegexOptions.None, TimeSpan.FromMilliseconds(250));
		if (match.Success && match.Groups.Count > 2)
		{
			string value = match.Groups[1].Value;
			int startTime = (int)(1000.0 * double.Parse(value) / (double)frameRate);
			string value2 = match.Groups[2].Value;
			int endTime = (int)(1000.0 * double.Parse(value2) / (double)frameRate);
			string value3 = match.Groups[match.Groups.Count - 1].Value;
			string[] source = value3.Split(_lineSeparators);
			List<string> lines = source.Where((string l) => !string.IsNullOrEmpty(l)).ToList();
			return new SubtitleItem
			{
				Lines = lines,
				StartTime = startTime,
				EndTime = endTime
			};
		}
		string message = $"The subtitle file line {line} is not in the micro dvd format. We stop the process.";
		throw new InvalidDataException(message);
	}

	private bool TryExtractFrameRate(string text, out float frameRate)
	{
		if (!string.IsNullOrEmpty(text))
		{
			return float.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out frameRate);
		}
		frameRate = defaultFrameRate;
		return false;
	}
}
