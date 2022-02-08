using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.Configuration;

/// <inheritdoc cref="IUserConfiguration"/>
public class UserConfiguration : IUserConfiguration
{
    private readonly ISettingsStore _settingsstore;

    public UserConfiguration() : this(new RegistrySettingsStore(Constants.SettingNames.Keyroot))
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

    private object? GetSetting(string key)
    {
        return _settingsstore.GetSetting(key);
    }

    private void SetSetting(string key, object value)
    {
        _settingsstore.SetSetting(key, value);
    }
}
