using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

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
        NewButton.IsVisible = false;
        EditButton.IsVisible = false;
    }
    
    private void editSongButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = true;
        NewButton.IsVisible = false;
        EditButton.IsVisible = false;
    }

    private void saveButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveButton.IsVisible = false;
        NewButton.IsVisible = true;
        EditButton.IsVisible = true;
    }
}