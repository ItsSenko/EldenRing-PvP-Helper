using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static Erd_Tools.Models.Weapon;
using Newtonsoft.Json.Linq;
using PvPHelper.Core.Extensions;
using PvPHelper.Core;

namespace PvPHelper.MVVM.Models.Builds
{
    public class BuildSaver
    {
        public static Build loadBuild(string buildName)
        {
            string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Elden Ring PvPHelper/Builds/{buildName}.json");
            if (File.Exists(FilePath))
                return JsonConvert.DeserializeObject<Build>(File.ReadAllText(FilePath));

            return null;
        }
        public static void exportBuild(Build build, string FilePath)
        {
            FilePath = Path.Combine(FilePath, $"{build.Name}.json");
            string json = JsonConvert.SerializeObject(build, Formatting.Indented);
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                sw.Write(json);
                sw.Close();
            }
        }
        public static Build importBuild(string FilePath)
        {
            if (File.Exists(FilePath))
                return JsonConvert.DeserializeObject<Build>(File.ReadAllText(FilePath));

            return null;
        }

        public static void saveBuild(Build build)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), $"Builds/{build.Name}.json");
            string json = JsonConvert.SerializeObject(build, Formatting.Indented);
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                sw.Write(json);
                sw.Close();
            }
        }
        public static List<Build> getBuilds()
        {
            List<Build> builds = new();
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Builds/");
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);
            foreach (string fileName in Directory.GetFiles(FilePath, "*.json"))
            {
                try
                {
                    
                    using (StreamReader sr = new StreamReader(Path.Combine(FilePath, fileName)))
                    {
                        string json = sr.ReadToEnd();

                        JObject jObject = JObject.Parse(json);
                        JArray jArray = (JArray)jObject["Inventories"];
                        if (jObject["Name"] == null)
                        {
                            if (jObject["BuildName"] != null)
                            {
                                if (ExtensionsCore.GetMainHook().Loaded && ExtensionsCore.GetMainHook().Setup)
                                {
                                    Helpers.UpdateBuild(fileName);
                                }
                            }
                            continue;
                        }
                        Build build = new(jObject["Name"].ToString(), new());
                        foreach (var inventory in jArray)
                        {
                            Inventory inv = new(inventory["Name"].ToString(), new());
                            JArray items = (JArray)inventory["Items"];
                            if (inventory["Name"].ToString() == "Weapons")
                            {
                                foreach (var item in items.ToObject<List<WeaponItem>>())
                                {
                                    inv.Items.Add(item);
                                }
                            }
                            else
                                inv.Items = items.ToObject<List<BuildItem>>();

                            build.Inventories.Add(inv);
                        }
                        builds.Add(build);
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
