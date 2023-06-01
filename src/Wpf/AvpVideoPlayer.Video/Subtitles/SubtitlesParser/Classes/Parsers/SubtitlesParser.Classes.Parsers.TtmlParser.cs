// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.TtmlParser
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SubtitlesParser.Classes.Parsers;

[ExcludeFromCodeCoverage]
public class TtmlParser : ISubtitlesParser
{
	public List<SubtitleItem> ParseStream(Stream xmlStream, Encoding encoding)
	{
		xmlStream.Position = 0L;
		List<SubtitleItem> list = new List<SubtitleItem>();
		XElement xElement = XElement.Load(xmlStream);
		XNamespace xNamespace = xElement.GetNamespaceOfPrefix("tt") ?? xElement.GetDefaultNamespace();
		List<XElement> list2 = xElement.Descendants(xNamespace + "p").ToList();
		foreach (XElement item2 in list2)
		{
			try
			{
				XmlReader xmlReader = item2.CreateReader();
				xmlReader.MoveToContent();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				string s = item2.Attribute("begin").Value.Replace("t", "");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				long num = ParseTimecode(s);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				string s2 = item2.Attribute("end").Value.Replace("t", "");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				long num2 = ParseTimecode(s2);
				string item = xmlReader.ReadInnerXml().Replace("<tt:", "<").Replace("</tt:", "</")
					.Replace($" xmlns:tt=\"{xNamespace}\"", "")
					.Replace($" xmlns=\"{xNamespace}\"", "");
				list.Add(new SubtitleItem
				{
					StartTime = (int)num,
					EndTime = (int)num2,
					Lines = new List<string> { item }
				});
			}
			catch (Exception arg)
			{
				Console.WriteLine("Exception raised when parsing xml node {0}: {1}", item2, arg);
			}
		}
		if (list.Any())
		{
			return list;
		}
		throw new ArgumentException("Stream is not in a valid TTML format, or represents empty subtitles");
	}

	private static long ParseTimecode(string s)
	{
		if (TimeSpan.TryParse(s, out var result))
		{
			return (long)result.TotalMilliseconds;
		}
		if (long.TryParse(s.TrimEnd('t'), out var result2))
		{
			return result2 / 10000;
		}
		return -1L;
	}
}
