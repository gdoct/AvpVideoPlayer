using AvpVideoPlayer.MetaData;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace AvpVideoPlayer.MetaData.Tests
{
    public class MetaDataServiceTests : IClassFixture<MetaDataContextFixture>
    {
        private readonly MetaDataContextFixture _fixture;

        public MetaDataServiceTests(MetaDataContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        public void GetMetadata_ThrowsOnInvalidPath(string path)
        {
            // Arrange
            var service = new MetaDataService(_fixture.Context);
            Assert.Throws<ArgumentException>(() => service.GetMetadata(path));
            Assert.Throws<ArgumentException>(() => service.GetMetadataForPath(path));
        }

        [Fact]
        public void GetMetadata_StateUnderTest_ExpectedBehavior()
        {
            var service = new MetaDataService(_fixture.Context);
            var fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var existing = service.GetMetadata(fi.FullName);
            Assert.NotNull(existing);
            Assert.Equal(fi.Length, existing.Length);
            Assert.Equal(fi.FullName, existing.FullName);
            Assert.Equal(fi.Name, existing.Name);
            Assert.Equal(fi.DirectoryName, existing.Path);
        }


        [Fact]
        public void GetMetadata_StateUnderTest_ExpectedBehavior2()
        {
            var service = new MetaDataService(_fixture.Context);
            var fi = new FileInfo($"C:\\temp\\{Guid.NewGuid()}\\text.txt");
            var meta = FileMetaData.Create(fi);
            service.SaveMetadata(meta);
            var existing = service.GetMetadata(fi.FullName);
            Assert.NotNull(existing);
            Assert.Equal(0, existing.Length);
            Assert.Equal(fi.FullName, existing.FullName);
            Assert.Equal(fi.Name, existing.Name);
            Assert.Equal(fi.DirectoryName, existing.Path);

        }

        //[Fact]
        //public void GetMetadata_NonExistingFile_ExpectedBehavior()
        //{
        //    var service = new MetaDataService(_fixture.Context);
        //    var fi = new FileInfo("c:\\temp\\doesnotexist.txt");
        //    var existing = service.GetMetadata("c:\\temp\\doesnotexist.txt");
        //    Assert.NotNull(existing);
        //    Assert.Equal(0, existing.Length);
        //    Assert.Equal(fi.FullName, existing.FullName);
        //    Assert.Equal(fi.Name, existing.Name);
        //    Assert.Equal(fi.DirectoryName, existing.Path);
        //}


        [Fact]
        public void SaveMetadata_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = new MetaDataService(_fixture.Context);
            var fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var dirname = fi.DirectoryName ?? "";
            var metaData = FileMetaData.Create(fi);

            Assert.True(_fixture.Context.GetMetadataForPath(dirname).Count == 0);
            Assert.True(service.GetMetadataForPath(dirname).Count == 0);
            // Act
            service.SaveMetadata(metaData);

            // Assert
            Assert.True(_fixture.Context.GetMetadataForPath(dirname).Count == 1);
            Assert.True(service.GetMetadataForPath(dirname).Count == 1);
        }
    }
}
