using System;
using System.Collections.Generic;
using System.Linq;

namespace AvpVideoPlayer.Uwp.Video.Subtitles;

public class SubtitleData
{
    public SubtitleData(int starttime, int endtime) : this(starttime, endtime, new List<SubtitleLine>())
    {

    }

    public SubtitleData(int starttime, int endtime, IList<SubtitleLine> lines)
    {
        StartTime = starttime;
        EndTime = endtime;
        Lines = lines;
    }

    public int StartTime { get; private set; }
    public int EndTime { get; private set; }
    public IList<SubtitleLine> Lines { get; private set; }

    public class SubtitleLine
    {
        public string Text { get; set; } = string.Empty;
        public bool Italic { get; set; }
        public bool Bold { get; set; }
        public bool Underline { get; set; }
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, Lines.Select(l => l.Text));
    }
}