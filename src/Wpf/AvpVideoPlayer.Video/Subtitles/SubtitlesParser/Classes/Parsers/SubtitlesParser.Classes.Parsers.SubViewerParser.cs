// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.SubViewerParser
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitlesParser.Classes.Parsers;

public class SubViewerParser : ISubtitlesParser
{
	private const string FirstLine = "[INFORMATION]";

	private const short MaxLineNumberForItems = 20;

	private readonly Regex _timestampRegex = new Regex("\\d{2}:\\d{2}:\\d{2}\\.\\d{2},\\d{2}:\\d{2}:\\d{2}\\.\\d{2}", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

	private const char TimecodeSeparator = ',';

	public List<SubtitleItem> ParseStream(Stream subStream, Encoding encoding)
	{
		subStream.Position = 0L;
		StreamReader streamReader = new StreamReader(subStream, encoding, detectEncodingFromByteOrderMarks: true);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
		string text = streamReader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
		if (text == "[INFORMATION]")
		{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			string text2 = streamReader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			int num = 2;
			while (text2 != null && num <= 20 && !IsTimestampLine(text2))
			{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
				text2 = streamReader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
				num++;
			}
			if (text2 != null && num <= 20 && IsTimestampLine(text2))
			{
				List<SubtitleItem> list = new List<SubtitleItem>();
				string line = text2;
				List<string> list2 = new List<string>();
				while (text2 != null)
				{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
					text2 = streamReader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
					if (IsTimestampLine(text2))
					{
						Tuple<int, int> tuple = ParseTimecodeLine(line);
						int item = tuple.Item1;
						int item2 = tuple.Item2;
						if (item > 0 && item2 > 0 && list2.Any())
						{
							list.Add(new SubtitleItem
							{
								StartTime = item,
								EndTime = item2,
								Lines = list2
							});
						}
						line = text2;
						list2 = new List<string>();
					}
					else if (!string.IsNullOrEmpty(text2))
					{
						list2.Add(text2);
					}
#pragma warning restore CS8604 // Possible null reference argument.
				}
				Tuple<int, int> tuple2 = ParseTimecodeLine(line);
				int item3 = tuple2.Item1;
				int item4 = tuple2.Item2;
				if (item3 > 0 && item4 > 0 && list2.Any())
				{
					list.Add(new SubtitleItem
					{
						StartTime = item3,
						EndTime = item4,
						Lines = list2
					});
				}
				if (list.Any())
				{
					return list;
				}
				throw new ArgumentException("Stream is not in a valid SubViewer format");
			}
			string message = $"Couldn't find the first timestamp line in the current sub file. Last line read: '{text2}', line number #{num}";
			throw new ArgumentException(message);
		}
		throw new ArgumentException("Stream is not in a valid SubViewer format");
	}

	private Tuple<int, int> ParseTimecodeLine(string line)
	{
		string[] array = line.Split(',');
		if (array.Length == 2)
		{
			int item = ParseTimecode(array[0]);
			int item2 = ParseTimecode(array[1]);
			return new Tuple<int, int>(item, item2);
		}
		string message = $"Couldn't parse the timecodes in line '{line}'";
		throw new ArgumentException(message);
	}

	private static int ParseTimecode(string s)
	{
		if (TimeSpan.TryParse(s, out var result))
		{
			return (int)result.TotalMilliseconds;
		}
		return -1;
	}

	private bool IsTimestampLine(string line)
	{
		if (string.IsNullOrEmpty(line))
		{
			return false;
		}
		return _timestampRegex.IsMatch(line);
	}
}
