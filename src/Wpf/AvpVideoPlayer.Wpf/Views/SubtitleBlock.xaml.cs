using AvpVideoPlayer.Video.Subtitles;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AvpVideoPlayer.Wpf.Views;

/// <summary>
/// Interaction logic for SubtitleBlock.xaml
/// </summary>
public partial class SubtitleBlock : UserControl
{
    public SubtitleBlock()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty SubtitleDependencyProperty
        = DependencyProperty.Register("Subtitle",
                                      typeof(SubtitleData),
                                      typeof(SubtitleBlock),
                                      new PropertyMetadata(new SubtitleData(0, 0),
                                      new PropertyChangedCallback(OnSubtitleChanged)));
    public SubtitleData Subtitle
    {
        set => SetValue(SubtitleDependencyProperty, value);
        get => (SubtitleData)GetValue(SubtitleDependencyProperty);
    }

    private static void OnSubtitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not SubtitleBlock instance)
            return;
        instance.UpdateText(e);
    }

    private void UpdateText(DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not SubtitleData data) return;
        var sublabels = new[] { Subtitle1, Subtitle2, Subtitle3, Subtitle4, Subtitle5 };
        foreach (var sub in sublabels)
            sub.Text = "";

        foreach (var textblock in sublabels)
            foreach (var line in data.Lines)
            {
                var run = GetRun(line);
                textblock.Inlines.Add(run);
            }
    }

    private static Run GetRun(SubtitleData.SubtitleLine line)
    {
        var text = line.Text + " ";
        var run = new Run { Text = text };
        if (line.Italic) run.FontStyle = FontStyles.Italic;
        if (line.Bold) run.FontWeight = FontWeights.Bold;
        if (line.Underline) run.TextDecorations = TextDecorations.Underline;
        return run;
    }
}
