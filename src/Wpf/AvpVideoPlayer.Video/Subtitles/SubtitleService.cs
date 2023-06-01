using AvpVideoPlayer.Api;
using System.IO;
using System.Windows;

namespace AvpVideoPlayer.Video.Subtitles;

public class SubtitleService
{
    private ISubtitleContext _subtitleContext;
    private readonly ISubtitleContextFactory _subtitleContextFactory;
    private IDictionary<SubtitleInfo, ISubtitleContext> LoadedSubtitles { get; set; } = new Dictionary<SubtitleInfo, ISubtitleContext>();
    private SubtitleData CurrentSubtitle { get; set; } = new SubtitleData(0, 0);

    public SubtitleService() : this(new SubtitleContextFactory())
    {

    }

    public SubtitleService(ISubtitleContextFactory subtitleContextFactory)
    {
        _subtitleContextFactory = subtitleContextFactory ?? throw new ArgumentNullException(nameof(subtitleContextFactory));
        _subtitleContext = subtitleContextFactory.Empty();
    }

    public SubtitleInfo? CurrentSubtitleInfo { get; private set; }

    public IEnumerable<SubtitleInfo> AvailableSubtitles => LoadedSubtitles.Keys;

    public bool IsSubtitleActive => _subtitleContext is not EmptySubtitleContext;

    /// <summary>
    /// Load subtitles from a supported file. This can be a video or subtitle file.
    /// </summary>
    /// <param name="filename"></param>
    public SubtitleInfo? AddSubtitlesFromFile(string? filename)
    {
        if (string.IsNullOrWhiteSpace(filename) 
            || LoadedSubtitles.Keys.Any(k => k.Filename == filename && k.Index == 0)
            || !File.Exists(filename))
        {
            return null;
        }
        var fi = new FileInfo(filename);
        CurrentSubtitle = new SubtitleData(0, 0);
        if (FileExtensions.IsVideoFile(fi))
        {
            return ExtractSubtitleInfoFromVideo(filename);
        }
        else
        {
            var subtitleContext = _subtitleContextFactory.FromFile(filename);
            var info = new SubtitleInfo
            {
                Filename = filename,
                SubtitleName = fi.Name,
                Index = 0,
                StreamInfo = string.Empty
            };
            LoadedSubtitles.Add(info, subtitleContext);
            return info;
        }
    }

    private SubtitleInfo? ExtractSubtitleInfoFromVideo(string filename)
    {
        SubtitleInfo? ret = null;
        var subs = _subtitleContextFactory.FromVideofile(filename);
        if (!subs.Any())
        {
            return null;
        }
        foreach (var sub in subs)
        {
            if (ret == null)
            {
                ret = sub.SubtitleInfo;
            }
            if (!LoadedSubtitles.ContainsKey(sub.SubtitleInfo))
            {
                LoadedSubtitles.Add(sub.SubtitleInfo, sub);
            }
        }
        return ret;
    }

    /// <summary>
    /// unload all subtitles
    /// </summary>
    public void ClearSubtitles()
    {
        _subtitleContext = _subtitleContextFactory.Empty();
        CurrentSubtitleInfo = null;
        LoadedSubtitles.Clear();
        CurrentSubtitle = new SubtitleData(0, 0);
    }

    /// <summary>
    /// Activate a loaded subtitle
    /// </summary>
    /// <param name="info"></param>
    public void ActivateSubtitle(SubtitleInfo? info)
    {
        if (info != null && LoadedSubtitles.ContainsKey(info))
        {
            var newcontext = LoadedSubtitles[info];
            if (newcontext is AvailableEmbeddedSubtitleContext avec)
            {
                avec.Activate();
            }
            _subtitleContext = newcontext;
            CurrentSubtitleInfo = info;
        }
        else
        {
            _subtitleContext = _subtitleContextFactory.Empty();
            CurrentSubtitleInfo = null;
        }
    }

    /// <summary>
    /// Get the subtitle from the loaded subtitle data to display for the given timespan
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public SubtitleData GetSubtitle(TimeSpan time)
    {
        if (CurrentSubtitle.EndTime < time.TotalMilliseconds || CurrentSubtitle.StartTime > time.TotalMilliseconds)
        {
            // current subtitle is invalid
            CurrentSubtitle = _subtitleContext.GetSubtitleForTime(time) ??
                              GetBlankUntilNextSubtitle(time);
        }
        return CurrentSubtitle;
    }

    /// <summary>
    /// Create a blank subtitle which lasts until the next subtitle needs to be shown
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private SubtitleData GetBlankUntilNextSubtitle(TimeSpan time)
    {
        var nextsub = _subtitleContext.GetNextSubtitleForTime(time);
        var currenttime = Convert.ToInt32(time.TotalMilliseconds);
        if (nextsub != null)
        {
            return new SubtitleData(currenttime, nextsub.StartTime);
        }
        else
        // just display a blank for 5 seconds.
        // if the user starts skipping the video it won't mess up the subtitles
        {
            return new SubtitleData(currenttime, currenttime + 5000);
        }
    }
}
