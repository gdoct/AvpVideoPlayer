using AvpVideoPlayer.Uwp.Video.Subtitles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AvpVideoPlayer.Uwp.Views
{
    public sealed partial class SubtitleBlock : UserControl
    {
        public SubtitleBlock()
        {
            this.InitializeComponent();
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
            if (line.Italic) run.FontStyle = Windows.UI.Text.FontStyle.Italic;
            if (line.Bold) run.FontWeight = Windows.UI.Text.FontWeights.Bold;
            if (line.Underline) run.TextDecorations = Windows.UI.Text.TextDecorations.Underline;
            return run;
        }
    }
}
