namespace AvpVideoPlayer.Subtitles.Tests;

public class EmbeddedSubtitleContextTests
{

    [Fact]
    public void ExtractEmbeddedSubtitle_StateUnderTest_ExpectedBehavior()
    {
        // Act\
        var filename = "test.mkv";
        var infos = EmbeddedSubtitleContext.ListEmbeddedSubtitles(filename);
        var result = EmbeddedSubtitleContext.ExtractEmbeddedSubtitle(infos.First());

        // Assert
        Assert.NotNull(result);
    }
}
