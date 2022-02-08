using AvpVideoPlayer.Uwp.Video.Subtitles;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.Uwp.Video;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection RegisterSubtitles(this IServiceCollection services)
    {
        return services
            .AddTransient<ISubtitleContextFactory, SubtitleContextFactory>()
            .AddTransient<SubtitleService>();
        //    .AddTransient<ISnapshotGenerator, SnapshotGenerator>()
        //    .AddTransient<ISnapshotService, SnapshotService>();
    }
}
