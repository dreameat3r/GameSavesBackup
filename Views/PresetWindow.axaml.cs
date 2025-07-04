using Avalonia.Controls;

namespace GameSavesBackup;

public partial class PresetWindow : Window
{
    public PresetWindow(string SourcePath, string TargetPath)
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        SourcePathTextBox.Text = SourcePath;
        TargetPathTextBox.Text = TargetPath;
    }
}