namespace AvpVideoPlayer.Installer.GenerateWix;

public class InstallerContent
{
    public string FeatureSection { get; }
    public string ContentsSection { get; }

    public InstallerContent(string featureSection, string contentsSection)
    {
        FeatureSection = featureSection;
        ContentsSection = contentsSection;
    }
}