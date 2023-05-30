// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Utils.SsaWrapStyleExtensions
namespace SubtitlesParser.Classes.Utils;

public static class SsaWrapStyleExtensions
{
	public static SsaWrapStyle FromString(this string rawString)
	{
		int result;
		return (!int.TryParse(rawString, out result)) ? SsaWrapStyle.None : result.FromInt();
	}

	public static SsaWrapStyle FromInt(this int rawInt)
	{
		if (1 == 0)
		{
		}
		SsaWrapStyle result = rawInt switch
		{
			0 => SsaWrapStyle.Smart, 
			1 => SsaWrapStyle.EndOfLine, 
			2 => SsaWrapStyle.None, 
			3 => SsaWrapStyle.SmartWideLowerLine, 
			_ => SsaWrapStyle.None, 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
