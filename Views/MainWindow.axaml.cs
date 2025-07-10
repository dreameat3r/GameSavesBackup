using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Text.Json;
using GameSavesBackup.Models;
using System.Collections.Generic;
using Tmds.DBus.Protocol;

namespace GameSavesBackup;

public partial class MainWindow : Window
{
    private List<BackupProfile> profiles;

    public MainWindow()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        profiles = LoadProfilesToList();
        foreach (var profile in profiles)
        {
            Presets.Items.Add(profile.GameName);
        }
    }

    private List<BackupProfile> LoadProfilesToList()
    {
        string json = File.ReadAllText("presets.json");
        return JsonSerializer.Deserialize<List<BackupProfile>>(json) ?? new();
    }

    private void PresetsSelection(object? sender, SelectionChangedEventArgs e)
    {
        var selectedItem = Presets.SelectedItem;
        if (selectedItem == null) return;

        var profile = profiles.Find(p => p.GameName == (string)selectedItem);

        if (profile != null)
        {
            SourcePathTextBox.Text = profile.SourcePath;
            TargetPathTextBox.Text = profile.TargetPath;
        }
    }

    public void OpenPresetEditor(object sender, RoutedEventArgs args)
    {
        var SourcePath = SourcePathTextBox.Text ?? "";
        var TargetPath = TargetPathTextBox.Text ?? "";
        var GameName = Presets.SelectedItem ?? " ";

        var ownerWindow = this;
        var window = new PresetWindow(SourcePath, TargetPath, (string)GameName);
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

        if (folders != null && folders.Count > 0)
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

