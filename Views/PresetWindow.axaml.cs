using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Text.Json;
using System.Text.Json.Serialization;
using GameSavesBackup.Models;
using System.Collections.Generic;

namespace GameSavesBackup;

public partial class PresetWindow : Window
{
    public PresetWindow()
    {
        InitializeComponent();
    }

    public PresetWindow(string SourcePath, string TargetPath, string GameName)
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        SourcePathTextBox.Text = SourcePath;
        TargetPathTextBox.Text = TargetPath;
        GameNameTextBox.Text = GameName;
    }

    private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private List<BackupProfile> LoadProfilesToList()
    {
        string json = File.ReadAllText("presets.json");
        return JsonSerializer.Deserialize<List<BackupProfile>>(json) ?? new();
    }

    private void WriteProfileToJson(List<BackupProfile> profiles)
    {
        var json = JsonSerializer.Serialize(profiles, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("presets.json", json);
    }

    public void AddNewProfile()
    {
        var profiles = LoadProfilesToList();

        int NextId = profiles.Any() ? profiles.Max(p => p.Id) + 1 : 1;

        var newProfile = new BackupProfile
        {
            Id = NextId,
            GameName = GameNameTextBox.Text ?? " ",
            SourcePath = SourcePathTextBox.Text ?? " ",
            TargetPath = SourcePathTextBox.Text ?? ""
        };

        profiles.Add(newProfile);
        WriteProfileToJson(profiles);
    }

    public void SavePreset(object sender, RoutedEventArgs args)
    {
        AddNewProfile();
        this.Close();
    }
    public void DeletePreset(object sender, RoutedEventArgs args)
    {

    }
}