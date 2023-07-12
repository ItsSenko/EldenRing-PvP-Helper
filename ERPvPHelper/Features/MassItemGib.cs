using Erd_Tools.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Erd_Tools;
using static Erd_Tools.Models.Weapon;
using Erd_Tools.Models.Items;

namespace ERPvPHelper.Features
{
    public partial class MassItemGib : Form
    {
        private ErdHook hook;
        public MassItemGib(ErdHook hook)
        {
            InitializeComponent();
            this.hook = hook;
            SetColors();
        }
        public void SetColors()
        {
            this.BackColor = Settings.Default.BackgroundColor;

            foreach (Control control in this.Controls)
            {
                if (control is GroupBox box)
                {
                    foreach (Control boxControl in box.Controls)
                    {
                        boxControl.BackColor = Settings.Default.BackgroundColor;
                        boxControl.ForeColor = Settings.Default.ForegroundColor;
                    }
                    continue;
                }
                control.BackColor = Settings.Default.BackgroundColor;
                control.ForeColor = Settings.Default.ForegroundColor;
            }
        }
        private void MassGib(ItemCategory category, string[] excludedNames = null, int quantity = 0)
        {
            List<ItemSpawnInfo> items = new();
            foreach (Item item in category.Items)
            {
                if (!Blacklist.blacklistedItems.Any(x => x.ItemID == item.ID && x.CatName == category.Name))
                {
                    if (excludedNames != null && excludedNames.Any(x => x == item.Name))
                        continue;
                    ItemSpawnInfo info = new(item.ID, item.ItemCategory, quantity == 0 ? item.MaxQuantity : quantity, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID);
                    items.Add(info);
                }
            }

            hook.GetItem(items, CancellationToken.None);
        }
        private void AllMeleeWeaponsBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Melee Weapons");
            List<ItemSpawnInfo> items = new();
            foreach (Item item in category.Items)
            {
                if(item is Weapon weapon)
                {
                    var gem = weapon.DefaultGem?.ID ?? -1;
                    int roundedLevel = (int)Math.Round(UpgradeLevel.Value / 2);
                    int upgradeLevel = weapon.Infusible ? (int)UpgradeLevel.Value : roundedLevel > 10 ? 10 : roundedLevel;

                    items.Add(new(weapon.ID, weapon.ItemCategory, 1, weapon.MaxQuantity, (int)Infusion.Standard, upgradeLevel, gem, weapon.EventID));
                }
            }
            hook.GetItem(items, CancellationToken.None);
        }

