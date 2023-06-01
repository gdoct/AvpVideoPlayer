// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.Parsers.YtXmlFormatParser
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace SubtitlesParser.Classes.Parsers;

[ExcludeFromCodeCoverage]
public class YtXmlFormatParser : ISubtitlesParser
{
	public List<SubtitleItem> ParseStream(Stream xmlStream, Encoding encoding)
	{
		xmlStream.Position = 0L;
		List<SubtitleItem> list = new List<SubtitleItem>();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(xmlStream);
		if (xmlDocument.DocumentElement != null)
		{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("//text");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			if (xmlNodeList != null)
			{
				for (int i = 0; i < xmlNodeList.Count; i++)
				{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
					XmlNode xmlNode = xmlNodeList[i];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
					try
					{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
						string value = xmlNode.Attributes["start"].Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
						float num = float.Parse(value, CultureInfo.InvariantCulture);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
						string value2 = xmlNode.Attributes["dur"].Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
						float num2 = float.Parse(value2, CultureInfo.InvariantCulture);
						string innerText = xmlNode.InnerText;
						list.Add(new SubtitleItem
						{
							StartTime = (int)(num * 1000f),
							EndTime = (int)((num + num2) * 1000f),
							Lines = new List<string> { innerText }
						});
					}
					catch (Exception arg)
					{
						Console.WriteLine("Exception raised when parsing xml node {0}: {1}", xmlNode, arg);
					}
				}
			}
		}
		if (list.Any())
		{
			return list;
		}
		throw new ArgumentException("Stream is not in a valid Youtube XML format");
	}
}
