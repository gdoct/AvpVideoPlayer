using System;
using System.IO;
using Xunit;

namespace AvpVideoPlayer.MetaData.Tests
{
    public class MetaDataContextTests : IClassFixture<MetaDataContextFixture>
    {
        private readonly MetaDataContextFixture _fixture;

        public MetaDataContextTests(MetaDataContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ContainsKey_StateUnderTest_ExpectedBehavior()
        {
            var testfile = Path.GetTempFileName();
            File.WriteAllText(testfile, "Hello, world!");
            var fi = new FileInfo(testfile);
            Assert.NotNull(_fixture.Context);
            var context = _fixture.Context;
            var data = FileMetaData.Create(fi);
            context.SaveMetadata(data);
            var other = context.GetMetadata(fi);
            Assert.NotNull(other);
            Assert.Equal(data.Name, other?.Name);

            File.Delete(testfile);
        }

        [Fact]
        public void InsertIsUpdate_StateUnderTest_ExpectedBehavior()
        {
            var testfile = Path.GetTempFileName();
            File.WriteAllText(testfile, "Hello, world!");
            var fi = new FileInfo(testfile);
            var comparer = new FileMetaDataComparer();

            var context = _fixture.Context;

            var data = FileMetaData.Create(fi);
            data.Tags.Add("test");

            context.SaveMetadata(data);
            var other = context.GetMetadata(fi);
            Assert.NotNull(other);
            Assert.Equal(data, other, comparer);

            data.Tags.Add("test2");
            context.SaveMetadata(data);
            var other2 = context.GetMetadata(fi);
            Assert.Equal(data, other2, comparer);
            File.Delete(testfile);
        }

        [Fact]
        public void CanCallGetMetadataForPath()
        {
            var context = _fixture.Context;
            var result = context.GetMetadataForPath("random");
            Assert.Empty(result);
        }

        [Fact]
        public void CanGetMetadataForPath()
        {
            var context = _fixture.Context;
            context.SaveMetadata(FileMetaData.Create(new FileInfo(@"c:\test\test1\a.txt")));
            context.SaveMetadata(FileMetaData.Create(new FileInfo(@"c:\test\test1\b.txt")));
            context.SaveMetadata(FileMetaData.Create(new FileInfo(@"c:\test\test2\a.txt")));

            var result = context.GetMetadataForPath(@"c:\test\test1");
            Assert.Equal(2, result.Count);
            var result2 = context.GetMetadataForPath(@"c:\test\test2");
            Assert.Equal(1, result2.Count);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CannotCallGetMetadataForPathWithInvalidPath(string value)
        {
            Assert.Throws<ArgumentNullException>(() => _fixture.Context.GetMetadataForPath(value));
        }
    }
}
