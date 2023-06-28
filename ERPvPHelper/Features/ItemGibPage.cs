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

namespace ERPvPHelper.Features
{
    public partial class ItemGibPage : Form
    {
        private ErdHook hook { get; set; }
        public ItemGibPage(ErdHook hook)
        {
            InitializeComponent();
            this.hook = hook;

            foreach(ItemCategory category in ItemCategory.All)
            {
                ItemCategoryOption newOption = new(category.Name, category);

                CategoryBox.Items.Add(newOption);
            }
        }

        private void CategoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemsBox.Items.Clear();
            ItemCategoryOption option = CategoryBox.SelectedItem as ItemCategoryOption;
            foreach(Item item in option.category.Items)
            {
                ItemGibOption itemGibOption = new(item.Name, item);
                ItemsBox.Items.Add(itemGibOption);
            }
            ItemsBox.Focus();
        }

        private void ItemsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfusionsBox.Items.Clear();
            AshOfWarBox.Items.Clear();

            ItemGibOption option = ItemsBox.SelectedItem as ItemGibOption;

            if (option.item is Weapon weapon)
            {
                if (weapon.Infusible)
                {
                    AshOfWarBox.Enabled = true;

                    foreach (Gem gem in Gem.All)
                    { 
                        if (gem.WeaponTypes.Contains(weapon.Type))
                        {
                            AshOfWarBox.Items.Add(new GemOption(gem.Name, gem));
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
                UpgradeBox.Enabled = true;

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
            if (option.item is Weapon weapon)
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
    }
}
