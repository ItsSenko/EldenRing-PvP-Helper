using PvPHelper.Console;
using PvPHelper.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PvPHelper.Core
{
    public class VersionController
    {
        public string _releaseUrl;
        public string _iconsReleaseUrl;
        private string _updateLocation;

        public string CurrentLocalVersion { get; private set; }
        public string CurrentVersion { get; private set; }

        public string CurrentIconsVersion { get; private set; }
        public string CurrentLocalIconsVersion { get; private set; }
        public bool UpdateAvailable => IsUpdateAvailable();

        public VersionController(string updateLocation)
        {
            _updateLocation = updateLocation;
            _releaseUrl = "https://api.github.com/repos/ItsSenko/EldenRing-PvP-Helper/releases/latest";
            _iconsReleaseUrl = "https://api.github.com/repos/ItsSenko/EldenRing-PvP-Helper-Icons/releases/latest";

            _updateLocation = Directory.GetCurrentDirectory();

            CurrentLocalVersion = GetCurrentLocalVersion("Resources");
            CurrentVersion = GetCurrentGlobalVersion(_releaseUrl);

            CurrentIconsVersion = GetCurrentGlobalVersion(_iconsReleaseUrl);

            if (Directory.Exists(Path.Combine(updateLocation, "Icons")))
                CurrentLocalIconsVersion = GetCurrentLocalVersion("Icons");
        }

        private string GetCurrentLocalVersion(string path)
        {
            string basePath = Directory.GetCurrentDirectory();
            string versionFilePath = Path.Combine(basePath, $"{path}/Version.txt");

            return File.ReadAllText(versionFilePath);
        }

        private string GetCurrentGlobalVersion(string releaseUrl)
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "PvPHelperUpdater");
                HttpResponseMessage response = client.GetAsync(releaseUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    var releaseData = JsonSerializer.Deserialize<Release>(responseBody);
                    return releaseData.TagName;
                }
                else
                    return "Unavailable";
            }
        }
        private bool IsUpdateAvailable()
        {
            return CurrentVersion != CurrentLocalVersion && CurrentVersion != "Unavailable";
        }
        public bool IsUpdateAvailableForIcons()
        {
            return CurrentIconsVersion != CurrentLocalIconsVersion && CurrentIconsVersion != "Unavailable";
        }
        public int DownloadProgress;
        public static event Action<int> ProgressChanged = new((i) => { });
        private bool CompletedDownload = false;
        public void Update(string releaseUrl, string version)
        {
            ProgressBarDialog progressBar = new($"Downloading update {version}");

            Thread thread = new(async () =>
            {
                try
                {
                    using (HttpClient client = new())
                    {
                        progressBar.Dispatcher.Invoke(() =>
                        {
                            progressBar.TitleBox.Text = "Connecting to GitHub...";
                        });
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("User-Agent", "PvPHelperUpdater");
                        HttpResponseMessage response = await client.GetAsync(releaseUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var releaseData = JsonSerializer.Deserialize<Release>(responseBody);
                            var asset = releaseData.Assets[0];

                            string zipUrl = asset.BrowserDownloadUrl;
                            HttpResponseMessage downloadResponse = await client.GetAsync(zipUrl);
                            progressBar.Dispatcher.Invoke(() =>
                            {
                                progressBar.TitleBox.Text = "Getting Content Stream...";
                            });

                            if (downloadResponse.IsSuccessStatusCode)
                            {
                                using (var contentStream = await downloadResponse.Content.ReadAsStreamAsync())
                                {
                                    using (FileStream fileStream = File.Create(Path.Combine(_updateLocation, "update.zip")))
                                    {
                                        long totalRead = 0;
                                        int bufferSize = 8096; // Start with a reasonably small buffer size
                                        byte[] buffer = new byte[bufferSize];
                                        int bytesRead;

                                        progressBar.Dispatcher.Invoke(() =>
                                        {
                                            progressBar.TitleBox.Text = $"Downloading update {version}...";
                                        });
                                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                        {
                                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                                            totalRead += bytesRead;

                                            // Calculate and update the downloadProgress variable as a percentage

                                            progressBar.Dispatcher.Invoke(() =>
                                            {
                                                progressBar.progressBar.Value = ((int)((double)totalRead / contentStream.Length * 100));
                                            });
                                        }

                                        fileStream.Close();
                                        ApplyUpdate();
                                    }
                                }
                            }
                            else
                                throw new Exception(response.ReasonPhrase);
                        }
                        else
                            throw new Exception(response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                    //CommandManager.Log("Couldnt update cause: " + ex.Message);
                }
            });

            progressBar.StartTask(thread);
            //Thread newThread = new(() => { task.GetAwaiter().GetResult(); });
        }

        public void ApplyUpdate()
        { 
            Process.Start(Path.Combine(_updateLocation, "PvPHelperUpdater.exe"), $"pweaseupdate {CurrentVersion}");
            Process.GetCurrentProcess().Kill();
        }
    }
    public class Asset
    {
        [JsonPropertyName("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }
    }

    public class Release
    {
        [JsonPropertyName("assets")]
        public List<Asset> Assets { get; set; }

        [JsonPropertyName("tag_name")]
        public string TagName { get; set; }
    }
}
