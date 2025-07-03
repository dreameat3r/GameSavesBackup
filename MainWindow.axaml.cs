using Avalonia.Controls;

namespace GameSavesBackup;

public partial class MainWindow : Window
{

    public string Greeting => "Hello world!";
    public MainWindow()
    {
        InitializeComponent();

        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        Presets.ItemsSource = new string[]
        {"Elden Ring", "Dark Souls 2", "Project Zomboid", "Baldur's Gate 3", "Terraria"};
    }
}