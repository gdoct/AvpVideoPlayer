using AvpVideoPlayer.Uwp.Api;
using System;

namespace AvpVideoPlayer.Uwp.Configuration;

/// <inheritdoc cref="IUserConfiguration"/>
public class UserConfiguration : IUserConfiguration
{
    private readonly ISettingsStore _settingsstore;

    public UserConfiguration() : this(new UwpSettingsStore())
    {

    }

    public UserConfiguration(ISettingsStore settingsstore)
    {
        _settingsstore = settingsstore;
    }

    /// <inheritdoc cref="IUserConfiguration.LastPath"/>
    public string LastPath
    {
        get => GetSetting(Constants.SettingNames.LastPathSetting)?.ToString() ?? string.Empty;
        set => SetSetting(Constants.SettingNames.LastPathSetting, value);
    }

    public bool Repeat
    {
        get => GetSetting(Constants.SettingNames.RepeatSetting)?.ToString()?.Equals("True", StringComparison.OrdinalIgnoreCase) ?? false;
        set => SetSetting(Constants.SettingNames.RepeatSetting, value ? "True" : "False");
    }

    private string GetSetting(string key)
    {
        return _settingsstore.GetSetting(key);
    }

    private void SetSetting(string key, string value)
    {
        _settingsstore.SetSetting(key, value);
    }
}
