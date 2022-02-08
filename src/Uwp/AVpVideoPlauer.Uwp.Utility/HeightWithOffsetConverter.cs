using System;
using Windows.UI.Xaml.Data;

namespace AvpVideoPlayer.Uwp.Utility;

public class HeightWithOffsetConverter : IValueConverter
{
    public double Offset { get; set; } = 0;
    public object Convert(object value, Type targetType, object parameter, string culture)
    {
        if (!double.TryParse(value.ToString(), out var height))
        {
            return Offset;
        }
        return height + Offset;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string culture)
    {
        throw new NotImplementedException();
    }
}
