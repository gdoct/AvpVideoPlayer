// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Writers.ISubtitlesWriter
using System.IO;
using AvpVideoPlayer.Subtitles.SubtitlesParser.Classes;

namespace AvpVideoPlayer.Subtitles.SubtitlesParser.Classes.Writers;

public interface ISubtitlesWriter
{
    void WriteStream(Stream stream, IEnumerable<SubtitleItem> subtitleItems, bool includeFormatting = true);

    Task WriteStreamAsync(Stream stream, IEnumerable<SubtitleItem> subtitleItems, bool includeFormatting = true);
}
