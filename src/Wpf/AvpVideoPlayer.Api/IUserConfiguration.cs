namespace AvpVideoPlayer.Api;

/// <summary>
/// Provides access to the user settings
/// </summary>
public interface IUserConfiguration
{
    /// <summary>
    /// The last used path for browsing
    /// </summary>
    string LastPath { get; set; }

    /// <summary>
    /// Persis the repeat setting
    /// </summary>
    bool Repeat { get; set; }
}
