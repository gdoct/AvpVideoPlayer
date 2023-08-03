namespace AvpVideoPlayer.Utility.Tests;

using AvpVideoPlayer.Wpf.Logic;
#pragma warning disable CS8604 // Possible null reference argument.
using System;
using System.Globalization;
using Xunit;
using T = System.String;

public class BoolToVisibilityConverterTests
{

    public BoolToVisibilityConverterTests()
    {
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new BoolToVisibilityConverter();
        Assert.NotNull(instance);
    }
}

public class BooleanConverter_Tests
{
    private readonly BooleanConverter<T> _testClass;
    private readonly T _trueValue;
    private readonly T _falseValue;

    public BooleanConverter_Tests()
    {
        _trueValue = "TestValue1741930528";
        _falseValue = "TestValue338835986";
        _testClass = new BooleanConverter<T>(_trueValue, _falseValue);
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new BooleanConverter<T>();
        Assert.NotNull(instance);
        instance = new BooleanConverter<T>(_trueValue, _falseValue);
        Assert.NotNull(instance);
    }

    [Fact]
    public void CanCallConvert()
    {
        var value = new object();
        var targetType = Type.GetType("TestValue230644121");
        var parameter = new object();
        var culture = CultureInfo.InvariantCulture;
        Assert.NotNull(_testClass.Convert(value, targetType, parameter, culture));
    }

    [Fact]
    public void CanCallConvertBack()
    {
        var value = new object();
        var targetType = Type.GetType("TestValue2109706206");
        var parameter = new object();
        var culture = CultureInfo.CurrentCulture;
        Assert.NotNull(_testClass.ConvertBack(value, targetType, parameter, culture));
    }

    [Fact]
    public void CanSetAndGetValueWhenFalse()
    {
        var testValue = "TestValue1100174755";
        _testClass.ValueWhenFalse = testValue;
        Assert.Equal(testValue, _testClass.ValueWhenFalse);
    }

    [Fact]
    public void CanSetAndGetValueWhenTrue()
    {
        var testValue = "TestValue536575635";
        _testClass.ValueWhenTrue = testValue;
        Assert.Equal(testValue, _testClass.ValueWhenTrue);
    }
}
#pragma warning restore CS8604 // Possible null reference argument.
