using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GameSavesBackup;

public partial class PresetWindow : Window
{
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

    public void SavePreset(object sender, RoutedEventArgs args)
    {
        
    }

    public void DeletePreset(object sender, RoutedEventArgs args)
    {
        
    }
}