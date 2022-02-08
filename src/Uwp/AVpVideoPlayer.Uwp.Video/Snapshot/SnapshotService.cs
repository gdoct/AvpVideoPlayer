//using System.Collections.Concurrent;
//using System.Drawing;
//using System.IO;
//using System.Runtime.InteropServices;
//using System.Windows;
//using System.Windows.Interop;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;

//namespace AvpVideoPlayer.Video.Snapshot;

//internal class SnapshotService : IDisposable, ISnapshotService
//{
//    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool DeleteObject([In] IntPtr hObject);

//    private readonly ISnapshotGenerator _snapshotGenerator;
//    private readonly ConcurrentBag<SnapshotData> _thumbnailData;
//    private bool disposedValue;
//    private CancellationTokenSource _cancellationTokenSource;
//    private Thread? _workerThread;
//    private ImageSource? _cache;
//    private TimeSpan? _cachetime;

//    public SnapshotService(ISnapshotGenerator snapshotGenerator)
//    {
//        _snapshotGenerator = snapshotGenerator;
//        _thumbnailData = new();
//        _cancellationTokenSource = new();
//    }

//    public ImageSource? GetBitmapForTime(TimeSpan time)
//    {
//        if (_cachetime != null)
//        {
//            if ((_cachetime.Value - time).Duration().TotalMilliseconds < 10000)
//            {
//                return _cache;
//            }
//        }
//        var data = _thumbnailData
//            .Where(s => s.Timestamp < time)
//            .OrderBy(s => time.TotalMilliseconds - s.Timestamp.TotalMilliseconds)
//            .FirstOrDefault();
//        if (data?.Bitmap == null) return null;
//        _cache = ImageSourceFromBitmap(data.Bitmap);
//        _cachetime = time;
//        return _cache;
//    }

//    public static ImageSource ImageSourceFromBitmap(Bitmap bmp)
//    {
//        var handle = bmp.GetHbitmap();
//        try
//        {
//            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
//        }
//        finally { DeleteObject(handle); }
//    }

//    public void LoadVideofile(string path)
//    {
//        // suppress "dead code" warning
//        bool isAsync = (DateTime.Today.Year > 2000);
//        if (isAsync)
//        {
//            if (_workerThread?.IsAlive == true)
//            {
//                _cancellationTokenSource.Cancel();
//                _cancellationTokenSource.Dispose();
//                _cancellationTokenSource = new();
//            }
//            foreach (var f in _thumbnailData) f?.Dispose();
//            _thumbnailData.Clear();
//            var token = _cancellationTokenSource.Token;
//            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
//            _workerThread = new Thread(() => GenerateThumbnailImpl(path, token));
//            _workerThread.Start();
//        }
//        else
//        {
//            _cancellationTokenSource = new();
//            foreach (var f in _thumbnailData) f?.Dispose();
//            _thumbnailData.Clear();
//            var token = _cancellationTokenSource.Token;
//            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
//            GenerateThumbnailImpl(path, token);
//        }
//    }

//    private void GenerateThumbnailImpl(string path, CancellationToken token)
//    {
//        try
//        {
//            foreach (var thumb in _snapshotGenerator.GenerateThumbnails(path, 30))
//            {
//                _thumbnailData.Add(thumb);
//                token.ThrowIfCancellationRequested();
//            }
//        }
//        catch (OperationCanceledException)
//        {
//            this.Log("************ downloading thumbnails was cancelled");
//        }
//    }


//    protected virtual void Dispose(bool disposing)
//    {
//        if (!disposedValue)
//        {
//            if (disposing)
//            {
//                foreach (var f in _thumbnailData) f?.Dispose();
//            }

//            disposedValue = true;
//        }
//    }

//    public void Dispose()
//    {
//        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//        Dispose(disposing: true);
//        GC.SuppressFinalize(this);
//    }
//}

//static class Logger
//{
//    public static void Log(this object _, string s)
//    {
//        System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:T}] {s}");
//    }
//}