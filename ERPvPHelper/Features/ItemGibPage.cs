using System.Collections;
using System.Diagnostics;
using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Entities;
using Erd_Tools.Models.Items;
using PropertyHook;
using SoulsFormats;
using static Erd_Tools.Models.Param;
using System.Runtime.InteropServices;
using System.Text;
using static Erd_Tools.Models.Weapon;
using System.Linq;

namespace ERPvPHelper.Features
{
    public partial class ItemGibPage : Form
    {
        private ErdHook hook { get; set; }
        private Logger logger { get; set; }
        public ItemGibPage(ErdHook hook, Logger logger)
        {
            InitializeComponent();
            this.hook = hook;
            this.logger = logger;

            foreach(ItemCategory category in ItemCategory.All)
            {
                ItemCategoryOption newOption = new(category.Name, category);

                CategoryBox.Items.Add(newOption);
            }

            ItemsBox.TextChanged += ItemsBox_TextChanged;
            AshOfWarBox.TextChanged += AshBox_TextChanged;
        }
        List<ItemGibOption> originData = new();
        List<ItemGibOption> updatedList = new List<ItemGibOption>();

        private void ItemsBox_TextChanged(object sender, EventArgs e)
        {
            if (ItemsBox.SelectedItem != null)
                return;
            if (string.IsNullOrEmpty(ItemsBox.Text))
            {
                ItemsBox.Items.Clear();
                foreach (ItemGibOption option in originData)
                {
                    ItemsBox.Items.Add(option);
                }
                return;
            }
            string searchText = ItemsBox.Text.ToLower();

            // Filter the list based on the search text and order by position
            updatedList = originData
                .Where(item => item.Name.ToLower().Contains(searchText))
                .OrderBy(item => item.Name.IndexOf(searchText))
                .ToList();

            // Update the ComboBox items only once the user finishes typing
            ItemsBox.BeginUpdate();
            ItemsBox.Items.Clear();
            foreach (ItemGibOption item in updatedList)
            {
                ItemsBox.Items.Add(item);
            }
            ItemsBox.EndUpdate();
            ItemsBox.Select(ItemsBox.Text.Length, 0);
        }
        List<GemOption> ashOriginData = new();
        List<GemOption> ashUpdatedList = new();
        private void AshBox_TextChanged(object sender, EventArgs e)
        {
            if (AshOfWarBox.SelectedItem != null)
                return;
            if (string.IsNullOrEmpty(AshOfWarBox.Text))
            {
                AshOfWarBox.Items.Clear();
                foreach (GemOption option in ashOriginData)
                {
                    AshOfWarBox.Items.Add(option);
                }
                return;
            }
            string searchText = AshOfWarBox.Text.ToLower();

            // Filter the list based on the search text and order by position
            ashUpdatedList = ashOriginData
                .Where(item => item.Name.ToLower().Contains(searchText))
                .OrderBy(item => item.Name.IndexOf(searchText))
                .ToList();

            // Update the ComboBox items only once the user finishes typing
            AshOfWarBox.BeginUpdate();
            AshOfWarBox.Items.Clear();
            foreach (GemOption item in ashUpdatedList)
            {
                AshOfWarBox.Items.Add(item);
            }
            AshOfWarBox.EndUpdate();
            AshOfWarBox.Select(ItemsBox.Text.Length, 0);
        }
        private void CategoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemsBox.Items.Clear();
            originData.Clear();
            ItemCategoryOption option = CategoryBox.SelectedItem as ItemCategoryOption;
            foreach(Item item in option.category.Items)
            {
                ItemGibOption itemGibOption = new(item.Name, option.Name, item);
                ItemsBox.Items.Add(itemGibOption);
                originData.Add(itemGibOption);
            }
            ItemsBox.Focus();
        }
        private string[] allowedCatsForUpgrade = new string[] { "Melee Weapons", "Ranged Weapons", "Spell Tools", "Shields"};
        private void ItemsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfusionsBox.Items.Clear();
            AshOfWarBox.Items.Clear();
            ashOriginData.Clear();

            ItemGibOption option = ItemsBox.SelectedItem as ItemGibOption;

            if (option.item is Weapon weapon)
            {
                if (!allowedCatsForUpgrade.Contains(option.CatName))
                {
                    UpgradeBox.Enabled = false;

                    QuantityBox.Maximum = option.item.MaxQuantity;
                    return;
                }
                if (weapon.Infusible)
                {
                    AshOfWarBox.Enabled = true;

                    foreach (Gem gem in Gem.All)
                    { 
                        if (gem.WeaponTypes.Contains(weapon.Type))
                        {
                            var newGem = new GemOption(gem.Name.Contains("Ash of War: ") ? gem.Name.Substring(12) : gem.Name, gem);
                            AshOfWarBox.Items.Add(newGem);
                            ashOriginData.Add(newGem);
                        }
                    }
                    UpgradeBox.Maximum = 25;
                }
                else
                {
                    AshOfWarBox.Enabled = false;
                    InfusionsBox.Enabled = false;
                    UpgradeBox.Maximum = 10;
                }
                UpgradeBox.Enabled = true;
            }
            else
                UpgradeBox.Enabled = false;

            QuantityBox.Maximum = option.item.MaxQuantity;
            
        }

        private void AshOfWarBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfusionsBox.Items.Clear();

            GemOption option = AshOfWarBox.SelectedItem as GemOption;
            foreach(Infusion infusion in option.gem.Infusions)
            {
                InfusionOption newOption = new(infusion.ToString(), infusion);
                InfusionsBox.Items.Add(newOption);
            }
            InfusionsBox.Enabled = true;
        }

        private void GibBtn_Click(object sender, EventArgs e)
        {
            ItemGibOption option = (ItemGibOption)ItemsBox.SelectedItem;
            if (option.item is Weapon weapon && allowedCatsForUpgrade.Contains(option.CatName))
            {
                if (weapon.Infusible)
                {
                    GemOption gemOption = (GemOption)AshOfWarBox.SelectedItem;
                    InfusionOption infusionOption = (InfusionOption)InfusionsBox.SelectedItem;

                    hook.GetItem(new(weapon.ID, weapon.ItemCategory, (int)QuantityBox.Value, weapon.MaxQuantity, infusionOption != null ? (int)infusionOption.infusion : (int)Infusion.Standard, (int)UpgradeBox.Value, gemOption != null ? gemOption.gem.ID : -1, weapon.EventID));
                }
                else
                {
                    hook.GetItem(new(weapon.ID, weapon.ItemCategory, (int)QuantityBox.Value, weapon.MaxQuantity, (int)Infusion.Standard, (int)UpgradeBox.Value, -1, weapon.EventID));
                }
            }
            else
            {
                hook.GetItem(new(option.item.ID, option.item.ItemCategory, (int)QuantityBox.Value, option.item.MaxQuantity, (int)Infusion.Standard, 0, -1, option.item.EventID));
            }
        }

        private void ItemGibPage_Load(object sender, EventArgs e)
        {
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
    }
}
