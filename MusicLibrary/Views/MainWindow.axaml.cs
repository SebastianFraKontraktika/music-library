using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace MusicLibrary.Views;

public partial class MainWindow : Window
{
    private readonly List<Button> _genreButtons = new();
    private readonly List<TextBox> _genreTextBox = new();
    public MainWindow()
    {
        InitializeComponent();
    }
    protected override void OnResized(WindowResizedEventArgs e)
    {
        base.OnResized(e);
        
        double newHeight = e.ClientSize.Height - Header.Bounds.Height;
        
        if (newHeight > 0)
            ScrollViewerLibrary.Height = newHeight;
    }
    
    private void AddSongButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = true;
        DeleteButton.IsVisible = true;
        NewButton.IsVisible = false;
        EditButton.IsVisible = false;
        
        var canvas = new Canvas
        {
            Height = 225,
            Width = 225,
            Background = Brushes.Green,
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
            Height = 100,
            Width = 100
        };
        var bitmap = new Bitmap(AssetLoader.Open(new Uri("avares://MusicLibrary/Assets/testBilde.png")));
        image.Source = bitmap;
        var genreButton = new Button
        {
            Content = "Genre",
            Background =  Brushes.White,
            Width = 30,
            IsDefault = true
        };
        genreButton.Click += genreButton_OnClick;

        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };

        var genreTextBox = new TextBox
        {
            Margin = new Thickness(5),
            PlaceholderText = "test"
        };
        
        Canvas.SetLeft(stackPanel, 55);
        Canvas.SetBottom(stackPanel, 5);
        Canvas.SetLeft(genreButton, 50);
        Canvas.SetBottom(image, 50);
        
        stackPanel.Children.Add(genreTextBox);
        stackPanel.Children.Add(genreButton);
        
        LibraryGrid.Children.Add(canvas);
        canvas.Children.Add(stackPanel);
        canvas.Children.Add(radiobutton);
        canvas.Children.Add(textblock);
        canvas.Children.Add(image);
        _genreButtons.Add(genreButton);
        _genreTextBox.Add(genreTextBox);
    }
    
    private void editSongButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = true;
        DeleteButton.IsVisible = true;
        NewButton.IsVisible = false;
        EditButton.IsVisible = false;
        
        var checkedButton = LibraryGrid.GetVisualDescendants()
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked == true);
        
        foreach (var button  in _genreButtons)
        {
            if (checkedButton != null && checkedButton.Parent is Canvas canvas && button.Parent?.Parent == canvas)
            {
                button.IsVisible = true;
            }
        }
        foreach (var textBox  in _genreTextBox)
        {
            if (checkedButton != null && checkedButton.Parent is Canvas canvas && textBox.Parent?.Parent == canvas)
            {
                textBox.IsVisible = true;
            }
        }
    }

    private void saveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = false;
        DeleteButton.IsVisible = false;
        NewButton.IsVisible = true;
        EditButton.IsVisible = true;
        
        foreach (var button  in _genreButtons)
        {
            button.IsVisible = false;
        }
        foreach (var textBox  in _genreTextBox)
        {
            textBox.IsVisible = false;
        }
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
        
        foreach (var button  in _genreButtons)
        {
            button.IsVisible = false;
        }
        foreach (var textBox  in _genreTextBox)
        {
            textBox.IsVisible = false;
        }
    }

    private void genreButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("Enter was pressed!");
    }
}