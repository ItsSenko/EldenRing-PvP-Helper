using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Erd_Tools.Models.Weapon;

namespace PvPHelper.MVVM.Models
{
    public class BuildPrefab
    {
        public string BuildName { get; set; }
        public List<WeaponPrefab> weapons { get; set; }
        public List<ArmorPrefab> armors { get; set; }
        public List<TalismanPrefab> talismans { get; set; }

        public BuildPrefab(string name, List<WeaponPrefab> weapons, List<ArmorPrefab> armors, List<TalismanPrefab> talismans)
        {
            this.BuildName = name;
            this.weapons = weapons;
            this.armors = armors;
            this.talismans = talismans;
        }

        public override string ToString()
        {
            return BuildName;
        }
    }
    public class WeaponPrefab
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int Infusion { get; set; }
        public int SwordArtID { get; set; }
        public int UpgradeLevel { get; set; }

        public WeaponPrefab(string name, int id, Infusion infusion, int swordArtId, int upgradeLevel)
        {
            this.Name = name;
            this.ID = id;
            this.Infusion = (int)infusion;
            this.SwordArtID = swordArtId;
            this.UpgradeLevel = upgradeLevel;
        }
    }
    public class ArmorPrefab
    {
        public int ID { get; set; }
        public ArmorPrefab(int id)
        {
            this.ID = id;
        }
    }
    public class TalismanPrefab
    {
        public int ID { get; set; }
        public TalismanPrefab(int id)
        {
            this.ID = id;
        }
    }

    public class BuildSaver
    {
        public static BuildPrefab loadBuild(string buildName)
        {
            string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Elden Ring PvPHelper/Builds/{buildName}.json");
            if (File.Exists(FilePath))
                return JsonConvert.DeserializeObject<BuildPrefab>(File.ReadAllText(FilePath));

            return null;
        }
        public static void exportBuild(BuildPrefab build, string FilePath)
        {
            FilePath = Path.Combine(FilePath, $"{build.BuildName}.json");
            string json = JsonConvert.SerializeObject(build, Formatting.Indented);
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                sw.Write(json);
                sw.Close();
            }
        }
        public static BuildPrefab importBuild(string FilePath)
        {
            if (File.Exists(FilePath))
                return JsonConvert.DeserializeObject<BuildPrefab>(File.ReadAllText(FilePath));

            return null;
        }

        public static void saveBuild(BuildPrefab build)
        {
            string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Builds/{build.BuildName}.json");
            string json = JsonConvert.SerializeObject(build, Formatting.Indented);
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                sw.Write(json);
                sw.Close();
            }
        }
        public static List<BuildPrefab> getBuilds()
        {
            List<BuildPrefab> builds = new();
            string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Builds/");
            foreach (string fileName in Directory.GetFiles(FilePath, "*.json"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Path.Combine(FilePath, fileName)))
                    {
                        string json = sr.ReadToEnd();
                        builds.Add(JsonConvert.DeserializeObject<BuildPrefab>(json));
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return builds;
        }
    }
}
