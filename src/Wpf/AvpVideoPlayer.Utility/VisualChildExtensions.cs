using System.Windows;
using System.Windows.Media;

namespace AvpVideoPlayer.Utility;
public static class VisualChildExtensions
{
    public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj)
           where T : DependencyObject
    {
        if (depObj != null)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T t)
                {
                    yield return t;
                }
                if (child != null)
                foreach (T childOfChild in child.FindVisualChildren<T>())
                {
                    yield return childOfChild;
                }
            }
        }
    }

    public static childItem? FindVisualChild<childItem>(this DependencyObject obj)
        where childItem : DependencyObject
    {
        foreach (childItem child in obj.FindVisualChildren<childItem>())
        {
            return child;
        }

        return null;
    }
}