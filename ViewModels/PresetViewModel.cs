using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using GameSavesBackup.Models;

namespace GameSavesBackup.ViewModels;

public class PresetViewModel : INotifyPropertyChanged
{
    private string _gameName = "";
    public string GameName
    {
        get => _gameName;
        set { _gameName = value; OnPropertyChanged(); }
    }

    public PresetViewModel(string sourcePath, string targetPath)
    {
        SourcePath = sourcePath;
        TargetPath = targetPath;
    }
    private string _sourcePath = "";
    public string SourcePath
    {
        get => _sourcePath;
        set { _sourcePath = value; OnPropertyChanged(); }
    }

    private string _targetPath = "";
    public string TargetPath
    {
        get => _targetPath;
        set { _targetPath = value; OnPropertyChanged(); }
    }

    public int? EditingId { get; private set; }

    public PresetViewModel() { }

    public PresetViewModel(BackupProfile profile)
    {
        EditingId = profile.Id;
        GameName = profile.GameName;
        SourcePath = profile.SourcePath;
        TargetPath = profile.TargetPath;
    }

    public void SaveOrUpdate()
    {
        var profiles = LoadProfiles();

        if (EditingId.HasValue)
        {
            var existing = profiles.FirstOrDefault(p => p.Id == EditingId.Value);
            if (existing != null)
            {
                existing.GameName = GameName;
                existing.SourcePath = SourcePath;
                existing.TargetPath = TargetPath;
            }
        }
        else
        {
            int nextId = profiles.Any() ? profiles.Max(p => p.Id) + 1 : 1;
            profiles.Add(new BackupProfile
            {
                Id = nextId,
                GameName = GameName,
                SourcePath = SourcePath,
                TargetPath = TargetPath
            });
        }

        SaveProfiles(profiles);
    }

    public void Delete()
    {
        if (!EditingId.HasValue) return;

        var profiles = LoadProfiles();
        var toDelete = profiles.FirstOrDefault(p => p.Id == EditingId.Value);
        if (toDelete != null)
        {
            profiles.Remove(toDelete);
            SaveProfiles(profiles);
        }
    }

    private List<BackupProfile> LoadProfiles()
    {
        if (!File.Exists("presets.json"))
            return new List<BackupProfile>();

        string json = File.ReadAllText("presets.json");
        return JsonSerializer.Deserialize<List<BackupProfile>>(json) ?? new();
    }

    private void SaveProfiles(List<BackupProfile> profiles)
    {
        var json = JsonSerializer.Serialize(profiles, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("presets.json", json);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
