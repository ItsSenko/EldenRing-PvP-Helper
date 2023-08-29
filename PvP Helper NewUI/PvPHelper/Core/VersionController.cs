using PvPHelper.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace PvPHelper.Core
{
    public class VersionController
    {
        private string _releaseUrl;
        private string _updateLocation;

        public string CurrentLocalVersion { get; private set; }
        public string CurrentVersion { get; private set; }
        public bool UpdateAvailable => IsUpdateAvailable();

        public VersionController(string updateLocation)
        {
            _updateLocation = updateLocation;
            _releaseUrl = "https://api.github.com/repos/ItsSenko/EldenRing-PvP-Helper/releases/latest";
            _updateLocation = Directory.GetCurrentDirectory();

            CurrentLocalVersion = GetCurrentLocalVersion();
            CurrentVersion = GetCurrentGlobalVersion();
        }

        private string GetCurrentLocalVersion()
        {
            string basePath = Directory.GetCurrentDirectory();
            string versionFilePath = Path.Combine(basePath, "Resources/Version.txt");

            return File.ReadAllText(versionFilePath);
        }

        private string GetCurrentGlobalVersion()
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "PvPHelperUpdater");
                HttpResponseMessage response = client.GetAsync(_releaseUrl).Result;

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
        public async Task Update()
        {
            try
            {
                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", "PvPHelperUpdater");
                    HttpResponseMessage response = await client.GetAsync(_releaseUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var releaseData = JsonSerializer.Deserialize<Release>(responseBody);
                        var asset = releaseData.Assets[0];

                        string zipUrl = asset.BrowserDownloadUrl;
                        HttpResponseMessage downloadResponse = await client.GetAsync(zipUrl);

                        if (downloadResponse.IsSuccessStatusCode)
                        {
                            using (var contentStream = await downloadResponse.Content.ReadAsStreamAsync())
                            {
                                using (FileStream fileStream = File.Create(Path.Combine(_updateLocation, "update.zip")))
                                {
                                    await contentStream.CopyToAsync(fileStream);
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
                CommandManager.Log("Couldnt update cause: " + ex.Message);
            }
        }

        public void ApplyUpdate()
        { 
            Process.Start(Path.Combine(_updateLocation, "PvPHelperUpdater.exe"), $"pweaseupdate {CurrentVersion}");
            Application.Current.Shutdown();
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
