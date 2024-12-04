using AvpVideoPlayer.Api;
using AvpVideoPlayer.Subtitles.SubtitlesParser.Classes;
using AvpVideoPlayer.Subtitles.SubtitlesParser.Classes.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvpVideoPlayer.Subtitles
{
    internal class StringSubtitleContext : ISubtitleContext
    {
        private readonly IList<SubtitleData> _subtitles;

        public StringSubtitleContext(string name, string contents)
        {
            _subtitles = ParseSubtitles(contents);
            SubtitleInfo = new SubtitleInfo
            {
                VideoFilename = name,
                SubtitleName = "default",
                Index = 0,
                StreamInfo = string.Empty
            };
        }

        public SubtitleInfo SubtitleInfo { get; }

        /// <inheritdoc cref="ISubtitleContext.GetSubtitleForTime(TimeSpan)"/>
        public SubtitleData? GetSubtitleForTime(TimeSpan time) => _subtitles.FirstOrDefault(c => c.StartTime <= time.TotalMilliseconds && c.EndTime >= time.TotalMilliseconds);

        /// <inheritdoc cref="ISubtitleContext.GetNextSubtitleForTime(TimeSpan)"/>
        public SubtitleData? GetNextSubtitleForTime(TimeSpan time) => _subtitles.FirstOrDefault(c => c.StartTime >= time.TotalMilliseconds);

        private static IList<SubtitleData> ParseSubtitles(string contents)
        {
            var sb = new StringBuilder(contents);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
            return new SubParser().ParseStream(stream)
                                  .OrderBy(s => s.StartTime)
                                  .Select(ConvertToSubtitleData)
                                  .ToList();
        }

        private static SubtitleData ConvertToSubtitleData(SubtitleItem subtitle)
        {
            return new SubtitleData
            (
                subtitle.StartTime,
                subtitle.EndTime,
                ConvertLines(subtitle.Lines).ToList()
            );
        }

        private static IEnumerable<SubtitleData.SubtitleLine> ConvertLines(List<string> lines) =>
            lines.Select(line => new SubtitleData.SubtitleLine
            {
                Italic = HasTag(line, "<i>"),
                Bold = HasTag(line, "<b>"),
                Underline = HasTag(line, "<u>"),
                Text = Regex.Replace(line.Trim(), "<.*?>", string.Empty, RegexOptions.None, TimeSpan.FromMilliseconds(250))
                                      .Replace(@"{\an7}", "")
                                      .Replace(@"\h", "")
            });

        private static bool HasTag(string line, string tag)
        {
            var closetag = "</" + tag[1..];
            if (line.Contains(tag) || line.Contains(closetag))
            {
                return true;
            }
            return false;
        }
    }
}
