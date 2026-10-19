using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using DiscordRPC;
using System.IO;
using System.Text.Json;

namespace DiscordRpcApp
{
    public class AppSettings
    {
        public string? ClientId { get; set; }
        public string? Details { get; set; }
        public string? LargeImage { get; set; }
        public string? LargeText { get; set; }
    }

    public partial class MainWindow : Window
    {
        private DiscordRpcClient? client;
        private readonly string configPath = "config.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (File.Exists(configPath))
            {
                try
                {
                    string json = File.ReadAllText(configPath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    if (settings != null)
                    {
                        TxtClientId.Text = settings.ClientId ?? "";
                        TxtDetails.Text = settings.Details ?? "";
                        TxtLargeImage.Text = settings.LargeImage ?? "";
                        TxtLargeText.Text = settings.LargeText ?? "";
                    }
                }
                catch
                {
                    // Ignore corrupted config files
                }
            }
        }

        private void SaveSettings()
        {
            var settings = new AppSettings
            {
                ClientId = TxtClientId.Text,
                Details = TxtDetails.Text,
                LargeImage = TxtLargeImage.Text,
                LargeText = TxtLargeText.Text
            };

            try
            {
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configPath, json);
            }
            catch
            {
                // Ignore save errors
            }
        }

        private void BtnUpdate_Click(object? sender, RoutedEventArgs e)
        {
            string clientId = TxtClientId.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(clientId))
            {
                UpdateStatusUI("Error: Client ID required", "#F23F43");
                return;
            }

            SaveSettings();

            if (client == null || client.ApplicationID != clientId)
            {
                client?.Dispose();
                client = new DiscordRpcClient(clientId);
                client.Initialize();
            }

            client.SetPresence(new RichPresence()
            {
                Details = TxtDetails.Text,
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    LargeImageKey = TxtLargeImage.Text,
                    LargeImageText = TxtLargeText.Text
                }
            });

            UpdateStatusUI("Rich Presence Active", "#23A55A");
        }

        private void BtnStop_Click(object? sender, RoutedEventArgs e)
        {
            if (client != null && !client.IsDisposed)
            {
                client.ClearPresence();
                client.Dispose();
                client = null;
            }

            UpdateStatusUI("Disconnected", "#F23F43");
        }

        private void UpdateStatusUI(string message, string hexColor)
        {
            LblStatus.Text = message;
            DotStatus.Fill = Brush.Parse(hexColor);
        }
    }
}