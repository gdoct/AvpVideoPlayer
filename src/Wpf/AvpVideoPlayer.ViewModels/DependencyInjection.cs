using AvpVideoPlayer.Api;
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
            .AddTransient<VideoPlayerViewModel>()
            .AddSingleton<IKeyboardController, KeyboardController>();
    }
}
