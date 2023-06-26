namespace AvpVideoPlayer.Api;

public class ChannelInfo
{
    public ChannelInfo(string id, string name, string logo, Uri uri, string group)
    {
        Id = id;
        Name = name;
        Logo = logo;
        Uri = uri;
        Group = group;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public Uri Uri { get; set; }
    public string Group { get; set; }
}
