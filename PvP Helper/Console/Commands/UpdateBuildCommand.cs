using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PvPHelper.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using Erd_Tools;
using PvPHelper.MVVM.Models.Builds;
using static Erd_Tools.Models.Weapon;
using Erd_Tools.Models;

namespace PvPHelper.Console.Commands
{
    internal class UpdateBuildCommand : CommandBase
    {
        private ErdHook hook;
        public UpdateBuildCommand(ErdHook hook)
        {
            Name = "Update Build";
            Description = "Updates the old build format pre-v1.6.2-beta to the post v1.6.2-beta format.";
            CommandString = "/transferbuild";
            HasParams = true;
            RequireParams = true;
            this.hook = hook;
            RequiresParamsString = new string[] { "buildName" };
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (string.IsNullOrEmpty(parameters[0]))
                throw new InvalidCommandException("Build name was missing or does not exist.");

            bool foundBuild = false;

            if (!hook.Loaded || !hook.Setup)
                throw new InvalidCommandException("Not attached to Elden Ring");


            string path = Path.Combine(Directory.GetCurrentDirectory(), $"Builds");
            foreach (var file in Directory.GetFiles(path))
            {
                if (Path.GetFileNameWithoutExtension(file).RemoveSpaces().ToLower() == parameters[0].ToLower())
                {
                    foundBuild = true;
                    UpdateBuild(file);
                }
            }

            if (!foundBuild)
                throw new InvalidCommandException($"The build '{parameters[0]}' does not exist");
        }

        public void UpdateBuild(string path)
        {
            string json = File.ReadAllText(path);

            JObject jObject = JObject.Parse(json);
            string name = jObject["BuildName"].ToString();

            JArray weaponsJArray = (JArray)jObject["weapons"];
            JArray armorsJArray = (JArray)jObject["armors"];
            JArray talismansJArray = (JArray)jObject["talismans"];

            List<WeaponItem> weaponItems = new();
            List<BuildItem> armorItems = new();
            List<BuildItem> talismanItems = new();

            foreach(var weaponJObject in weaponsJArray)
            {
                string Name = weaponJObject["Name"].ToString();
                int ID = weaponJObject["ID"].ToObject<int>();
                int infusion = weaponJObject["Infusion"].ToObject<int>();
                int SwordArtID = weaponJObject["SwordArtID"].ToObject<int>();
                int UpgradeLevel = weaponJObject["UpgradeLevel"].ToObject<int>();

                WeaponItem newWeapon = new(Name, ID, 0, null, infusion, ((Infusion)infusion).ToString(), SwordArtID, 0, UpgradeLevel);

                if (hook.Loaded && hook.Setup)
                {
                    Weapon weapon = Helpers.GetWeaponFromID(ID);
                    Gem gem = Helpers.GetGemFromID(SwordArtID);

                    if (weapon != null)
                    {
                        newWeapon.IconID = weapon.IconID;
                        newWeapon.Category = weapon.ItemCategory.ToString();
                    }

                    if (gem != null)
                    {
                        newWeapon.GemIconID = gem.IconID;
                    }
                }

                weaponItems.Add(newWeapon);
            }

            foreach(var talismanJObj in talismansJArray)
            {
                int ID = talismanJObj["ID"].ToObject<int>();
                BuildItem talismanItem = new(null, ID, 0, null);
                Item item = Helpers.GetItemFromID(ID, Item.Category.Accessory.ToString());

                talismanItem.Name = item.Name;
                talismanItem.IconID = item.IconID;
                talismanItem.Category = item.ItemCategory.ToString();

                talismanItems.Add(talismanItem);
            }

            foreach (var armorJObj in armorsJArray)
            {
                int ID = armorJObj["ID"].ToObject<int>();
                BuildItem armorItem = new(null, ID, 0, null);
                Item item = Helpers.GetItemFromID(ID, Item.Category.Protector.ToString());

                armorItem.Name = item.Name;
                armorItem.IconID = item.IconID;
                armorItem.Category = item.ItemCategory.ToString();

                armorItems.Add(armorItem);
            }

            Build updatedBuild = new(name, new() { new("Weapons", new()), new("Talismans", talismanItems), new("Armors", armorItems) });
            updatedBuild.Inventories[0].Items.AddRange(weaponItems);

            InputDialog authorDialog = new("Author");
            authorDialog.OnSave += (author) => 
            {
                updatedBuild.Author = author;
            };
            authorDialog.ShowDialog();

            InputDialog descriptionDialog = new("Description");
            descriptionDialog.OnSave += (desc) =>
            {
                updatedBuild.Description = desc;
            };
            descriptionDialog.ShowDialog();

            BuildSaver.saveBuild(updatedBuild);
            CommandManager.Log("Updated Build!");
        }

        
    }
}
