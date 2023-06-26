using AvpVideoPlayer.Api;
using AvpVideoPlayer.Wpf.Logic;
using AvpVideoPlayer.Wpf.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvpVideoPlayer.Wpf;

public static class DependencyInjection
{
    public static IServiceCollection RegisterViews(this IServiceCollection services)
    {
        return services.AddSingleton<MainWindow>()
                       .AddSingleton<DialogBox>()
                       .AddSingleton<IViewRegistrationService>(ViewRegistrationService.Instance)
                       .AddSingleton<IDispatcherHelper, DispatcherHelper>()
                       .AddSingleton<IIdleTimeDetector, IdleTimeDetector>();
    }
}
