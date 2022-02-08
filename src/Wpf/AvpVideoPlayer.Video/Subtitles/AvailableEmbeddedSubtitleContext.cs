using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.Video.Subtitles;

/// <inheritdoc cref="ISubtitleContext"/>
internal class AvailableEmbeddedSubtitleContext : ISubtitleContext
{
    private readonly object _syncRoot = new();

    public AvailableEmbeddedSubtitleContext(SubtitleInfo subtitleInfo)
    {
        SubtitleInfo = subtitleInfo;
    }

    public SubtitleInfo SubtitleInfo { get; }

    public ISubtitleContext? ActualContext { get; private set; } = null;

    /// <inheritdoc cref="ISubtitleContext.GetSubtitleForTime(TimeSpan)"/>
    public SubtitleData? GetNextSubtitleForTime(TimeSpan time)
    {
        return ActualContext?.GetNextSubtitleForTime(time);
    }

    /// <inheritdoc cref="ISubtitleContext.GetSubtitleForTime(TimeSpan)"/>
    public SubtitleData? GetSubtitleForTime(TimeSpan time)
    {
        return ActualContext?.GetSubtitleForTime(time);
    }

    public void Activate()
    {
        Task.Run(() =>
        {
            lock (_syncRoot)
            {
                if (ActualContext != null) return;
                var sub = EmbeddedSubtitleContext.ExtractEmbeddedSubtitle(SubtitleInfo);
                ActualContext = sub;
            }
        });
    }
}
