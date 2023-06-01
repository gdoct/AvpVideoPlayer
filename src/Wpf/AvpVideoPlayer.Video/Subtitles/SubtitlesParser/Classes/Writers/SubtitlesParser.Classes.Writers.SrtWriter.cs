// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Writers.SrtWriter
namespace SubtitlesParser.Classes;

using System.Diagnostics.CodeAnalysis;
using System.IO;

[ExcludeFromCodeCoverage]
public class SrtWriter : ISubtitlesWriter
{
	private static IEnumerable<string> SubtitleItemToSubtitleEntry(SubtitleItem subtitleItem, int subtitleEntryNumber, bool includeFormatting)
	{
		List<string> list = new List<string>();
		list.Add(subtitleEntryNumber.ToString());
		list.Add(formatTimecodeLine());
		if (!includeFormatting && subtitleItem.PlaintextLines != null)
		{
			list.AddRange(subtitleItem.PlaintextLines);
		}
		else
		{
			list.AddRange(subtitleItem.Lines);
		}
		return list;
		string formatTimecodeLine()
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(subtitleItem.StartTime);
			TimeSpan timeSpan2 = TimeSpan.FromMilliseconds(subtitleItem.EndTime);
			return $"{timeSpan:hh\\:mm\\:ss\\,fff} --> {timeSpan2:hh\\:mm\\:ss\\,fff}";
		}
	}

	public void WriteStream(Stream stream, IEnumerable<SubtitleItem> subtitleItems, bool includeFormatting = true)
	{
		using TextWriter textWriter = new StreamWriter(stream);
		List<SubtitleItem> list = subtitleItems.ToList();
		for (int i = 0; i < list.Count; i++)
		{
			SubtitleItem subtitleItem = list[i];
			IEnumerable<string> enumerable = SubtitleItemToSubtitleEntry(subtitleItem, i + 1, includeFormatting);
			foreach (string item in enumerable)
			{
				textWriter.WriteLine(item);
			}
			textWriter.WriteLine();
		}
	}

	public async Task WriteStreamAsync(Stream stream, IEnumerable<SubtitleItem> subtitleItems, bool includeFormatting = true)
	{
		await using TextWriter writer = new StreamWriter(stream);
		List<SubtitleItem> items = subtitleItems.ToList();
		for (int i = 0; i < items.Count; i++)
		{
			SubtitleItem subtitleItem = items[i];
			IEnumerable<string> lines = SubtitleItemToSubtitleEntry(subtitleItem, i + 1, includeFormatting);
			foreach (string line in lines)
			{
				await writer.WriteLineAsync(line);
			}
			await writer.WriteLineAsync();
		}
	}
}
