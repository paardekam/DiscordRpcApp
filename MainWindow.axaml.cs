using Avalonia.Controls;
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
                    // Ignore load errors if file is corrupt
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
                LblStatus.Text = "Error: Client ID is required!";
                LblStatus.Foreground = Brushes.Red;
                return;
            }

            // Save user inputs to config file on update
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

            LblStatus.Text = "Status: Rich Presence Active & Saved!";
            LblStatus.Foreground = Brushes.Green;
        }

        private void BtnStop_Click(object? sender, RoutedEventArgs e)
        {
            if (client != null && !client.IsDisposed)
            {
                client.ClearPresence();
                client.Dispose();
                client = null;
            }

            LblStatus.Text = "Status: Disconnected";
            LblStatus.Foreground = Brushes.Red;
        }
    }
}