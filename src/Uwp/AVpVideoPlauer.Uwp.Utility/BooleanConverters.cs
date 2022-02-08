using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AvpVideoPlayer.Uwp.Utility;

public sealed class BoolToVisibilityConverter : BooleanConverter<Visibility> { public BoolToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { } }
public class BoolToStringConverter : BooleanConverter<string> { }
public class BoolToDoubleConverter : BooleanConverter<double> { }

/// <summary>
/// Generic boolean-to-T converter
/// </summary>
/// <typeparam name="T"></typeparam>
public class BooleanConverter<T> : IValueConverter
{
    public T ValueWhenFalse { get; set; }

    public T ValueWhenTrue { get; set; }

    public BooleanConverter()
    {

    }

    public BooleanConverter(T trueValue, T falseValue)
    {
        ValueWhenTrue = trueValue;
        ValueWhenFalse = falseValue;
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is bool boolean && boolean ? ValueWhenTrue : ValueWhenFalse;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is T t && EqualityComparer<T>.Default.Equals(t, ValueWhenTrue);
    }
}
