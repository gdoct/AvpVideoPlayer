// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.SrtParser
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AvpVideoPlayer.Subtitles.SubtitlesParser.Classes;

namespace AvpVideoPlayer.Subtitles.SubtitlesParser.Classes.Parsers;

[ExcludeFromCodeCoverage]
public class SrtParser : ISubtitlesParser
{
    private readonly string[] _delimiters = new string[3] { "-->", "- >", "->" };

    public List<SubtitleItem> ParseStream(Stream srtStream, Encoding encoding)
    {
        if (!srtStream.CanRead || !srtStream.CanSeek)
        {
            string message = $"Stream must be seekable and readable in a subtitles parser. Operation interrupted; isSeekable: {srtStream.CanSeek} - isReadable: {srtStream.CanSeek}";
            throw new ArgumentException(message);
        }
        srtStream.Position = 0L;
        StreamReader reader = new StreamReader(srtStream, encoding, detectEncodingFromByteOrderMarks: true);
        List<SubtitleItem> list = new List<SubtitleItem>();
        List<string> list2 = GetSrtSubTitleParts(reader).ToList();
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
                        subtitleItem.PlaintextLines.Add(Regex.Replace(item2, "\\{.*?\\}|<.*?>", string.Empty, RegexOptions.None, TimeSpan.FromMilliseconds(250)));
                    }
                }
                if ((subtitleItem.StartTime != 0 || subtitleItem.EndTime != 0) && subtitleItem.Lines.Any())
                {
                    list.Add(subtitleItem);
                }
            }
            if (list.Any())
            {
                return list;
            }
            throw new ArgumentException("Stream is not in a valid Srt format");
        }
        throw new FormatException("Parsing as srt returned no srt part.");
    }

    private static IEnumerable<string> GetSrtSubTitleParts(TextReader reader)
    {
        StringBuilder sb = new StringBuilder();
        while (true)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string line = reader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            if (line == null)
            {
                break;
            }
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
        startTc = ParseSrtTimecode(array[0]);
        endTc = ParseSrtTimecode(array[1]);
        return true;
    }

    private static int ParseSrtTimecode(string s)
    {
        Match match = Regex.Match(s, "[0-9]+:[0-9]+:[0-9]+([,\\.][0-9]+)?", RegexOptions.None, TimeSpan.FromMilliseconds(250));
        if (match.Success)
        {
            s = match.Value;
            if (TimeSpan.TryParse(s.Replace(',', '.'), out var result))
            {
                return (int)result.TotalMilliseconds;
            }
        }
        return -1;
    }
}
