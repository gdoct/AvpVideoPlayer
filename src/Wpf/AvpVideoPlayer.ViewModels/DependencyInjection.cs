using AvpVideoPlayer.ViewModels.Controls;
using AvpVideoPlayer.ViewModels.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.ViewModels;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        return services
            .AddTransient<LibraryViewModel>()
            .AddTransient<FileListViewModel>()
            .AddTransient<FolderDropDownViewModel>()
            .AddTransient<MainWindowViewModel>()
            .AddTransient<PlayerControlsViewModel>()
            .AddTransient<SearchBoxViewModel>()
            .AddTransient<VideoPlayerViewModel>();
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IKeyboardController, KeyboardController>()
            .AddTransient<IM3UService, M3UService>();
    }
}
