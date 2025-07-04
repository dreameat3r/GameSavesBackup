using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace GameSavesBackup;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        Presets.ItemsSource = new string[]
        {"Elden Ring", "Dark Souls 2", "Project Zomboid", "Baldur's Gate 3", "Terraria"};
    }

    public void OpenPresetEditor(object sender, RoutedEventArgs args)
    {
        var ownerWindow = this;
        var window = new PresetWindow();
        window.ShowDialog(ownerWindow);
    }

    public void OpenSettings(object sender, RoutedEventArgs args)
    {
        var ownerWindow = this;
        var window = new SettingsWindow();
        window.ShowDialog(ownerWindow);
    }

    private async void SelectSourceFolder(object? sender, RoutedEventArgs e)
    {
        var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Source Folder",
            AllowMultiple = false
        });

        if (folders != null  && folders.Count > 0)
        {
            var folder = folders[0];
            SourcePathTextBox.Text = folder.Path.LocalPath;
        }
    }

    private async void SelectTargetFolder(object? sender, RoutedEventArgs e)
    {
        var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Target Folder",
            AllowMultiple = false
        });

        if (folders != null && folders.Count > 0)
        {
            var folder = folders[0];
            TargetPathTextBox.Text = folder.Path.LocalPath;
        }
    }
}