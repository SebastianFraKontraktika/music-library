using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using MusicLibrary.Data;
using MusicLibrary.Models;

namespace MusicLibrary.Views;

public partial class MainWindow : Window
{
    private string _imagePath = string.Empty;
    
    private readonly List<Button> _genreButtons = new();
    private readonly List<TextBox> _genreTextBox = new();

    private readonly List<Viewbox> _artistView = new();
    private readonly List<Viewbox> _titleView = new();
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

    private async void filePickerButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(sender as Button);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Select Album Cover",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Images")
                    {
                        Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp" }
                    }
                }
            });

        if (files.Count > 0)
        {
            var imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            Directory.CreateDirectory(imageFolder);
            
            var filename = $"{DateTime.Now.Ticks}_{Path.GetFileName(files[0].Path.LocalPath)}";
            var destinationPath = Path.Combine(imageFolder, filename);
            
            File.Copy(files[0].Path.LocalPath, destinationPath, overwrite: true);
            
            _imagePath = destinationPath;
        }
    }
    
    private async void AddSongButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = true;
        DeleteButton.IsVisible = true;
        NewButton.IsVisible = false;
        EditButton.IsVisible = false;
        
        var canvas = new Canvas
        {
            Height = 225,
            Width = 225
        };
        var radiobutton = new RadioButton { GroupName = "RadSongs" };
        var image = new Image
        {
            Height = 125,
            Width = 125
        };
        var fileOpener = new Button
        {
            Content =  "Open File",
            Foreground =   Brushes.Black,
            Background =  Brushes.White,
        };
        fileOpener.Click += filePickerButton_OnClick; 
        
        // var bitmap = new Bitmap(AssetLoader.Open(new Uri("avares://MusicLibrary/Assets/testBilde.png")));
        // image.Source = bitmap;
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
        var artistView = new Viewbox
        {
            Stretch = Stretch.Uniform,
            StretchDirection = StretchDirection.DownOnly,
            Width = 175
        };
        var titleView = new Viewbox
        {
            Stretch = Stretch.Uniform,
            StretchDirection = StretchDirection.DownOnly,
            Width = 175
        };
        
        var titleTextBox = new TextBox
        {
            Foreground =  Brushes.White,
            PlaceholderText = "Album Title"
        };
        
        var artistTextBox = new TextBox
        {
            Foreground =  Brushes.White,
            PlaceholderText = "Album Artist",
        };
        artistTextBox.Classes.Add("nameInput");
        titleTextBox.Classes.Add("nameInput");
        
        Canvas.SetLeft(titleView, 30);
        Canvas.SetBottom(titleView, 80);
        Canvas.SetLeft(artistView, 30);
        Canvas.SetBottom(artistView, 60);
        Canvas.SetLeft(stackPanel, 55);
        Canvas.SetBottom(stackPanel, 5);
        Canvas.SetLeft(image, 50);
        Canvas.SetBottom(image, 100);
        Canvas.SetLeft(fileOpener, 50);
        Canvas.SetBottom(fileOpener, 100);
        
        artistView.Child = artistTextBox;
        titleView.Child = titleTextBox;
        
        stackPanel.Children.Add(genreTextBox);
        stackPanel.Children.Add(genreButton);
        
        LibraryGrid.Children.Add(canvas);
        canvas.Children.Add(stackPanel);
        canvas.Children.Add(radiobutton);
        canvas.Children.Add(image);
        canvas.Children.Add(titleView);
        canvas.Children.Add(artistView);
        canvas.Children.Add(fileOpener);
        _genreButtons.Add(genreButton);
        _genreTextBox.Add(genreTextBox);
        _artistView.Add(artistView);
        _titleView.Add(titleView);
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
            if (checkedButton?.Parent is Canvas canvas && button.Parent?.Parent == canvas)
            {
                button.IsVisible = true;
            }
        }
        foreach (var textBox  in _genreTextBox)
        {
            if (checkedButton?.Parent is Canvas canvas && textBox.Parent?.Parent == canvas)
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
        
        // change textBox to textBlock
        var titleTextBlock = new TextBlock
        {
            Foreground = Brushes.White
        };
        var artistTextBlock = new TextBlock
        {
            Foreground = Brushes.White
        };
        using (var context = new ApplicationDbContext())
        {
            foreach (var (artViewBox, titViewBox) in _artistView.Zip(_titleView))
            {
                if (artViewBox.Child is TextBox { Text: not null } artTextBox &&
                    titViewBox.Child is TextBox { Text: not null } titTextBox)
                {
                    var newMusic = new Music
                    {
                        AlbumName = titTextBox.Text,
                        ArtistName = artTextBox.Text,
                        Path = _imagePath
                    };
                    context.Musics.Add(newMusic);
                    context.SaveChanges();
                }
            }
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

        if (checkedButton?.Parent is Canvas canvas)
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