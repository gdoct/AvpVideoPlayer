using System.Globalization;
using System.Windows.Data;

namespace AvpVideoPlayer.Utility;

public class HeightWithOffsetConverter : IValueConverter
{
    public double Offset { get; set; } = 0;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!double.TryParse(value.ToString(), out var height))
        {
            return Offset;
        }
        return height + Offset;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
