using System;
using System.Windows;
using System.Windows.Controls;
using AvpVideoPlayer.Utility;

namespace AvpVideoPlayer.Wpf.Views;
    
public class CustomComboBox : ComboBox
{
    public DataTemplate SelectionBoxTemplate
    {
        get { return (DataTemplate)GetValue(SelectionBoxTemplateProperty); }
        set { SetValue(SelectionBoxTemplateProperty, value); }
    }

    public static readonly DependencyProperty SelectionBoxTemplateProperty = DependencyProperty.Register(
        "SelectionBoxTemplate",
        typeof(DataTemplate),
        typeof(CustomComboBox),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsArrange, (sender, e) =>
            {
                var comboBox = (CustomComboBox)sender;
                if (comboBox.selectionBoxHost == null) return;

                if (e.NewValue != null)
                {
                    // Kick in our own selection box template.
                    comboBox.selectionBoxHost.ContentTemplate = e.NewValue as DataTemplate;
                }
                else
                {
                    // Revert back to default selection box template.
                    comboBox.selectionBoxHost.ContentTemplate = comboBox.SelectionBoxItemTemplate;
                }
            }));

    private ContentPresenter? selectionBoxHost = null;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        var sselectionBoxHost = this.FindVisualChild<ContentPresenter>();
        if (sselectionBoxHost != null)
        {
            selectionBoxHost = sselectionBoxHost;
            selectionBoxHost.ContentTemplate = SelectionBoxTemplate;
        }
    }
}