using System;
using Windows.UI.Xaml.Data;

namespace AvpVideoPlayer.Uwp.Utility;

/// <summary>
/// convert file size in bytes to kb/mb/gb/...
/// </summary>
public class SizeToStringConverter : IValueConverter
{

    private static readonly string[] SizeSuffixes =
               { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
    public SizeToStringConverter()
    {
    }

    public virtual object Convert(object value, Type targetType, object parameter, string culture)
    {
        if (value == null) return string.Empty;
        var val = (long)value;
        if (val == 0) return string.Empty;
        var ret = SizeSuffix(val);
        return ret;
    }

    public virtual object ConvertBack(object value, Type targetType, object parameter, string culture)
    {
        throw new NotImplementedException();
    }

    private static string SizeSuffix(long value, int decimalPlaces = 1)
    {
        if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException(nameof(decimalPlaces)); }
        if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
        if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

        // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
        int mag = (int)Math.Log(value, 1024);

        // 1L << (mag * 10) == 2 ^ (10 * mag) 
        // [i.e. the number of bytes in the unit corresponding to mag]
        decimal adjustedSize = (decimal)value / (1L << (mag * 10));

        // make adjustment when the value is large enough that
        // it would round up to 1000 or more
        if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
        {
            mag += 1;
            adjustedSize /= 1024;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}",
            adjustedSize,
            SizeSuffixes[mag]);
    }
}
