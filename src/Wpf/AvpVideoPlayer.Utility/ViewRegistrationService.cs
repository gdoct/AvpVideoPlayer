using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.Utility;

public class ViewRegistrationService : IViewRegistrationService
{
    public static ViewRegistrationService Instance { get; } = new ViewRegistrationService();

    private readonly IDictionary<string, object> _register = new Dictionary<string, object>();

    public bool Register(string key, object instance)
    {
        if (instance is null) return false;
        if (Contains(key)) return false;
        _register.Add(key, instance);
        return true;
    }

    public bool Contains(string key)
    {
        return _register.ContainsKey(key);
    }

    public object GetInstance(string key)
    {
        if (!Contains(key)) throw new KeyNotFoundException();
        return _register[key];
    }
}
