using Avalonia.Controls;
using Avalonia.Interactivity;
using GameSavesBackup.Models;
using GameSavesBackup.ViewModels;

namespace GameSavesBackup.Views;

public enum PresetResult
{
    None,
    Saved,
    Deleted
}

public partial class PresetWindow : Window
{
    public PresetResult Result { get; private set; } = PresetResult.None;

    public PresetWindow()
    {
        InitializeComponent();
        DataContext = new PresetViewModel();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    public PresetWindow(string sourcePath, string targetPath)
    {
        InitializeComponent();
        DataContext = new PresetViewModel(sourcePath, targetPath);
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    public PresetWindow(BackupProfile profile)
    {
        InitializeComponent();
        DataContext = new PresetViewModel(profile);
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    private void SavePreset(object? sender, RoutedEventArgs e)
    {
        if (DataContext is PresetViewModel vm)
        {
            vm.SaveOrUpdate();
            Result = PresetResult.Saved;
        }

        this.Close();
    }

    private void DeletePreset(object? sender, RoutedEventArgs e)
    {
        if (DataContext is PresetViewModel vm)
        {
            vm.Delete();
            Result = PresetResult.Deleted;
        }

        this.Close();
    }

    private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
