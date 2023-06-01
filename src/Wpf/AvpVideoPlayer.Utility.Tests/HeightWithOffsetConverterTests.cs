namespace AvpVideoPlayer.Utility.Tests;

using System;
using Xunit;
using System.Globalization;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.

public class HeightWithOffsetConverterTests
{
    private readonly HeightWithOffsetConverter _testClass;

    public HeightWithOffsetConverterTests()
    {
        _testClass = new HeightWithOffsetConverter();
    }

    [Fact]
    public void CanCallConvert()
    {
        var value = new object();
        var targetType = Type.GetType("TestValue262727588");
        var parameter = new object();
        var culture = CultureInfo.InvariantCulture;
        try
        {
            _ = _testClass.Convert(value, targetType, parameter, culture);
        }
        catch(Exception ex) 
        {
            Assert.Fail(ex.ToString());
            throw;
        }
    }

    [Fact]
    public void CannotCallConvertBackWithNullValue()
    {
        _ = Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(default, Type.GetType("TestValue661582023"), new object(), CultureInfo.CurrentCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullTargetType()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(new object(), default, new object(), CultureInfo.InvariantCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullParameter()
    {
        _ = Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(new object(), Type.GetType("TestValue1652838375"), default, CultureInfo.CurrentCulture));
    }

    [Fact]
    public void CannotCallConvertBackWithNullCulture()
    {
        Assert.Throws<NotImplementedException>(() => _testClass.ConvertBack(new object(), Type.GetType("TestValue122349774"), new object(), default));
    }

    [Fact]
    public void CanSetAndGetOffset()
    {
        var testValue = 574046130.24;
        _testClass.Offset = testValue;
        Assert.Equal(testValue, _testClass.Offset);
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8604 // Possible null reference argument.
