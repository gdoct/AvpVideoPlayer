using AvpVideoPlayer.Uwp.Api;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.Uwp.Configuration;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection RegisterUserConfiguration(this IServiceCollection services)
    {
        return services
            .AddTransient<IUserConfiguration, UserConfiguration>()
            .AddSingleton<ISettingsStore>(new UwpSettingsStore());
    }
}
