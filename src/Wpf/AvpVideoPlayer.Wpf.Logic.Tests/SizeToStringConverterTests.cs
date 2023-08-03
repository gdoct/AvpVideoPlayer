namespace AvpVideoPlayer.Utility.Tests;
#pragma warning disable CS8604 // Possible null reference argument.
using AvpVideoPlayer.Wpf.Logic;
using System;
using System.Globalization;
using Xunit;

public class SizeToStringConverterTests
{
    private readonly SizeToStringConverter _testClass;

    public SizeToStringConverterTests()
    {
        _testClass = new SizeToStringConverter();
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new SizeToStringConverter();
        Assert.NotNull(instance);
    }

    [Fact]
    public void CanCallConvert()
    {
        long value = 2;
        var targetType = typeof(long);
        var parameter = new object();
        var culture = CultureInfo.InvariantCulture;
        Assert.NotNull( _testClass.Convert(value, targetType, parameter, culture));
    }

    [Fact]
    public void CanCallConvertBack()
    {
        Assert.Throws<NotImplementedException>(() =>
        {
            var value = new object();
            var targetType = Type.GetType("TestValue372231955");
            var parameter = new object();
            var culture = CultureInfo.InvariantCulture;
            var result = _testClass.ConvertBack(value, targetType, parameter, culture);
        });
    }
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

    [Fact]
    public void CannotCallConvertBackWithNullValue()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(default, Type.GetType("TestValue989924959"), new object(), CultureInfo.CurrentCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullTargetType()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(new object(), default, new object(), CultureInfo.InvariantCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullParameter()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(new object(), Type.GetType("TestValue1960747760"), default, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullCulture()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(new object(), Type.GetType("TestValue1470055515"), new object(), default));
    }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
