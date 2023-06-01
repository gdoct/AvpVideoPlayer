using AvpVideoPlayer.Api;
using AvpVideoPlayer.Video.Subtitles;
using Moq;
using Xunit;

namespace AvpVideoPlayer.Video.Tests.Subtitles
{
    public class SubtitleServiceTests
    {
        [Fact]
        public void AddSubtitlesFromFile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var factory = new Mock<ISubtitleContextFactory>();
            var context = new Mock<ISubtitleContext>();
            context.Setup(c=> c.SubtitleInfo).Returns(new SubtitleInfo { Filename=",z", Index=0, StreamInfo="x", SubtitleName="a" });
            factory.Setup(c => c.Empty()).Returns(() => context.Object);
            factory.Setup(c => c.FromFile(It.IsAny<string>())).Returns(() => new[] { context.Object });
            var service = new SubtitleService(factory.Object);
            var filename = "test.srt";
            // Act
            var result = service.AddSubtitlesFromFile(filename);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AddSubtitlesFromVideoFile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var factory = new Mock<ISubtitleContextFactory>();
            var context = new Mock<ISubtitleContext>();
            context.Setup(c => c.SubtitleInfo).Returns(new SubtitleInfo { Filename = ",z", Index = 0, StreamInfo = "x", SubtitleName = "a" });
            factory.Setup(c => c.Empty()).Returns(() => context.Object);
            factory.Setup(c => c.FromVideofile(It.IsAny<string>())).Returns(() => new[] { context.Object });
            var service = new SubtitleService(factory.Object);
            var filename = "test.mkv";
            // Act
            var result = service.AddSubtitlesFromFile(filename);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ClearSubtitles_StateUnderTest_ExpectedBehavior()
        {
            var factory = new Mock<ISubtitleContextFactory>();
            var context = new Mock<ISubtitleContext>();
            context.Setup(c => c.SubtitleInfo).Returns(new SubtitleInfo { Filename = ",z", Index = 0, StreamInfo = "x", SubtitleName = "a" });
            factory.Setup(c => c.Empty()).Returns(() => context.Object);
            factory.Setup(c => c.FromVideofile(It.IsAny<string>())).Returns(() => new[] { context.Object });
            var service = new SubtitleService(factory.Object);
            service.ClearSubtitles();
            Assert.Null(service.CurrentSubtitleInfo);
        }
    }
}
