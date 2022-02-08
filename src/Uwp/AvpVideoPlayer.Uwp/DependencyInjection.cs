using AvpVideoPlayer.Uwp.Api;
using AvpVideoPlayer.Uwp.Utility;
using AvpVideoPlayer.Uwp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvpVideoPlayer.Uwp;

public static class DependencyInjection
{
    public static IServiceCollection RegisterViews(this IServiceCollection services)
    {
        return services.AddSingleton<MainPage>()
            //.AddSingleton<DialogBox>()
            .AddSingleton<IViewRegistrationService>(ViewRegistrationService.Instance);
        ;
    }

    public static ServiceProvider Services { get; set; }
}
