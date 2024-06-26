using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PvPHelper.MVVM.Models.Regions
{
    public class RegionManager
    {
        public List<PlayRegion> Regions { get; set; }
        private string RegionsPath { get; set; }
        public RegionManager()
        {
            Regions = new List<PlayRegion>();
            RegionsPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources/PlayRegions.json");
        }

        public List<PlayRegion> GetRegions()
        {
            if (Regions.Count != 0)
                return Regions;
            Regions = JsonConvert.DeserializeObject<List<PlayRegion>>(File.ReadAllText(RegionsPath));
            return Regions;
        }

        public List<SavedRegion> GetSavedRegions()
        {
            List<SavedRegion> savedRegions = new();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Saved Regions/");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            foreach(string fileName in Directory.GetFiles(filePath, "*.json"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Path.Combine(filePath, fileName)))
                    {
                        string json = sr.ReadToEnd();
                        savedRegions.Add(JsonConvert.DeserializeObject<SavedRegion>(json));
                    }
                }
                catch { }
            }
            return savedRegions;
        }
        public void SaveRegions()
        {
            string json = JsonConvert.SerializeObject(Regions, Formatting.Indented);
            File.WriteAllText(RegionsPath, json);
        }

        public void SaveRegion(string path, SavedRegion regions)
        {
            string json = JsonConvert.SerializeObject(regions, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
