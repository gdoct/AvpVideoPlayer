using Xunit;

namespace AvpVideoPlayer.Utility.Tests
{
    public class M3uParserTests
    {
        [Fact]
        public void ParsePlaylist_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var m3uParser = new M3UParser("sample.m3u");

            // Act
            var result = m3uParser.ParsePlaylist();

            // Assert
            Assert.Equal(4, result.Count);
        }
    }
}
