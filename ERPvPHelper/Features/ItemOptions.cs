using Erd_Tools;
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
using static Erd_Tools.Models.Weapon;

namespace ERPvPHelper.Features
{
    public partial class ItemOptions : Form
    {
        private BuildPrefabMaker maker;
        private ItemControl item;
        private ErdHook hook;
        public ItemOptions(BuildPrefabMaker maker, ItemControl item, ErdHook hook)
        {
            InitializeComponent();

            this.maker = maker;
            this.item = item;
            this.hook = hook;

            if (item.InventoryState == BuildPrefabMaker.InventoryState.Weapons)
            {
                if (item.Item is Weapon weapon)
                {
                    if (!weapon.Infusible)
                    {
                        AshOfWarBox.Enabled = false;
                        InfusionBox.Enabled = false;
                        return;
                    }

                    foreach(var gem in Gem.All)
                    {
                        if (gem.WeaponTypes.Contains(weapon.Type))
                        {
                            var index = AshOfWarBox.Items.Add(new GemOption(gem.Name, gem));

                            if (item.WeaponPrefab.SwordArtID == gem.ID)
                            {
                                AshOfWarBox.SelectedIndex = index;
                            }
                        }
                    }

                    GemOption gemOption = AshOfWarBox.SelectedItem as GemOption;
                    if (gemOption.gem.Infusions == null)
                    {
                        return;
                    }
                    foreach(Infusion infusion in gemOption.gem.Infusions)
                    {
                        var index = InfusionBox.Items.Add(new InfusionOption(infusion.ToString(), infusion));
                        if (item.WeaponPrefab.Infusion == (int)infusion)
                        {
                            InfusionBox.SelectedIndex = index;
                        }
                    }
                }
            }
            else
            {
                AshOfWarBox.Enabled = false;
                InfusionBox.Enabled = false;
                SaveBtn.Enabled = false;
            }
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

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            switch (item.InventoryState)
            {
                case BuildPrefabMaker.InventoryState.Weapons:
                    {
                        if (item.Item is Weapon weapon)
                        {
                            GemOption gem = AshOfWarBox.SelectedItem as GemOption;
                            InfusionOption infusion = InfusionBox.SelectedItem as InfusionOption;
                            if (infusion == null)
                                infusion = new(Infusion.Standard.ToString(), Infusion.Standard);

                            var prefab = maker.weaponsPrefab.FirstOrDefault(item.WeaponPrefab);
                            var btn = maker.weaponButtons.FirstOrDefault(item);

                            item.Text = $"{weapon.Name} [{infusion.Name}] +{item.WeaponPrefab.UpgradeLevel}";
                            item.WeaponPrefab.Infusion = (int)infusion.infusion;
                            item.WeaponPrefab.SwordArtID = gem == null ? (weapon.DefaultGem?.ID ?? -1) : gem.gem.ID;

                            prefab.Infusion = item.WeaponPrefab.Infusion;
                            prefab.SwordArtID = item.WeaponPrefab.SwordArtID;
                            
                        }
                        maker.UpdateInventory();
                        break;
                    }
            }

            Close();
        }
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            switch(item.InventoryState)
            {
                case BuildPrefabMaker.InventoryState.Weapons:
                    {
                        maker.weaponsPrefab.Remove(item.WeaponPrefab);
                        maker.weaponButtons.Remove(item);
                        maker.UpdateInventory();
                        break;
                    }
                case BuildPrefabMaker.InventoryState.Armors:
                    {
                        maker.armorsPrefab.Remove(item.ArmorPrefab);
                        maker.armorButtons.Remove(item);
                        maker.UpdateInventory();
                        break;
                    }
                case BuildPrefabMaker.InventoryState.Talismans:
                    {
                        maker.talismanPrefab.Remove(item.TalismanPrefab);
                        maker.talismanButtons.Remove(item);
                        maker.UpdateInventory();
                        break;
                    }
            }

            Close();
        }

        private void AshOfWarBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfusionBox.Items.Clear();
            GemOption gemOption = AshOfWarBox.SelectedItem as GemOption;
            foreach (Infusion infusion in gemOption.gem.Infusions)
            {
                var index = InfusionBox.Items.Add(new InfusionOption(infusion.ToString(), infusion));
                if (item.WeaponPrefab.Infusion == (int)infusion)
                {
                    InfusionBox.SelectedIndex = index;
                }
            }
        }
    }
}
