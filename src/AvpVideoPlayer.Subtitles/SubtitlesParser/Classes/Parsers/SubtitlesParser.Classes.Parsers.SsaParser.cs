// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.SsaParser
using AvpVideoPlayer.Subtitles.SubtitlesParser.Classes;
using AvpVideoPlayer.Subtitles.SubtitlesParser.Classes.Utils;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AvpVideoPlayer.Subtitles.SubtitlesParser.Classes.Parsers;

[ExcludeFromCodeCoverage]
public class SsaParser : ISubtitlesParser
{
    public List<SubtitleItem> ParseStream(Stream ssaStream, Encoding encoding)
    {
        if (!ssaStream.CanRead || !ssaStream.CanSeek)
        {
            string message = $"Stream must be seekable and readable in a subtitles parser. Operation interrupted; isSeekable: {ssaStream.CanSeek} - isReadable: {ssaStream.CanRead}";
            throw new ArgumentException(message);
        }
        ssaStream.Position = 0L;
        StreamReader streamReader = new StreamReader(ssaStream, encoding, detectEncodingFromByteOrderMarks: true);
        SsaWrapStyle ssaWrapStyle = SsaWrapStyle.None;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        string text = streamReader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        int num = 1;
        while (text != null && text != "[Events]")
        {
            if (text.StartsWith("WrapStyle: "))
            {
                ssaWrapStyle = text.Split(':')[1].TrimStart().FromString();
            }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            text = streamReader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            num++;
        }
        if (text != null)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string text2 = streamReader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            if (!string.IsNullOrEmpty(text2))
            {
                List<string> list = (from head in text2.Split(',')
                                     select head.Trim()).ToList();
                int num2 = list.IndexOf("Start");
                int num3 = list.IndexOf("End");
                int num4 = list.IndexOf("Text");
                if (num2 > 0 && num3 > 0 && num4 > 0)
                {
                    List<SubtitleItem> list2 = new List<SubtitleItem>();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    for (text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
                    {
                        if (!string.IsNullOrEmpty(text))
                        {
                            string[] array = text.Split(',');
                            string s = array[num2];
                            string s2 = array[num3];
                            string text3 = string.Join(",", array.Skip(num4));
                            int num5 = ParseSsaTimecode(s);
                            int num6 = ParseSsaTimecode(s2);
                            if (num5 > 0 && num6 > 0 && !string.IsNullOrEmpty(text3))
                            {
                                List<string> source;
                                switch (ssaWrapStyle)
                                {
                                    case SsaWrapStyle.Smart:
                                    case SsaWrapStyle.EndOfLine:
                                    case SsaWrapStyle.SmartWideLowerLine:
                                        source = text3.Split("\\N").ToList();
                                        break;
                                    case SsaWrapStyle.None:
                                        source = Regex.Split(text3, "(?:\\\\n)|(?:\\\\N)", RegexOptions.None, TimeSpan.FromMilliseconds(250))
                                                      .ToList();
                                        break;
                                    default:
                                        throw new ArgumentException("invalid ssaWrapStyle in the ssaStream");
                                }
                                source = source.Select((line) => line.TrimStart()).ToList();
                                SubtitleItem item = new SubtitleItem
                                {
                                    StartTime = num5,
                                    EndTime = num6,
                                    Lines = source,
                                    PlaintextLines = source.Select((subtitleLine) =>
                                    Regex.Replace(subtitleLine, "\\{.*?\\}", string.Empty, RegexOptions.None, TimeSpan.FromMilliseconds(250))).ToList()
                                };
                                list2.Add(item);
                            }
                        }
                    }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    if (list2.Any())
                    {
                        return list2;
                    }
                    throw new ArgumentException("Stream is not in a valid Ssa format");
                }
                string message2 = string.Format("Couldn't find all the necessary columns headers ({0}, {1}, {2}) in header line {3}", "Start", "End", "Text", text2);
                throw new ArgumentException(message2);
            }
            string message3 = $"The header line after the line '{text}' was null -> no need to continue parsing";
            throw new ArgumentException(message3);
        }
        string message4 = string.Format("We reached line '{0}' with line number #{1} without finding to Event section ({2})", text, num, "[Events]");
        throw new ArgumentException(message4);
    }

    private static int ParseSsaTimecode(string s)
    {
        if (TimeSpan.TryParse(s, out var result))
        {
            return (int)result.TotalMilliseconds;
        }
        return -1;
    }
}
