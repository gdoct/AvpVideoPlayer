using AvpVideoPlayer.Api;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.Configuration;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection RegisterUserConfiguration(this IServiceCollection services)
    {
        return services
            .AddTransient<IUserConfiguration, UserConfiguration>()
            .AddSingleton<ISettingsStore>(new RegistrySettingsStore(Constants.SettingNames.Keyroot));
    }
}
