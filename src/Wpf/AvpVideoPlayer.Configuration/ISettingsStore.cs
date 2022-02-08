namespace AvpVideoPlayer.Configuration;

public interface ISettingsStore
{
    object? GetSetting(string key);
    void SetSetting(string key, object value);
}
