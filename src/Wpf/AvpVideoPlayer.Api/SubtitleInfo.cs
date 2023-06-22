namespace AvpVideoPlayer.Api;

/// <summary>
/// Describes a subtitle for a video
/// </summary>
public class SubtitleInfo
{
    /// <summary>
    /// Textual description of the subtitle
    /// </summary>
    public string SubtitleName { get; set; } = "";

    /// <summary>
    /// Stream ID of the subtitle (e.g. "0:3")
    /// </summary>
    public string StreamInfo { get; set; } = "";

    /// <summary>
    /// Flat index
    /// </summary>
    public int Index { get; set; }

    public string VideoFilename { get; set; } = "";
}