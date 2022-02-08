namespace AvpVideoPlayer.Configuration.Tests;

using AvpVideoPlayer.Configuration;
using Microsoft.Win32;
using Moq;
using System;
using Xunit;

public class UserConfigurationTests
{

    public UserConfigurationTests()
    {
    }

    [Fact]
    public void CanConstruct()
    {
        var registryStore = new Mock<ISettingsStore>();
        var instance = new UserConfiguration(registryStore.Object);
        Assert.NotNull(instance);
        var instance2 = new UserConfiguration();
        Assert.NotNull(instance2);
    }

    [Fact]
    public void CanSetAndGetLastPath()
    {
        var testValue = "TestValue1755087316";
        var registryStore = new Mock<ISettingsStore>();
        registryStore.Setup(r => r.SetSetting("LastPath", testValue));
        var instance = new UserConfiguration(registryStore.Object)
        {
            LastPath = testValue
        };
        registryStore.VerifyAll();
    }


    [Fact]
    public void NullLastPathReturnsEmptyString()
    {
        var registryStore = new Mock<ISettingsStore>();
        registryStore.Setup(r => r.GetSetting("LastPath")).Returns(null);
        var instance = new UserConfiguration(registryStore.Object);
        var s = instance.LastPath;
        Assert.Equal(s, string.Empty);
        registryStore.VerifyAll();
    }


    [Fact]
    public void LastPathReturnsString()
    {
        var registryStore = new Mock<ISettingsStore>();
        registryStore.Setup(r => r.GetSetting("LastPath")).Returns("test");
        var instance = new UserConfiguration(registryStore.Object);
        var s = instance.LastPath;
        Assert.Equal("test", s);
        registryStore.VerifyAll();
    }
}
