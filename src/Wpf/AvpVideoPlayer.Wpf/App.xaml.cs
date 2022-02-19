using AvpVideoPlayer.Api;
using AvpVideoPlayer.Configuration;
using AvpVideoPlayer.EventHub;
using AvpVideoPlayer.Video;
using AvpVideoPlayer.ViewModels;
using AvpVideoPlayer.ViewModels.Events;
using AvpVideoPlayer.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace AvpVideoPlayer.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;
#if USE_EXTENSIBILITY
    private IPluginHost<IAvpPlugin>? _pluginhost;
#endif

    public App()
    {
        ServiceCollection services = new ();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
        var folder = (Environment.Is64BitProcess) ? @"ffmpeg\x64" : @"ffmpeg\x86";
        var binfolder = Path.Combine(AppContext.BaseDirectory, folder);
        FFMpegCore.GlobalFFOptions.Configure(options => options.BinaryFolder = binfolder);
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.RegisterViews()
            .AddTransient<IDialogService, DialogService>()
            .RegisterUserConfiguration()
            .RegisterEventHub()
            .RegisterSubtitles()
            .RegisterViewModels();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
#if USE_EXTENSIBILITY
        InitializePlugins();
#endif
        var mainwindow = _serviceProvider.GetService<MainWindow>();
        if (mainwindow is null) throw new TypeLoadException("type MainWindow not registered" );
        mainwindow.Show();
        var args = Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            var fi = new FileInfo(args[1]);
            if (!fi.Exists) return;
            var eventhub = _serviceProvider.GetService<IEventHub>();
            eventhub?.Publish(new SelectVideoEvent(fi.FullName));
            eventhub?.Publish(new PlayStateChangeRequestEvent(PlayStates.Play));
            mainwindow.WindowState = WindowState.Maximized;
            System.Threading.Thread.Sleep(10);
            eventhub?.Publish(new FullScreenEvent(true));
        }
    }

#if USE_EXTENSIBILITY
    private void InitializePlugins()
    {
        var eh = _serviceProvider.GetService<IEventHub>();
        if (eh is null) return;
        _pluginhost = new PluginHostBuilder<IAvpPlugin>(eh)
            .IncludePath(@"E:\Development\AvpVideoPlayer\src\Plugins\AvpVideoPlayer.Plugins.WebHost\bin\Debug\net6.0")
            .Build();
    }
#endif

    private void App_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        var dialogtext = "An unhandled exception occurred - application will exit." + Environment.NewLine + e.Exception;
        e.Handled = MessageBoxResult.Cancel ==
                MessageBox.Show(caption: $"Unhandled: {e.Exception.Message}",
                        messageBoxText: dialogtext,
                        button: MessageBoxButton.OKCancel,
                        icon: MessageBoxImage.Error,
                        defaultResult: MessageBoxResult.OK);
    }
}
