// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Writers.SsaWriter
using SubtitlesParser.Classes.Utils;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace SubtitlesParser.Classes;

[ExcludeFromCodeCoverage]
public class SsaWriter : ISubtitlesWriter
{
	private void WriteHeader(TextWriter writer, SsaWrapStyle wrapStyle = SsaWrapStyle.None)
	{
		writer.WriteLine("[Script Info]");
		writer.WriteLine($"{';'} Script generated by SubtitlesParser v{GetType().Assembly.GetName().Version}");
		writer.WriteLine($"{';'} https://github.com/AlexPoint/SubtitlesParser");
		writer.WriteLine("ScriptType: v4.00");
		writer.WriteLine(string.Format("{0}{1}", "WrapStyle: ", wrapStyle));
		writer.WriteLine();
		writer.Flush();
	}

	private async Task WriteHeaderAsync(TextWriter writer, SsaWrapStyle wrapStyle = SsaWrapStyle.None)
	{
		await writer.WriteLineAsync("[Script Info]");
		await writer.WriteLineAsync($"{';'} Script generated by SubtitlesParser v{GetType().Assembly.GetName().Version}");
		await writer.WriteLineAsync($"{';'} https://github.com/AlexPoint/SubtitlesParser");
		await writer.WriteLineAsync("ScriptType: v4.00");
		await writer.WriteLineAsync(string.Format("{0}{1}", "WrapStyle: ", wrapStyle));
		await writer.WriteLineAsync();
		await writer.FlushAsync();
	}

	private static string SubtitleItemToDialogueLine(SubtitleItem subtitleItem, bool includeFormatting)
	{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		string[] array = new string[10]
		{
			"0",
			TimeSpan.FromMilliseconds(subtitleItem.StartTime).ToString("h\\:mm\\:ss\\.fff"),
			TimeSpan.FromMilliseconds(subtitleItem.EndTime).ToString("h\\:mm\\:ss\\.fff"),
			null,
			null,
			"0",
			"0",
			"0",
			null,
			null
		};
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
		List<string> source = ((!includeFormatting && subtitleItem.PlaintextLines != null) ? subtitleItem.PlaintextLines : subtitleItem.Lines);
		array[9] = source.Aggregate(string.Empty, (string current, string line) => current + line + "\\N").TrimEnd('\\', 'N');
		StringBuilder stringBuilder = new StringBuilder("Dialogue: ");
		return stringBuilder.AppendJoin(',', array).ToString();
	}

	public void WriteStream(Stream stream, IEnumerable<SubtitleItem> subtitleItems, bool includeFormatting = true)
	{
		using TextWriter textWriter = new StreamWriter(stream);
		WriteHeader(textWriter);
		textWriter.WriteLine("[Events]");
		textWriter.WriteLine("Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text");
		foreach (SubtitleItem subtitleItem in subtitleItems)
		{
			textWriter.WriteLine(SubtitleItemToDialogueLine(subtitleItem, includeFormatting));
		}
	}

	public async Task WriteStreamAsync(Stream stream, IEnumerable<SubtitleItem> subtitleItems, bool includeFormatting = true)
	{
		await using TextWriter writer = new StreamWriter(stream);
		await WriteHeaderAsync(writer);
		await writer.WriteLineAsync("[Events]");
		await writer.WriteLineAsync("Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text");
		foreach (SubtitleItem item in subtitleItems)
		{
			await writer.WriteLineAsync(SubtitleItemToDialogueLine(item, includeFormatting));
		}
	}
}
