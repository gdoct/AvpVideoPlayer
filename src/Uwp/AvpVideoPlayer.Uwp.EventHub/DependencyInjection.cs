using AvpVideoPlayer.Uwp.Api;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.Uwp.EventHub;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection RegisterEventHub(this IServiceCollection services)
    {
        return services
            .AddSingleton<IEventHub, AvpVideoPlayer.Uwp.EventHub.EventHub>();
    }
}
