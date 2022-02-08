using AvpVideoPlayer.Api;
using System.Globalization;
using System.Windows.Data;

namespace AvpVideoPlayer.Utility;

public class FiletypeToIconConverter : IValueConverter
{
    public FiletypeToIconConverter()
    {
    }

    public virtual object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not FileTypes tt) return string.Empty;
        return tt switch
        {
            FileTypes.Video => "/Images/video.png",
            FileTypes.Subtitles => "/Images/subs.png",
            _ => "/Images/folderyellow.png",
        };
    }

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