        private void AllRangeBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Ranged Weapons");
            List<ItemSpawnInfo> items = new();
            foreach (Item item in category.Items)
            {
                if (item is Weapon weapon)
                {
                    var gem = weapon.DefaultGem?.ID ?? -1;
                    int roundedLevel = (int)Math.Round(UpgradeLevel.Value / 2);
                    int upgradeLevel = weapon.Infusible ? (int)UpgradeLevel.Value : roundedLevel > 10 ? 10 : roundedLevel;

                    items.Add(new(weapon.ID, weapon.ItemCategory, 1, weapon.MaxQuantity, (int)Infusion.Standard, upgradeLevel, gem, weapon.EventID));
                }
            }
            hook.GetItem(items, CancellationToken.None);
        }

        private void AllShieldBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Shields");
            List<ItemSpawnInfo> items = new();
            foreach (Item item in category.Items)
            {
                if (item is Weapon weapon)
                {
                    var gem = weapon.DefaultGem?.ID ?? -1;
                    int roundedLevel = (int)Math.Round(UpgradeLevel.Value / 2);
                    int upgradeLevel = weapon.Infusible ? (int)UpgradeLevel.Value : roundedLevel > 10 ? 10 : roundedLevel;

                    items.Add(new(weapon.ID, weapon.ItemCategory, 1, weapon.MaxQuantity, (int)Infusion.Standard, upgradeLevel, gem, weapon.EventID));
                }
            }
            hook.GetItem(items, CancellationToken.None);
        }

        private void AllSpellTools_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Spell Tools");
            List<ItemSpawnInfo> items = new();
            foreach (Item item in category.Items)
            {
                if (item is Weapon weapon)
                {
                    var gem = weapon.DefaultGem?.ID ?? -1;
                    int roundedLevel = (int)Math.Round(UpgradeLevel.Value / 2);
                    int upgradeLevel = weapon.Infusible ? (int)UpgradeLevel.Value : roundedLevel > 10 ? 10 : roundedLevel;

                    items.Add(new(weapon.ID, weapon.ItemCategory, 1, weapon.MaxQuantity, (int)Infusion.Standard, upgradeLevel, gem, weapon.EventID));
                }
            }

            hook.GetItem(items, CancellationToken.None);
        }

        private void AllArmorBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Armor");
            MassGib(category);
        }

        private void AllTaliBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");
            MassGib(category);
        }

        private void AllCookbookBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Cookbooks");
            MassGib(category);
        }

        private void AllMerchantBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Merchant Items");
            MassGib(category);
        }

        private void AllTearsBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Crystal Tears");
            MassGib(category);
        }

        private void AllConsumeBtn_Click(object sender, EventArgs e)
        {
            string[] exclude = new string[] { "Cracked Pot", "Ritual Pot", "Perfume Bottle",
            "Remembrance of the Regal Ancestor","Remembrance of the Naturalborn","Remembrance of the Lichdragon",
            "Remembrance of the Fire Giant", "Remembrance of the Grafted", "Remembrance of the Full Moon Queen",
            "Remembrance of the Blasphemous","Remembrance of the Starscourge","Remembrance of the Omen King",
            "Remembrance of the Blood Lord", "Remembrance of the Rot Goddess", "Remembrance of the Black Blade",
            "Remembrance of Hoarah Loux", "Remembrance of the Dragonlord", "Elden Remembrance", "Shabriri Grape", 
            "Baldachin's Blessing", "Radiant Baldachin's Blessing"};
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Consumables");
            MassGib(category, exclude);
        }

        private void AllAmmoBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Ammo");
            MassGib(category);
        }

        private void AllUpgMatsBtn_Click(object sender, EventArgs e)
        {
            string[] exclude = new string[] { "Golden Seed", "Sacred Tear", "Memory Stone", "Talisman Pouch" };
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Upgrade Materials");
            MassGib(category, exclude);
        }

        private void AllToolsBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Tools");
            MassGib(category);
        }

        private void AllCraftMatBtn_Click(object sender, EventArgs e)
        {
            string[] exclude = new string[] { "Cracked Pot", "Ritual Pot", "Celestial Dew" };
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Crafting Materials");
            MassGib(category, exclude);
        }

        private void AllAshBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Gems");
            MassGib(category);
        }

        private void AllSpellsBtn_Click(object sender, EventArgs e)
        {
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Magic");
            MassGib(category, quantity: 1);
        }

        private void AllLimBtn_Click(object sender, EventArgs e)
        {
            string[] itemsToAdd = new string[] { "Cracked Pot", "Ritual Pot", "Perfume Bottle", "Duelist's Furled Finger", "White Cipher Ring", "Blue Cipher Ring",
            "Taunter's Tounge", "Recusant Finger", "Bloody Finger"};
            ItemCategory keyItemsCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Key Items");
            ItemCategory consumablesCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Consumables");

            List<ItemSpawnInfo> keyItems = new();
            List<ItemSpawnInfo> consumables = new();

            foreach(Item item in keyItemsCat.Items)
            {
                if (itemsToAdd.Contains(item.Name))
                {
                    keyItems.Add(new(item.ID, item.ItemCategory, item.MaxQuantity, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID));
                }
            }

            foreach (Item item in consumablesCat.Items)
            {
                if (itemsToAdd.Contains(item.Name))
                {
                    consumables.Add(new(item.ID, item.ItemCategory, item.MaxQuantity, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID));
                }
            }

            hook.GetItem(keyItems, CancellationToken.None);
            hook.GetItem(consumables, CancellationToken.None);
        }

        int[] whetbladeFlags = new int[] { 65600, 65610, 65620, 65630, 65640, 65650, 65660, 65670, 65680, 65690, 65700, 65710, 65720 };
        private void AllWhetbladesBtn_Click(object sender, EventArgs e)
        {
            foreach (int whetblade in whetbladeFlags)
                hook.SetEventFlag(whetblade, true);
        }
    }
}
