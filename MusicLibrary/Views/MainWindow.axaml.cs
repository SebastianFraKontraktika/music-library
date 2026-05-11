using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Collections;
using System.Diagnostics;
using System.Linq;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace MusicLibrary.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void AddSongButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = true;
        DeleteButton.IsVisible = true;
        NewButton.IsVisible = false;
        EditButton.IsVisible = false;
        
        var canvas = new Canvas
        {
            Height = 150,
            Width = 150
        };
        var radiobutton = new RadioButton
        {
            GroupName = "RadSongs",
            Background = Brushes.Red
            
        };
        var textblock = new TextBlock
        {
            Text = "test" + Random.Shared.Next(3,1000)
        };
        var image = new Image
        {
            Height = 75,
            Width = 75
        };
        
        int gridCount = LibraryGrid.ColumnDefinitions.Count;
        LibraryGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        canvas[Grid.ColumnProperty] = gridCount;
        canvas.Children.Add(radiobutton);
        canvas.Children.Add(textblock);
        LibraryGrid.Children.Add(canvas);
    }
    
    private void editSongButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = true;
        DeleteButton.IsVisible = true;
        NewButton.IsVisible = false;
        EditButton.IsVisible = false;
    }

    private void saveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = false;
        DeleteButton.IsVisible = false;
        NewButton.IsVisible = true;
        EditButton.IsVisible = true;
    }

    private void deleteButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = false;
        DeleteButton.IsVisible = false;
        NewButton.IsVisible = true;
        EditButton.IsVisible = true;

        var checkedButton = LibraryGrid.GetVisualDescendants()
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked == true);

        if (checkedButton != null && checkedButton.Parent is Canvas canvas)
        {
            LibraryGrid.Children.Remove(canvas);
        }
    }
}