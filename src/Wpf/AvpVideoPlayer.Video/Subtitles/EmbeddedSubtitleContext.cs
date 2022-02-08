using AvpVideoPlayer.Api;
using FFMpegCore;
using System.IO;

namespace AvpVideoPlayer.Video.Subtitles;

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

        FFMpegArguments
                  .FromFileInput(subtitleInfo.Filename)
                  .OutputToFile(tempfile, true, options =>
                      options.WithCustomArgument($"-map {subtitleInfo.StreamInfo}"))
                  .ProcessSynchronously();


        if (!File.Exists(tempfile)) return null;
        var subs = new EmbeddedSubtitleContext(tempfile);
        File.Delete(tempfile);
        return subs;
    }


    public static IEnumerable<SubtitleInfo> ListEmbeddedSubtitles(string videofile)
    {
        if (!File.Exists(videofile)) yield break;
        var contents = FFProbe.Analyse(videofile);
        if (!contents.SubtitleStreams.Any())
            yield break;
        foreach (var stream in contents.SubtitleStreams)
        {
            yield return new SubtitleInfo
            {
                Filename = videofile,
                Index = stream.Index,
                SubtitleName = $"{stream.Language}",
                StreamInfo = $"0:{stream.Index}"
            };
        }
    }
}

