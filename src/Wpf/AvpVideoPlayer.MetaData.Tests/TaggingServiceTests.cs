namespace AvpVideoPlayer.MetaData.Tests
{
    using System;
    using Xunit;
    using AvpVideoPlayer.MetaData;
    using System.IO;

    public class TaggingServiceTests : IClassFixture<MetaDataContextFixture>
    {
        private readonly MetaDataContextFixture _fixture;

        public TaggingServiceTests(MetaDataContextFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public void TaggingService_BasicOperations()
        {
            var context = _fixture.Context;

            var taggingService = new TaggingService(context);
            taggingService.SetupTags();
            var tags = taggingService.GetTags();
            Assert.Equal(4, tags.Count);

            taggingService.Add("Hi");
            taggingService.Add("Hi2");
            taggingService.Add("Hi3");
            taggingService.Add("Hi4");
            tags = taggingService.GetTags();
            Assert.Equal(8, tags.Count);

            taggingService.Remove("Regular");
            tags = taggingService.GetTags();
            Assert.Equal(7, tags.Count);

            taggingService.Remove("Does not exist");
            tags = taggingService.GetTags();
            Assert.Equal(7, tags.Count);
        }
    }
}