using Windows.Storage;

namespace AvpVideoPlayer.Uwp.Configuration;

public class UwpSettingsStore : ISettingsStore
{
    public UwpSettingsStore()
    {
    }

    public string GetSetting(string key)
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        if (localSettings.Values.ContainsKey(key))
        {
            return localSettings.Values[key].ToString();
        }
        return string.Empty;
    }

    public void SetSetting(string key, string value)
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        localSettings.Values[key] = value;
    }
}
