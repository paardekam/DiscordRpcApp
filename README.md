# Discord Rich Presence Manager

A modern, cross-platform desktop application built with **C#** and **Avalonia UI** that lets you easily manage and broadcast a custom Rich Presence status on Discord through a sleek graphical interface.

---

## Features

* **Discord Dark Theme UI:** Styled to match Discord's native design language (`#1E1F22`, `#5865F2` blurple accents, rounded cards).
* **Live Status Indicator:** Real-time feedback badge showing your current connection state (Active/Disconnected).
* **External Image Support:** Paste direct image or GIF links (e.g., Imgur URLs) for your large display asset.
* **Persistent Settings:** Automatically saves your configuration to `config.json` so your information is remembered across reopens.
* **Cross-Platform & Portable:** Develop and test natively on macOS or Linux, and publish into a single standalone Windows `.exe` executable.

---

## Prerequisites

Make sure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed on your system (v8.0 or newer).

Verify your installation via terminal:
```bash
dotnet --version
```

---

## Step 1: Creating a Discord Developer Application

Before using the app, you need to register a developer application on Discord to get your unique Client ID:

1. Go to the [Discord Developer Portal](https://discord.com/developers/applications).
2. Log in with your Discord account.
3. Click the **New Application** button in the top right corner.
4. Enter a name for your application (this name will appear as the main title of your activity on your Discord profile) and click **Create**.
5. On the **General Information** page, locate your **Application ID** (also called **Client ID**) and click **Copy**.

---

## Step 2: Running the Project Locally

1. Open your terminal and navigate to the project root directory:
   ```bash
   cd DiscordRpcApp
   ```
2. Restore dependencies and launch the application:
   ```bash
   dotnet run
   ```

---

## Step 3: How to Use the Application

1. Launch the application.
2. Paste your copied **Client ID** into the **Discord Client ID** field.
3. Fill in your **Status Details** (e.g., "Coding a C# App").
4. Add a direct image URL (e.g., Imgur link) in the **Large Image URL** field and enter optional hover text.
5. Click **Update Status** to broadcast your custom presence to Discord.
6. The app will automatically save your settings to `config.json` so your configuration is retained when you reopen it.

---

## Step 4: Building the Standalone Windows Executable (`.exe`)

To compile the project into a single, portable, self-contained Windows executable file (`.exe`), run the following command in your terminal:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### Locating Your Output Executable:
Once published, your standalone Windows `.exe` will be located in the output directory:
> `bin/Release/net10.0/win-x64/publish/DiscordRpcApp.exe`

---

## Project Structure & Maintenance

* **`MainWindow.axaml`**: Modern dark-themed user interface markup.
* **`MainWindow.axaml.cs`**: Handles Discord RPC connections, auto-formatting image links, and JSON config persistence.
* **`config.json`**: Generated locally upon clicking "Update Status" to retain your inputs across launches.
* **`.gitignore`**: Ignores build artifacts (`bin/`, `obj/`, `publish/`) and local configurations (`config.json`).
