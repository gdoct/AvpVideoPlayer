namespace AvpVideoPlayer.Subtitles.Tests;

public class EmbeddedSubtitleContextTests
{

    [Fact]
    public void ExtractEmbeddedSubtitle_StateUnderTest_ExpectedBehavior()
    {
        // Act\
        var filename = @"C:\Download\Shared\Margin.Call.2011.1080p.BluRay.H264.AAC-RARBG\Margin.Call.2011.1080p.BluRay.H264.AAC-RARBG.mp4";
//        var filename = "test.mkv";
        var infos = EmbeddedSubtitleContext.ListEmbeddedSubtitles(filename);
        var result = EmbeddedSubtitleContext.ExtractEmbeddedSubtitle(infos.First());

        // Assert
        Assert.NotNull(result);
    }
}
