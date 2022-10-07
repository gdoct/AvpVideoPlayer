using AvpVideoPlayer.Api;
using AvpVideoPlayer.Configuration;
using AvpVideoPlayer.EventHub;
using AvpVideoPlayer.MetaData;
using AvpVideoPlayer.Video;
using AvpVideoPlayer.ViewModels;
using AvpVideoPlayer.ViewModels.Events;
using AvpVideoPlayer.Wpf.Services;
using AvpVideoPlayer.Wpf.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;

namespace AvpVideoPlayer.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;
    private IDisposable? _showtageditorsub;
    private IDisposable? _showlibmanagersub;
#if USE_EXTENSIBILITY
    private IPluginHost<IAvpPlugin>? _pluginhost;
#endif

    public App()
    {
        ServiceCollection services = new();
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
            .RegisterMetadataServices()
            .RegisterViewModels();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _showtageditorsub?.Dispose();
        _showlibmanagersub?.Dispose();
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
#if USE_EXTENSIBILITY
        InitializePlugins();
#endif
        var mainwindow = _serviceProvider.GetService<MainWindow>();
        if (mainwindow is null) throw new TypeLoadException("type MainWindow not registered");
        mainwindow.Show();
        var eventhub = _serviceProvider.GetService<IEventHub>();
        if (eventhub != null && System.Threading.SynchronizationContext.Current != null)
        {
            _showtageditorsub = eventhub.Events
                                .OfType<ShowTagEditorEvent>()
                                .ObserveOn(System.Threading.SynchronizationContext.Current)
                                .Subscribe(ShowTagEditor);
            _showlibmanagersub = eventhub.Events
                                .OfType<ManageLibraryEvent>()
                                .ObserveOn(System.Threading.SynchronizationContext.Current)
                                .Subscribe(ShowLibraryManager);
        }
        var args = Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            var fi = new FileInfo(args[1]);
            if (!fi.Exists) return;
            eventhub?.Publish(new SelectVideoEvent(new VideoFileViewModel(fi)));
            eventhub?.Publish(new PlayStateChangeRequestEvent(PlayStates.Play));
            mainwindow.WindowState = WindowState.Maximized;
            System.Threading.Thread.Sleep(10);
            eventhub?.Publish(new FullScreenEvent(true));
        }
    }

    private void ShowLibraryManager(ManageLibraryEvent obj)
    {
        var metacontext = _serviceProvider.GetService<IMetaDataContext>();
        if (metacontext == null) return;
        var vm = new LibraryManagerViewModel(metacontext);
        var win = new ManageLibraryWindow()
        {
            DataContext = vm
        };
        win.ShowDialog();
    }

    private void ShowTagEditor(ShowTagEditorEvent obj)
    {
        var taggingsvc = _serviceProvider.GetService<ITaggingService>();
        var eventhub = _serviceProvider.GetService<IEventHub>();
        if (taggingsvc == null || eventhub == null) return;
        var win = new TagEditor
        {
            Width = 550,
            Height = 381,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        var dc = new TagEditorViewModel(win, eventhub, taggingsvc);
        win.DataContext = dc;
        win.ShowDialog();
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
