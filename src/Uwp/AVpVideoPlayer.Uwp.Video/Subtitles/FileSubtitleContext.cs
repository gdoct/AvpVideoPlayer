using AvpVideoPlayer.Uwp.Api;
using System.Collections.Generic;
//using SubtitlesParser.Classes;
//using SubtitlesParser.Classes.Parsers;
using System.IO;
using System.Linq;
using System;
namespace AvpVideoPlayer.Uwp.Video.Subtitles;

/// <inheritdoc cref="ISubtitleContext"/>
internal class FileSubtitleContext : ISubtitleContext
{
    private readonly IList<SubtitleData> _subtitles = new List<SubtitleData>();

    public FileSubtitleContext(string filename)
    {
        _subtitles = LoadSubtitlesFromFile(filename);
        SubtitleInfo = new SubtitleInfo
        {
            Filename = filename,
            SubtitleName = "default",
            Index = 0,
            StreamInfo = string.Empty
        };
    }

    public SubtitleInfo SubtitleInfo { get; }

    /// <inheritdoc cref="ISubtitleContext.GetSubtitleForTime(TimeSpan)"/>
    public SubtitleData GetSubtitleForTime(TimeSpan time)
    {
        return _subtitles.FirstOrDefault(c => c.StartTime <= time.TotalMilliseconds && c.EndTime >= time.TotalMilliseconds);
    }

    /// <inheritdoc cref="ISubtitleContext.GetNextSubtitleForTime(TimeSpan)"/>
    public SubtitleData GetNextSubtitleForTime(TimeSpan time)
    {
        return _subtitles.FirstOrDefault(c => c.StartTime >= time.TotalMilliseconds);
    }

    private static IList<SubtitleData> LoadSubtitlesFromFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
        {
            return new List<SubtitleData>();
        }
        IList<SubtitleData> result = null;
        //using (var stream = File.OpenRead(filename))
        //{
        //    result = new SubParser().ParseStream(stream)
        //                            .OrderBy(s => s.StartTime)
        //                            .Select(ConvertToSubtitleData)
        //                            .ToList();
        //}

        return result;
    }

    //private static SubtitleData ConvertToSubtitleData(SubtitleItem subtitle)
    //{
    //    return new SubtitleData
    //    (
    //        subtitle.StartTime,
    //        subtitle.EndTime,
    //        ConvertLines(subtitle.Lines)
    //    );
    //}

    //private static List<SubtitleData.SubtitleLine> ConvertLines(List<string> lines)
    //{
    //    List<SubtitleData.SubtitleLine> result = new();
    //    foreach (string line in lines)
    //    {
    //        SubtitleData.SubtitleLine newline = new();
    //        string subline = line.Trim()
    //            .Replace(@"<font face=""Monospace"">", "")
    //            .Replace(@"{\an7}", "")
    //            .Replace(@"\h", "")
    //            .Replace(@"</font>", "");
    //        if (!string.IsNullOrWhiteSpace(subline))
    //        {
    //            newline.Italic = TryReplaceTag(ref subline, "<i>");
    //            newline.Bold = TryReplaceTag(ref subline, "<b>");
    //            newline.Underline = TryReplaceTag(ref subline, "<u>");
    //        }
    //        newline.Text = subline;
    //        result.Add(newline);
    //    }
    //    return result;
    //}

    //private static bool TryReplaceTag(ref string line, string tag)
    //{
    //    var closetag = "</" + tag.Substring(1);
    //    if (line.Contains(tag) || line.Contains(closetag))
    //    {
    //        line = line.Replace(tag, "").Replace(closetag, "");
    //        return true;
    //    }
    //    return false;
    //}
}
