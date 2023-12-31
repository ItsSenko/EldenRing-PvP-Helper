﻿using System.Collections;
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
    public partial class BuildPrefabMaker : Form
    {
        private ErdHook hook { get; set; }
        public List<WeaponPrefab> weaponsPrefab { get; set; }
        public List<ArmorPrefab> armorsPrefab { get; set; }
        public List<TalismanPrefab> talismanPrefab { get; set; }
        public BuildPrefab prefab { get; set; }
        private Logger logger { get; set; }
        public BuildPrefabMaker(ErdHook hook, Logger logger)
        {
            InitializeComponent();
            this.hook = hook;
            this.logger = logger;

            weaponsPrefab = new();
            armorsPrefab = new();
            talismanPrefab = new();

            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Melee Weapons");
            ItemCategory category1 = ItemCategory.All.FirstOrDefault(x => x.Name == "Ranged Weapons");
            ItemCategory category2 = ItemCategory.All.FirstOrDefault(x => x.Name == "Armor");
            ItemCategory category3 = ItemCategory.All.FirstOrDefault(x => x.Name == "Spell Tools");
            ItemCategory category4 = ItemCategory.All.FirstOrDefault(x => x.Name == "Shields");
            ItemCategory category5 = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");

            CategoryBox.Items.Add(new ItemCategoryOption(category.Name, category));
            CategoryBox.Items.Add(new ItemCategoryOption(category1.Name, category1));
            CategoryBox.Items.Add(new ItemCategoryOption(category2.Name, category2));
            CategoryBox.Items.Add(new ItemCategoryOption(category3.Name, category3));
            CategoryBox.Items.Add(new ItemCategoryOption(category4.Name, category4));
            CategoryBox.Items.Add(new ItemCategoryOption(category5.Name, category5));

            SetColors();
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
                if (control is FlowLayoutPanel panel)
                {
                    foreach(Control panelControl in panel.Controls)
                    {
                        panelControl.BackColor = Settings.Default.BackgroundColor;
                        panelControl.ForeColor = Settings.Default.ForegroundColor;
                    }
                }
                control.BackColor = Settings.Default.BackgroundColor;
                control.ForeColor = Settings.Default.ForegroundColor;
            }
        }

        private void InventoryDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(ItemControl)) != null)
            {
                FlowLayoutPanel p = InventoryLayoutPanel;
                int IndexOnPanel = p.Controls.GetChildIndex((sender as ItemControl));
                int IndexOfControl = p.Controls.GetChildIndex(e.Data.GetData(typeof(ItemControl)) as ItemControl);

                ItemControl Control = (ItemControl)e.Data.GetData(typeof(ItemControl));
                ItemControl PanelControl = sender as ItemControl;

                p.Controls.SetChildIndex(Control, IndexOnPanel);
                p.Controls.SetChildIndex(PanelControl, IndexOfControl);
                Control.Invalidate();
                PanelControl.Invalidate();

                switch(Control.InventoryState)
                {
                    case InventoryState.Weapons:
                        {
                            weaponButtons.Clear();
                            foreach(ItemControl ctrl in p.Controls)
                            {
                                weaponButtons.Add(ctrl);
                            }
                            break;
                        }
                    case InventoryState.Talismans:
                        {
                            talismanButtons.Clear();
                            foreach(ItemControl ctrl in p.Controls)
                            {
                                talismanButtons.Add(ctrl);
                            }
                            break;
                        }
                    case InventoryState.Armors:
                        {
                            armorButtons.Clear();
                            foreach (ItemControl ctrl in p.Controls)
                            {
                                armorButtons.Add(ctrl);
                            }
                            break;
                        }
                }
            }
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
        private string[] allowedCatsForUpgrade = new string[] { "Melee Weapons", "Ranged Weapons", "Spell Tools", "Shields" };
        private void ItemsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemGibOption option = ItemsBox.SelectedItem as ItemGibOption;
            if (option == null || option.item == null)
                return;

            InfusionsBox.Items.Clear();
            AshOfWarBox.Items.Clear();
            AshOfWarBox.Text = string.Empty;
            InfusionsBox.Text = string.Empty;
            ashOriginData.Clear();

            if (option.item is Weapon weapon && allowedCatsForUpgrade.Contains(option.CatName))
            {
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
                    if (UpgradeBox.Value == 10)
                        UpgradeBox.Value = UpgradeBox.Maximum;
                }
                else
                {
                    AshOfWarBox.Enabled = false;
                    InfusionsBox.Enabled = false;
                    UpgradeBox.Maximum = 10;
                    if (UpgradeBox.Value == 25)
                        UpgradeBox.Value = UpgradeBox.Maximum;
                }
                UpgradeBox.Enabled = true;

            }
            else
            {
                AshOfWarBox.Enabled = false;
                InfusionsBox.Enabled = false;
                UpgradeBox.Enabled = false;
            }

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

                    WeaponPrefab prefab = new(weapon.Name, weapon.ID, infusionOption != null ? infusionOption.infusion : Infusion.Standard, gemOption != null ? gemOption.gem.ID : -1, (int)UpgradeBox.Value);
                    createBtn($"{weapon.Name} [{(infusionOption != null ? infusionOption.infusion : Infusion.Standard)}] +{UpgradeBox.Value}", option.item, weaponPrefab: prefab);
                }
                else
                {
                    WeaponPrefab prefab = new(weapon.Name, weapon.ID, Infusion.Standard, -1, (int)UpgradeBox.Value);
                    createBtn($"{weapon.Name} [{Infusion.Standard}] +{UpgradeBox.Value}", option.item, weaponPrefab: prefab);
                }
            }
            else
            {
                switch(option.item.ItemCategory)
                {
                    case Item.Category.Protector:
                        {
                            ArmorPrefab prefab = new(option.item.ID);
                            createBtn(option.item.Name, option.item, armorPrefab: prefab);
                            break;
                        }
                    case Item.Category.Accessory:
                        {
                            TalismanPrefab prefab = new(option.item.ID);
                            createBtn(option.item.Name, option.item, talismanPrefab: prefab);
                            break;
                        }
                }
            }
        }
        public void LoadBuild(string name)
        {
            BuildName.Text = name;
            if (weaponsPrefab.Count > 0)
            {
                foreach (WeaponPrefab prefab in weaponsPrefab)
                {
                    ItemCategory itemCat = ItemCategory.All.FirstOrDefault(x => x.Items.Any(x => x.ID == prefab.ID));
                    if (itemCat != null)
                    {
                        Item item = itemCat.Items.FirstOrDefault(x => x.ID == prefab.ID);
                        createBtn($"{item.Name} [{(Infusion)prefab.Infusion}] +{prefab.UpgradeLevel}", item, weaponPrefab: prefab);
                    }
                }
            }
            if (armorsPrefab.Count > 0)
            {
                foreach (ArmorPrefab prefab in armorsPrefab)
                {
                    ItemCategory itemCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Armor");
                    if (itemCat != null)
                    {
                        Item item = itemCat.Items.FirstOrDefault(x => x.ID == prefab.ID);
                        createBtn($"{item.Name}", item, armorPrefab: prefab);
                    }
                }
            }
            if (talismanPrefab.Count > 0)
            {
                foreach (TalismanPrefab prefab in talismanPrefab)
                {
                    ItemCategory itemCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");
                    if (itemCat != null)
                    {
                        Item item = itemCat.Items.FirstOrDefault(x => x.ID == prefab.ID);
                        createBtn($"{item.Name}", item, talismanPrefab: prefab);
                    }
                }
            }
        }
        public List<ItemControl> weaponButtons = new();
        public List<ItemControl> armorButtons = new();
        public List<ItemControl> talismanButtons = new();
        public InventoryState currState;

        private void createBtn(string name, Item item, ArmorPrefab armorPrefab = null, WeaponPrefab weaponPrefab = null, TalismanPrefab talismanPrefab = null)
        {
            ItemControl newBtn = new(item);
            newBtn.Text = name;
            newBtn.FlatStyle = FlatStyle.Flat;
            newBtn.BackColor = Settings.Default.BackgroundColor;
            newBtn.ForeColor = Settings.Default.ForegroundColor;
            newBtn.Size = new(81, 84);
            
            newBtn.MouseDown += (s, e) => 
            { 
                if (MoveItemsToggle.Checked)
                    DoDragDrop(newBtn, DragDropEffects.All);
                else
                {
                    ItemOptions itemOption = new(this, newBtn, hook);
                    itemOption.Show();
                }
            };
            newBtn.DragOver += InventoryDragDrop;

            switch (item.ItemCategory)
            {
                case Item.Category.Weapons:
                    {
                        newBtn.WeaponPrefab = weaponPrefab;
                        newBtn.InventoryState = InventoryState.Weapons;
                        weaponButtons.Add(newBtn);
                        if (currState == InventoryState.Weapons)
                            InventoryLayoutPanel.Controls.Add(newBtn);
                        else
                        {
                            currState = InventoryState.Weapons;
                            InventoryLayoutPanel.Controls.Clear();
                            foreach(var btn in weaponButtons)
                                InventoryLayoutPanel.Controls.Add(btn);
                        }
                        break;
                    }
                case Item.Category.Protector:
                    {
                        newBtn.ArmorPrefab = armorPrefab;
                        newBtn.InventoryState = InventoryState.Armors;
                        armorButtons.Add(newBtn);
                        if (currState == InventoryState.Armors)
                            InventoryLayoutPanel.Controls.Add(newBtn);
                        else
                        {
                            currState = InventoryState.Armors;
                            InventoryLayoutPanel.Controls.Clear();
                            foreach (var btn in armorButtons)
                                InventoryLayoutPanel.Controls.Add(btn);
                        }
                        break;
                    }
                case Item.Category.Accessory:
                    {
                        newBtn.TalismanPrefab = talismanPrefab;
                        newBtn.InventoryState = InventoryState.Talismans;
                        talismanButtons.Add(newBtn);
                        if (currState == InventoryState.Talismans)
                            InventoryLayoutPanel.Controls.Add(newBtn);
                        else
                        {
                            currState = InventoryState.Talismans;
                            InventoryLayoutPanel.Controls.Clear();
                            foreach (var btn in talismanButtons)
                                InventoryLayoutPanel.Controls.Add(btn);
                        }
                        break;
                    }
            }
        }

        public enum InventoryState
        {
            Weapons,
            Armors,
            Talismans
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            foreach(ItemControl itemControl in weaponButtons)
            {
                if (!weaponsPrefab.Contains(itemControl.WeaponPrefab))
                    weaponsPrefab.Add(itemControl.WeaponPrefab);
            }
            foreach(ItemControl itemControl in armorButtons)
            {
                if (!armorsPrefab.Contains(itemControl.ArmorPrefab))
                    armorsPrefab.Add(itemControl.ArmorPrefab);
            }
            foreach(ItemControl itemControl in talismanButtons)
            {
                if (!talismanPrefab.Contains(itemControl.TalismanPrefab))
                    talismanPrefab.Add(itemControl.TalismanPrefab);
            }
            BuildPrefab newBuildPrefab = new(BuildName.Text, weaponsPrefab, armorsPrefab, talismanPrefab);
            BuildSaver.saveBuild(newBuildPrefab);
            this.Close();
        }
        public void UpdateInventory()
        {
            switch (currState)
            {
                case InventoryState.Weapons:
                    {
                        InventoryLayoutPanel.Controls.Clear();
                        foreach(ItemControl itemControl in weaponButtons)
                        {
                            InventoryLayoutPanel.Controls.Add(itemControl);
                        }
                        break;
                    }
                case InventoryState.Talismans:
                    {
                        InventoryLayoutPanel.Controls.Clear();
                        foreach (ItemControl itemControl in talismanButtons)
                        {
                            InventoryLayoutPanel.Controls.Add(itemControl);
                        }
                        break;
                    }
                case InventoryState.Armors:
                    {
                        InventoryLayoutPanel.Controls.Clear();
                        foreach (ItemControl itemControl in armorButtons)
                        {
                            InventoryLayoutPanel.Controls.Add(itemControl);
                        }
                        break;
                    }
            }
        }
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void SetInventory(InventoryState state)
        {
            InventoryLayoutPanel.Controls.Clear();

            switch(state)
            {
                case InventoryState.Weapons:
                    {
                        foreach(ItemControl control in weaponButtons)
                        {
                            InventoryLayoutPanel.Controls.Add(control);
                        }
                        currState = InventoryState.Weapons;
                        break;
                    }
                case InventoryState.Armors:
                    {
                        foreach(ItemControl control in armorButtons)
                        {
                            InventoryLayoutPanel.Controls.Add(control);
                        }
                        currState = InventoryState.Armors;
                        break;
                    }
                case InventoryState.Talismans:
                    {
                        foreach(ItemControl control in talismanButtons)
                        {
                            InventoryLayoutPanel.Controls.Add(control);
                        }
                        currState = InventoryState.Talismans;
                        break;
                    }
            }
        }
        private void WeaponsBtn_Click(object sender, EventArgs e)
        {
            SetInventory(InventoryState.Weapons);
        }

        private void ArmorsBtn_Click(object sender, EventArgs e)
        {
            SetInventory(InventoryState.Armors);
        }

        private void TalismanBtn_Click(object sender, EventArgs e)
        {
            SetInventory(InventoryState.Talismans);
        }

        private void BuildPrefabMaker_Load(object sender, EventArgs e)
        {
            if (weaponsPrefab.Count > 0)
            {
                foreach (WeaponPrefab prefab in weaponsPrefab)
                {
                    ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Items.FirstOrDefault(x => x.ID == prefab.ID && x is Weapon) != null);
                    Item item = category.Items.FirstOrDefault(x => x.ID == prefab.ID);
                    if (item is Weapon weapon)
                    {
                        Gem gem = Gem.All.FirstOrDefault(x => x.ID == prefab.SwordArtID);

                        if (!gem.WeaponTypes.Contains(weapon.Type))
                        {
                            var result = MessageBox.Show($"Warning: The weapon [{weapon.Name}] does not have a proper ash of war. Would you like to change its ash of war back to default?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                var defaultGem = weapon.DefaultGem?.ID ?? -1;
                                prefab.SwordArtID = defaultGem;
                            }
                        }
                    }
                }
            }
        }

        private void InventoryLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    public partial class ItemControl : Button
    {
        public string Name { get; set; }
        public Item Item { get; set; }
        public BuildPrefabMaker.InventoryState InventoryState { get; set; }
        public ArmorPrefab ArmorPrefab { get; set; }
        public WeaponPrefab WeaponPrefab { get; set; }
        public TalismanPrefab TalismanPrefab { get; set; }

        public ItemControl(Item item) : base()
        {
            this.Name = item.Name;
            this.Item = item;

            AllowDrop = true;
        }
    }
}
