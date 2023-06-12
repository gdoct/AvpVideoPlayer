using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.Subtitles;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection RegisterSubtitles(this IServiceCollection services)
    {
        return services
            .AddTransient<ISubtitleContextFactory, SubtitleContextFactory>()
            .AddTransient<SubtitleService>();
    }
}
