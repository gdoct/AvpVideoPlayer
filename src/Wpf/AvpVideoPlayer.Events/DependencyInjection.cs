using AvpVideoPlayer.Api;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.EventHub;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection RegisterEventHub(this IServiceCollection services)
    {
        return services
            .AddSingleton<IEventHub, EventHub>();
    }
}
