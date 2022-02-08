namespace AvpVideoPlayer.Configuration.Tests;

using AvpVideoPlayer.Configuration;
using Microsoft.Win32;
using System;
using Xunit;

public class RegistrySettingsStoreTests : IDisposable
{
    private readonly RegistrySettingsStore registryStore;
    private bool disposedValue;
    private readonly string _testkey;
    public RegistrySettingsStoreTests()
    {
        _testkey = $@"Software\Drdata.nl\VideoLibraryManager\Test_{Guid.NewGuid}";
        registryStore = new RegistrySettingsStore(_testkey) { };
    }

    [Fact]
    public void CanCallGetSetting()
    {
        var key = "TestValue303520938";
        var result = registryStore.GetSetting(key);
        Assert.Null(result);
    }

    [Fact]
    public void CanCallSetSetting()
    {
        var key = "TestValue378638215";
        var value = "hello, world";
        registryStore.SetSetting(key, value);
        var result = registryStore.GetSetting(key);
        Assert.Equal(value, result);
    }

    [Fact]
    public void CanCallInvalidSetting()
    {
        var key = @"Test\Value378*638215";
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        registryStore.SetSetting(key, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        var result = registryStore.GetSetting(key);
        Assert.Equal(string.Empty, result);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Registry.CurrentUser.DeleteSubKey(_testkey);
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~RegistrySettingsStoreTests()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
