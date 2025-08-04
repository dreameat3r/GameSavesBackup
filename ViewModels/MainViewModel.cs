using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using GameSavesBackup.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSavesBackup.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<BackupProfile> Profiles { get; } = new();

    private BackupProfile? _selectedProfile;
    public BackupProfile? SelectedProfile
    {
        get => _selectedProfile;
        set
        {
            if (_selectedProfile == value) return;

            _selectedProfile = value;
            OnPropertyChanged();
            UpdatePathFromSelected();
        }
    }

    private string _sourcePath = "";
    public string SourcePath
    {
        get => _sourcePath;
        set
        {
            if (_sourcePath != value)
            {
                _sourcePath = value;
                OnPropertyChanged();
            }
        }
    }

    private string _targetPath = "";
    public string TargetPath
    {
        get => _targetPath;
        set
        {
            if (_targetPath != value)
            {
                _targetPath = value;
                OnPropertyChanged();
            }
        }
    }

    public MainViewModel()
    {
        LoadProfiles();
    }

    public void LoadProfiles()
    {
        Profiles.Clear();

        if (!File.Exists("presets.json"))
            return;

        var json = File.ReadAllText("presets.json");
        var list = JsonSerializer.Deserialize<List<BackupProfile>>(json);
        if (list != null)
        {
            foreach (var profile in list)
                Profiles.Add(profile);
        }
    }

    public BackupProfile? GetProfileByName(string? name)
    {
        return Profiles.FirstOrDefault(p => p.GameName == name);
    }

    private void UpdatePathFromSelected()
    {
        if (_selectedProfile is null)
        {
            SourcePath = "";
            TargetPath = "";
            return;
        }

        SourcePath = _selectedProfile.SourcePath;
        TargetPath = _selectedProfile.TargetPath;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? prop = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    public void Backup()
    {
        if (string.IsNullOrWhiteSpace(SourcePath) || string.IsNullOrWhiteSpace(TargetPath))
            return;

        if (!Directory.Exists(SourcePath))
            return;

        if (!Directory.Exists(TargetPath))
            return;

        CopyDirectory(SourcePath, TargetPath);
    }

    private void CopyDirectory(string sourceDir, string targetDir)
    {
        Directory.CreateDirectory(targetDir);

        var files = Directory.GetFiles(sourceDir);
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            var targetFile = Path.Combine(targetDir, fileName);
            File.Copy(file, targetDir, overwrite: true);
        }

        var directories = Directory.GetDirectories(sourceDir);
        foreach (var dir in directories)
        {
            var dirName = Path.GetFileName(dir);
            var targetSubDir = Path.Combine(targetDir, dirName);
            CopyDirectory(dir, targetSubDir);
        } 
    }
}
