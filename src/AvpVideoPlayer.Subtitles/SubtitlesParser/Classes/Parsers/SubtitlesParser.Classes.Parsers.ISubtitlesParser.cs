// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.ISubtitlesParser
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using AvpVideoPlayer.Subtitles.SubtitlesParser.Classes;

namespace AvpVideoPlayer.Subtitles.SubtitlesParser.Classes.Parsers;

public interface ISubtitlesParser
{
    List<SubtitleItem> ParseStream(Stream stream, Encoding encoding);
}
