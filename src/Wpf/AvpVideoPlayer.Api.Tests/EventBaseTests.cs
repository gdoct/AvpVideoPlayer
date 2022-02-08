namespace AvpVideoPlayer.Api.Tests;

using Xunit;
using AvpVideoPlayer.Api;
using System;

public class EventBaseTests
{
    private class TestEventBase : EventBase
    {
    }

    private class TestEventBase<T> : EventBase<T>
    {
        public TestEventBase(T data) : base(data)
        {

        }
    }

    class NullableTestClass
    {
        public override string? ToString()
        {
            return null;
        }
    }

    private readonly TestEventBase _testClass;

    public EventBaseTests()
    {
        _testClass = new TestEventBase();
    }

    [Fact]
    public void ToStringTruncates()
    {
        var evt = new TestEventBase<string>("1234567890123456789012345678901234567890");
        var result = evt.ToString();
        Assert.Equal("TestEventBase`1 => 12345678901234567890", result);
    }

    [Fact]
    public void ToStringTruncatesOnlyOver20Chars()
    {
        var evt = new TestEventBase<string>("1234567890123456789");
        var result = evt.ToString();
        Assert.Equal("TestEventBase`1 => 1234567890123456789", result);
    }


    [Fact]
    public void ToStringHandlesNull()
    {
        var evt = new TestEventBase<NullableTestClass>(new NullableTestClass());
        var result = evt.ToString();
        Assert.Equal("TestEventBase`1 => <empty>", result);
    }

    [Fact]
    public void ToStringHandlesInt()
    {
        var evt = new TestEventBase<int>(33);
        var result = evt.ToString();
        Assert.Equal("TestEventBase`1 => 33", result);
    }

    [Fact]
    public void CanCallToString()
    {
        Assert.NotNull(_testClass.ToString());
    }

    [Fact]
    public void NullConstructThrows()
    {
        string? s = null;
#pragma warning disable CS8604 // Possible null reference argument.
        Assert.Throws<ArgumentNullException>(() => new TestEventBase<string>(s));
#pragma warning restore CS8604 // Possible null reference argument.
    }
}
