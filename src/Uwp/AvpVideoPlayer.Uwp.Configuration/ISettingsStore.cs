namespace AvpVideoPlayer.Uwp.Configuration;

public interface ISettingsStore
{
    string GetSetting(string key);
    void SetSetting(string key, string value);
}
