namespace AvpVideoPlayer.Utility.Tests;

using AvpVideoPlayer.Wpf.Logic;
using System;
using System.Globalization;
using Xunit;
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

public class FiletypeToIconConverterTests
{
    private readonly FiletypeToIconConverter _testClass;

    public FiletypeToIconConverterTests()
    {
        _testClass = new FiletypeToIconConverter();
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new FiletypeToIconConverter();
        Assert.NotNull(instance);
    }

    [Fact]
    public void CanCallConvert()
    {
        var value = new object();
        var targetType = Type.GetType("TestValue267378840");
        var parameter = new object();
        var culture = CultureInfo.InvariantCulture;
        Assert.NotNull(_testClass.Convert(value, targetType, parameter, culture));
    }

    [Fact]
    public void CanNotCallConvertBack()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack("test", typeof(string), "yo", CultureInfo.CurrentCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullValue()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(default, Type.GetType("TestValue1011568605"), new object(), CultureInfo.CurrentCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullTargetType()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(new object(), default, new object(), CultureInfo.InvariantCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullParameter()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(new object(), Type.GetType("TestValue1327763569"), default, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullCulture()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(new object(), Type.GetType("TestValue361797947"), new object(), default));
    }
}
