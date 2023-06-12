using AvpVideoPlayer.Api;
using System.Xml.Xsl;

namespace AvpVideoPlayer.Subtitles;

/// <inheritdoc cref="ISubtitleContext"/>
internal class EmbeddedSubtitleContext : FileSubtitleContext
{
    private EmbeddedSubtitleContext(string filename) : base(filename)
    {
    }

    public static ISubtitleContext? ExtractEmbeddedSubtitle(SubtitleInfo subtitleInfo)
    {
        // call ffmpeg to extract specified subtitle to temp file
        // run ffmpeg -i inputfile -map 0:3 out.srt
        var tempfile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.srt");

// .. extract subtitles to tempfile

        var file = TagLib.File.Create(tempfile);
        if (file.Tag is not TagLib.Matroska.Tag mkvtag)
        {
            return null;
        }

        if (!File.Exists(tempfile)) return null;
        var subs = new EmbeddedSubtitleContext(tempfile);
        File.Delete(tempfile);
        return subs;
    }


    public static IEnumerable<SubtitleInfo> ListEmbeddedSubtitles(string videofile)
    {
        if (!File.Exists(videofile)) yield break;
        throw new NotImplementedException();
        dynamic FFProbe = new System.Dynamic.ExpandoObject();
        var contents = FFProbe.Analyse(videofile);
        if (!contents.SubtitleStreams.Any())
            yield break;
        foreach (var stream in contents.SubtitleStreams)
        {
            yield return new SubtitleInfo
            {
                Filename = videofile,
                Index = stream.Index,
                SubtitleName = $"{stream.CodecName} ({stream.Language})",
                StreamInfo = $"0:{stream.Index}"
            };
        }
    }



}

