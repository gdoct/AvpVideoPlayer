// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.VttParser
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitlesParser.Classes.Parsers;

[ExcludeFromCodeCoverage]
public class VttParser : ISubtitlesParser
{
	private readonly string[] _delimiters = new string[3] { "-->", "- >", "->" };

	public List<SubtitleItem> ParseStream(Stream vttStream, Encoding encoding)
	{
		if (!vttStream.CanRead || !vttStream.CanSeek)
		{
			string message = $"Stream must be seekable and readable in a subtitles parser. Operation interrupted; isSeekable: {vttStream.CanSeek} - isReadable: {vttStream.CanSeek}";
			throw new ArgumentException(message);
		}
		vttStream.Position = 0L;
		StreamReader reader = new StreamReader(vttStream, encoding, detectEncodingFromByteOrderMarks: true);
		List<SubtitleItem> list = new List<SubtitleItem>();
		List<string> list2 = GetVttSubTitleParts(reader).ToList();
		if (list2.Any())
		{
			foreach (string item in list2)
			{
				List<string> list3 = (from s in item.Split(new string[1] { Environment.NewLine }, StringSplitOptions.None)
					select s.Trim() into l
					where !string.IsNullOrEmpty(l)
					select l).ToList();
				SubtitleItem subtitleItem = new SubtitleItem();
				foreach (string item2 in list3)
				{
					if (subtitleItem.StartTime == 0 && subtitleItem.EndTime == 0)
					{
						if (TryParseTimecodeLine(item2, out var startTc, out var endTc))
						{
							subtitleItem.StartTime = startTc;
							subtitleItem.EndTime = endTc;
						}
					}
					else
					{
						subtitleItem.Lines.Add(item2);
					}
				}
				if ((subtitleItem.StartTime != 0 || subtitleItem.EndTime != 0) && subtitleItem.Lines.Any())
				{
					list.Add(subtitleItem);
				}
			}
			return list;
		}
		throw new FormatException("Parsing as VTT returned no VTT part.");
	}

	private static IEnumerable<string> GetVttSubTitleParts(TextReader reader)
	{
		StringBuilder sb = new StringBuilder();
		while (true)
		{
			string text;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			string line = (text = reader.ReadLine());
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			if (text == null)
			{
				break;
			}
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			if (string.IsNullOrEmpty(line.Trim()))
			{
				string res = sb.ToString().TrimEnd();
				if (!string.IsNullOrEmpty(res))
				{
					yield return res;
				}
				sb = new StringBuilder();
			}
			else
			{
				sb.AppendLine(line);
			}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
		}
		if (sb.Length > 0)
		{
			yield return sb.ToString();
		}
	}

	private bool TryParseTimecodeLine(string line, out int startTc, out int endTc)
	{
		string[] array = line.Split(_delimiters, StringSplitOptions.None);
		if (array.Length != 2)
		{
			startTc = -1;
			endTc = -1;
			return false;
		}
		startTc = ParseVttTimecode(array[0]);
		endTc = ParseVttTimecode(array[1]);
		return true;
	}

	private static int ParseVttTimecode(string s)
	{
		int hours = 0;
		int minutes = 0;
		int seconds = 0;
		int num = -1;
		Match match = Regex.Match(s, "([0-9]+):([0-9]+):([0-9]+)[,\\.]([0-9]+)", RegexOptions.None, TimeSpan.FromMilliseconds(250));
		if (match.Success)
		{
			hours = int.Parse(match.Groups[1].Value);
			minutes = int.Parse(match.Groups[2].Value);
			seconds = int.Parse(match.Groups[3].Value);
			num = int.Parse(match.Groups[4].Value);
		}
		else
		{
			match = Regex.Match(s, "([0-9]+):([0-9]+)[,\\.]([0-9]+)", RegexOptions.None, TimeSpan.FromMilliseconds(250));
			if (match.Success)
			{
				minutes = int.Parse(match.Groups[1].Value);
				seconds = int.Parse(match.Groups[2].Value);
				num = int.Parse(match.Groups[3].Value);
			}
		}
		if (num >= 0)
		{
			return (int)new TimeSpan(0, hours, minutes, seconds, num).TotalMilliseconds;
		}
		return -1;
	}
}
