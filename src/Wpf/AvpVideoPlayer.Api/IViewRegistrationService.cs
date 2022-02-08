namespace AvpVideoPlayer.Api;

public interface IViewRegistrationService
{
    bool Contains(string key);
    object GetInstance(string key);
    bool Register(string key, object instance);
}
