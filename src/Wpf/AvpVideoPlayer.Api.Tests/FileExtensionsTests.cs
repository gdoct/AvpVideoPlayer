namespace AvpVideoPlayer.Api.Tests;

using System;
using Xunit;
using System.IO;

public static class FileExtensionsTests
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    [Theory]
    [InlineData(".mkv")]
    [InlineData(".mpg")]
    [InlineData(".mp4")]
    [InlineData(".mpv")]
    [InlineData(".webm")]
    [InlineData(".264")]
    [InlineData(".mpeg")]
    [InlineData(".srt")]
    [InlineData(".ssa")]
    [InlineData(".vtt")]
    [InlineData(".tt")]
    [InlineData(".sv")]
    [InlineData(".mdvd")]
    public static void SupportedExtensionsAreValid(string value)
    {
        var fi = new FileInfo($"test.{value}");
        Assert.True(FileExtensions.IsValidFile(fi));
    }

    [Theory]
    [InlineData(".mkv")]
    [InlineData(".mpg")]
    [InlineData(".mp4")]
    [InlineData(".mpv")]
    [InlineData(".webm")]
    [InlineData(".264")]
    [InlineData(".mpeg")]
    public static void SupportedExtensionsAreVideo(string value)
    {
        var fi = new FileInfo($"test.{value}");
        Assert.True(FileExtensions.IsVideoFile(fi));
    }

    [Theory]
    [InlineData(".srt")]
    [InlineData(".ssa")]
    [InlineData(".vtt")]
    [InlineData(".tt")]
    [InlineData(".sv")]
    [InlineData(".mdvd")]
    public static void SupportedExtensionsAreSubtitle(string value)
    {
        var fi = new FileInfo($"test.{value}");
        Assert.True(FileExtensions.IsSubtitleFile(fi));
    }

    [Theory]
    [InlineData(".srt")]
    [InlineData(".ssa")]
    [InlineData(".vtt")]
    [InlineData(".tt")]
    [InlineData(".sv")]
    [InlineData(".mdvd")]
    public static void UnsupportedExtensionsAreNotVideo(string value)
    {
        var fi = new FileInfo($"test.{value}");
        Assert.False(FileExtensions.IsVideoFile(fi));
    }

    [Fact]
    public static void CannotCallIsValidFileWithNullFile()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        FileInfo fi = null;
        string st = null;
#pragma warning disable CS8604 // Possible null reference argument.
        _ = Assert.Throws<ArgumentNullException>(() => FileExtensions.IsValidFile(fi));
        _ = Assert.Throws<ArgumentNullException>(() => FileExtensions.IsValidFile(st));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

    [Fact]
    public static void CannotCallIsVideoFileWithNullFile()
    {
        Assert.Throws<ArgumentNullException>(() => FileExtensions.IsVideoFile(null));
    }

    [Fact]
    public static void CannotCallIsSubtitleFileWithNullFile()
    {
        Assert.Throws<ArgumentNullException>(() => FileExtensions.IsSubtitleFile(null));
    }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
