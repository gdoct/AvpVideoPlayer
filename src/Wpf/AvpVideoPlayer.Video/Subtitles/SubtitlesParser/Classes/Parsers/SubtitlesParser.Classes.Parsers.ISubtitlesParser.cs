// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.ISubtitlesParser
using System.IO;
using System.Text;

namespace SubtitlesParser.Classes.Parsers;

public interface ISubtitlesParser
{
	List<SubtitleItem> ParseStream(Stream stream, Encoding encoding);
}
