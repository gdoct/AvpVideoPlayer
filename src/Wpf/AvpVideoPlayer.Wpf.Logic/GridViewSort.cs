﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AvpVideoPlayer.Wpf.Logic;

/// <summary>
/// class to add sorting to the listview control
/// </summary>
public static class GridViewSort
{
    #region Attached properties

    public static ICommand GetCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(CommandProperty);
    }

    public static void SetCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(CommandProperty, value);
    }

    // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(GridViewSort),
            new UIPropertyMetadata(
                null,
                (o, e) =>
                {
                    if (o is ItemsControl listView && !GetAutoSort(listView)) // Don't change click handler if AutoSort enabled
                    {
                        if (e.OldValue != null && e.NewValue == null)
                        {
                            listView.RemoveHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                        if (e.OldValue == null && e.NewValue != null)
                        {
                            listView.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                    }
                }
            )
        );

    public static bool GetAutoSort(DependencyObject obj)
    {
        return (bool)obj.GetValue(AutoSortProperty);
    }

    public static void SetAutoSort(DependencyObject obj, bool value)
    {
        obj.SetValue(AutoSortProperty, value);
    }

    // Using a DependencyProperty as the backing store for AutoSort.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AutoSortProperty =
        DependencyProperty.RegisterAttached(
            "AutoSort",
            typeof(bool),
            typeof(GridViewSort),
            new UIPropertyMetadata(
                false,
                (o, e) =>
                {
                    if (o is ListView listView && GetCommand(listView) == null) // Don't change click handler if a command is set
                    {
                        bool oldValue = (bool)e.OldValue;
                        bool newValue = (bool)e.NewValue;
                        if (oldValue && !newValue)
                        {
                            listView.RemoveHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                        if (!oldValue && newValue)
                        {
                            listView.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        }
                    }
                }
            )
        );

    public static string GetPropertyName(DependencyObject obj)
    {
        return (string)obj.GetValue(PropertyNameProperty);
    }

    public static void SetPropertyName(DependencyObject obj, string value)
    {
        obj.SetValue(PropertyNameProperty, value);
    }

    // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PropertyNameProperty =
        DependencyProperty.RegisterAttached(
            "PropertyName",
            typeof(string),
            typeof(GridViewSort),
            new UIPropertyMetadata(null)
        );

    #endregion

    #region Column header click event handler

    private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
    {
        var headerClicked = e.OriginalSource as GridViewColumnHeader ?? throw new ArgumentException("");
        if (headerClicked?.Column != null)
        {
            string propertyName = GetPropertyName(headerClicked.Column);
            if (!string.IsNullOrEmpty(propertyName))
            {
                SortGridColumn(headerClicked, propertyName);
            }
        }
    }

    private static void SortGridColumn(GridViewColumnHeader headerClicked, string propertyName)
    {
        var listView = GetAncestor<ListView>(headerClicked);
        if (listView != null)
        {
            ICommand command = GetCommand(listView);
            if (command != null && command.CanExecute(propertyName))
            {
                command.Execute(propertyName);
            }
            else if (GetAutoSort(listView))
            {
                ApplySort(listView.Items, propertyName);
            }
        }
    }

    #endregion

    #region Helper methods

    public static T? GetAncestor<T>(DependencyObject reference) where T : DependencyObject
    {
        DependencyObject parent = VisualTreeHelper.GetParent(reference);
        while (parent is not T)
        {
            parent = VisualTreeHelper.GetParent(parent);
        }
        if (parent != null)
            return (T)parent;
        else
            return null;
    }

    public static void ApplySort(ICollectionView view, string propertyName)
    {
        ListSortDirection direction = ListSortDirection.Ascending;
        if (view.SortDescriptions.Count > 0)
        {
            SortDescription currentSort = view.SortDescriptions[0];
            if (currentSort.PropertyName == propertyName)
            {
                if (currentSort.Direction == ListSortDirection.Ascending)
                    direction = ListSortDirection.Descending;
                else
                    direction = ListSortDirection.Ascending;
            }
            view.SortDescriptions.Clear();
        }
        if (!string.IsNullOrEmpty(propertyName))
        {
            view.SortDescriptions.Add(new SortDescription(propertyName, direction));
        }
    }

    #endregion
}