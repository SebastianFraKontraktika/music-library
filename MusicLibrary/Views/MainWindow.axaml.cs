using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace MusicLibrary.Views;

public partial class MainWindow : Window
{
    private readonly List<Button> _genreButtons = new();
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
        var bitmap = new Bitmap(AssetLoader.Open(new Uri("avares://MusicLibrary/Assets/37cents.png")));
        image.Source = bitmap;
        var button = new Button
        {
            Background = Brushes.White,
            Content = "Genre",
            IsVisible = false
        };
        button.Click += genreButton_OnClick;

        var genreButton = new Button
        {
            Content = "Genre",
            Background =  Brushes.White,
            IsVisible = false
        };
        genreButton.Click += genreButton_OnClick;
        
        int gridCount = LibraryGrid.ColumnDefinitions.Count;
        LibraryGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        canvas[Grid.ColumnProperty] = gridCount;
        LibraryGrid.Children.Add(canvas);
        canvas.Children.Add(radiobutton);
        canvas.Children.Add(textblock);
        canvas.Children.Add(genreButton);
        canvas.Children.Add(image);
        _genreButtons.Add(genreButton);
    }
    
    private void editSongButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = true;
        DeleteButton.IsVisible = true;
        NewButton.IsVisible = false;
        EditButton.IsVisible = false;

        foreach (var button  in _genreButtons)
        {
            button.IsVisible = true;
        }
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

    private void genreButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Enter was pressed!");
    }
}