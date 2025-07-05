using System;
using System.Text.Json;

namespace GameSavesBackup.Models;

public class BackupProfile
{
    public required int Id {get; set;}
    public required string GameName { get; set; }
    public required string SourcePath { get; set; } 
    public required string TargetPath { get; set; }
    
    
    public void Backup()
    {

    }
}