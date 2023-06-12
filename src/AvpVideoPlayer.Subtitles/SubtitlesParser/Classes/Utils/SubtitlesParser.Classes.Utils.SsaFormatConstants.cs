// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Utils.SsaFormatConstants

using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.Subtitles.SubtitlesParser.Classes.Utils;

[ExcludeFromCodeCoverage]
public static class SsaFormatConstants
{
    public const string SCRIPT_INFO_LINE = "[Script Info]";

    public const string EVENT_LINE = "[Events]";

    public const char SEPARATOR = ',';

    public const char COMMENT = ';';

    public const string WRAP_STYLE_PREFIX = "WrapStyle: ";

    public const string DIALOGUE_PREFIX = "Dialogue: ";

    public const string START_COLUMN = "Start";

    public const string END_COLUMN = "End";

    public const string TEXT_COLUMN = "Text";
}
