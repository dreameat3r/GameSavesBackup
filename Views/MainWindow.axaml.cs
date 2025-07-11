using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Text.Json;
using GameSavesBackup.Models;
using GameSavesBackup.ViewModels;

namespace GameSavesBackup.Views;

public partial class MainWindow : Window
{
    private MainViewModel ViewModel => (MainViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        DataContext = new MainViewModel();
    }

    private async void SelectSourceFolder(object? sender, RoutedEventArgs e)
    {
        var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Выберите исходную папку",
            AllowMultiple = false
        });

        if (folders is { Count: > 0 })
            ViewModel.SourcePath = folders[0].Path.LocalPath;
    }

    private async void SelectTargetFolder(object? sender, RoutedEventArgs e)
    {
        var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Выберите папку назначения",
            AllowMultiple = false
        });

        if (folders is { Count: > 0 })
            ViewModel.TargetPath = folders[0].Path.LocalPath;
    }

    private async void OpenPresetEditor(object? sender, RoutedEventArgs e)
    {
        BackupProfile? profile = ViewModel.SelectedProfile;

        PresetWindow window;

        if (profile != null)
        {
            window = new PresetWindow(profile);
        }
        else
        {
            var sourcePath = ViewModel.SourcePath;
            var targetPath = ViewModel.TargetPath;

            window = new PresetWindow(sourcePath, targetPath);
        }

        await window.ShowDialog(this);

        if (window.Result is PresetResult.Saved or PresetResult.Deleted)
        {
            ViewModel.LoadProfiles();
        }
    }

    private void ClearFields(object? sender, RoutedEventArgs e)
    {
        ViewModel.SelectedProfile = null;
        ViewModel.SourcePath = "";
        ViewModel.TargetPath = "";
    }

    private void OpenSettings(object? sender, RoutedEventArgs e)
    {
        var window = new SettingsWindow();
        window.ShowDialog(this);
    }
}
