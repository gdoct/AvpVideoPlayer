// SubtitlesParser, Version=1.5.1.0, Culture=neutral, PublicKeyToken=null
// SubtitlesParser.Classes.SubtitleItem

namespace SubtitlesParser.Classes;

public class SubtitleItem
{
	public int StartTime { get; set; }

	public int EndTime { get; set; }

	public List<string> Lines { get; set; }

	public List<string> PlaintextLines { get; set; }

	public SubtitleItem()
	{
		Lines = new List<string>();
		PlaintextLines = new List<string>();
	}

	public override string ToString()
	{
		TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, StartTime);
		TimeSpan timeSpan2 = new TimeSpan(0, 0, 0, 0, EndTime);
		return string.Format("{0} --> {1}: {2}", timeSpan.ToString("G"), timeSpan2.ToString("G"), string.Join(Environment.NewLine, Lines));
	}
}
