using Microsoft.Win32;

namespace AvpVideoPlayer.Configuration;

public class RegistrySettingsStore : ISettingsStore
{
    private readonly string _keyroot;

    public RegistrySettingsStore(string keyroot)
    {
        _keyroot = keyroot;
    }
    public object? GetSetting(string key)
    {
        var subkey = Registry.CurrentUser.CreateSubKey(_keyroot);
        var value = subkey?.GetValue(key ?? string.Empty);
        return value;
    }

    public void SetSetting(string key, object value)
    {
        var subkey = Registry.CurrentUser.CreateSubKey(_keyroot);
        subkey.SetValue(key ?? string.Empty, value ?? string.Empty);
    }
}
