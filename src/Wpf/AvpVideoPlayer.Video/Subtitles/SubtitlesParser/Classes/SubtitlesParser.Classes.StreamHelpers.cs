// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.StreamHelpers
using System.IO;

namespace SubtitlesParser.Classes;

internal static class StreamHelpers
{
	public static Stream CopyStream(Stream inputStream)
	{
		MemoryStream memoryStream = new MemoryStream();
		int num;
		do
		{
			byte[] buffer = new byte[1024];
			num = inputStream.Read(buffer, 0, 1024);
			memoryStream.Write(buffer, 0, num);
		}
		while (inputStream.CanRead && num > 0);
		memoryStream.ToArray();
		return memoryStream;
	}
}
