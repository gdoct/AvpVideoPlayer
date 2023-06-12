using AvpVideoPlayer.Api;
using AvpVideoPlayer.Subtitles;
using AvpVideoPlayer.Subtitles.Tests;
using Moq;

namespace AvpVideoPlayer.Video.Tests.Subtitles
{
    public class SubtitleServiceTests
    {
        private readonly SubtitleService _testClass;
        private readonly ISubtitleContextFactory _subtitleContextFactory;

        public SubtitleServiceTests()
        {
            _subtitleContextFactory = Mock.Of<ISubtitleContextFactory>();
            _testClass = new SubtitleService(_subtitleContextFactory);
        }

        [Fact]
        public void AddSubtitlesFromFile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var factory = new Mock<ISubtitleContextFactory>();
            var context = new Mock<ISubtitleContext>();
            context.Setup(c=> c.SubtitleInfo).Returns(new SubtitleInfo { Filename=",z", Index=0, StreamInfo="x", SubtitleName="a" });
            factory.Setup(c => c.Empty()).Returns(() => context.Object);
            factory.Setup(c => c.FromFile(It.IsAny<string>())).Returns(() => context.Object );
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

        [Fact]
        public void CanConstruct()
        {
            var instance = new SubtitleService(_subtitleContextFactory);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CannotConstructWithNullSubtitleContextFactory()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentNullException>(() => new SubtitleService(default));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        [Fact]
        public void CanCallAddSubtitlesFromFile()
        {
            var instance = new SubtitleService(_subtitleContextFactory);
            using (var sub = TempSubtitle.Create())
            {
                instance.AddSubtitlesFromFile(sub.Filename);
            }
            Assert.Single(instance.AvailableSubtitles);
        }

        [Fact]
        public void CanCallClearSubtitles()
        {
            var instance = new SubtitleService(_subtitleContextFactory);
            using (var sub = TempSubtitle.Create())
            {
                instance.AddSubtitlesFromFile(sub.Filename);
            }
            instance.ClearSubtitles();
            Assert.Empty(instance.AvailableSubtitles);
        }

        [Fact]
        public void CanCallActivateSubtitle()
        {
            var instance = new SubtitleService(_subtitleContextFactory);
            using (var sub = TempSubtitle.Create())
            {
                instance.AddSubtitlesFromFile(sub.Filename);
            }
            var info = instance.AvailableSubtitles.First();
            instance.ActivateSubtitle(info);
            Assert.True(instance.IsSubtitleActive);
        }

        [Fact]
        public void CanCallGetSubtitle()
        {
            var time = new TimeSpan(0, 0, 8);
            var instance = new SubtitleService(new SubtitleContextFactory());
            using (var sub = TempSubtitle.Create())
            {
                instance.AddSubtitlesFromFile(sub.Filename);
            }
            var info = instance.AvailableSubtitles.First();
            instance.ActivateSubtitle(info);
            var subt = instance.GetSubtitle(time);
            Assert.NotNull(subt);
            Assert.True(subt.ToString().Length > 5);
        }

        [Fact]
        public void CanGetIsSubtitleActive()
        {
            Assert.IsType<bool>(_testClass.IsSubtitleActive);
        }
    }
}
