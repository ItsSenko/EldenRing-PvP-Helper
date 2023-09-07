using Erd_Tools;
using PvPHelper.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PvPHelper.MVVM.Models;
using Erd_Tools.Models;
using static Erd_Tools.Models.Weapon;
using PvPHelper.MVVM.Views.UserControls;
using System.Collections.ObjectModel;
using PvPHelper.MVVM.Commands.PrefabCreator;
using System.Windows.Media;

namespace PvPHelper.MVVM.ViewModels
{
    public class PrefabCreatorViewModel : ViewModelBase
    {
        #region DataBindings
        private IEnumerable<object> _categoryItemsSource;

        public IEnumerable<object> CategoryItemsSource
        {
            get { return _categoryItemsSource; }
            set { _categoryItemsSource = value; OnPropertyChanged(); }
        }
        private object _categorySelectedItem;

        public object CategorySelectedItem
        {
            get { return _categorySelectedItem; }
            set 
            { 
                _categorySelectedItem = value; 
                OnPropertyChanged(); 
                OnCategoryChanged(); 
            }
        }
        private IEnumerable<object> _selectedInventoryItemsSource;

        public IEnumerable<object> SelectedInventoryItemsSource
        {
            get { return _selectedInventoryItemsSource; }
            set { _selectedInventoryItemsSource = value; OnPropertyChanged(); }
        }
        private object _selectedInventory;

        public object SelectedInventory
        {
            get { return _selectedInventory; }
            set { _selectedInventory = value; OnPropertyChanged(); OnInventoryChanged(true); }
        }
        private int _selectedInvIdx;

        public int SelectedInventoryIndex
        {
            get { return _selectedInvIdx; }
            set { _selectedInvIdx = value; OnPropertyChanged(); }
        }

        private ObservableCollection<InventoryItem> _inventoryItems;

        public ObservableCollection<InventoryItem> InventoryItems
        {
            get { return _inventoryItems; }
            set { _inventoryItems = value; OnPropertyChanged(); }
        }

        private IEnumerable<object> _buildItemsSource;

        public IEnumerable<object> BuildItemsSource
        {
            get { return _buildItemsSource; }
            set { _buildItemsSource = value; OnPropertyChanged(); }
        }
        private object _selectedBuild;

        public object SelectedBuild
        {
            get { return _selectedBuild; }
            set { _selectedBuild = value; OnPropertyChanged(); OnBuildChanged(); }
        }


        public ICommand Load { get; set; }
        public ICommand RefreshBuilds { get; set; }
        public ICommand CreateNew { get; set; }
        public ICommand Save { get; set; }
        #region ItemGib
        private IEnumerable<object> _weaponsItemsSource;

        public IEnumerable<object> WeaponsItemsSource
        {
            get { return _weaponsItemsSource; }
            set 
            { 
                _weaponsItemsSource = value; 
                OnPropertyChanged();
                if (OriginWeapons == null)
                    OriginWeapons = value.ToList();
            }
        }
        private List<object> _originWeapons;

        public List<object> OriginWeapons
        {
            get { return _originWeapons; }
            set { _originWeapons = value; OnPropertyChanged(); }
        }

        private object _weaponsSelectedItem;

        public object WeaponsSelectedItem
        {
            get { return _weaponsSelectedItem; }
            set { _weaponsSelectedItem = value; OnPropertyChanged(); OnItemSelectedChanged(); }
        }


        private IEnumerable<object> _ashesItemSource;

        public IEnumerable<object> AshesItemsSource
        {
            get { return _ashesItemSource; }
            set 
            { 
                _ashesItemSource = value; 
                OnPropertyChanged();
                if (OriginAshes == null)
                    OriginAshes = value.ToList();
            }
        }
        private List<object> _originAshes;

        public List<object> OriginAshes
        {
            get { return _originAshes; }
            set { _originAshes = value; OnPropertyChanged(); }
        }

        private object _ashesSelectedItem;

        public object AshesSelectedItem
        {
            get { return _ashesSelectedItem; }
            set { _ashesSelectedItem = value; OnPropertyChanged(); OnAshSelectedChanged(); }
        }


        private IEnumerable<object> _infItemsSource;

        public IEnumerable<object> InfusionsItemsSource
        {
            get { return _infItemsSource; }
            set 
            { 
                _infItemsSource = value; 
                OnPropertyChanged();
                if (OriginInfusions == null)
                    OriginInfusions = value.ToList();
            }
        }
        private List<object> _originInfusions;

        public List<object> OriginInfusions
        {
            get { return _originInfusions; }
            set { _originInfusions = value; OnPropertyChanged(); }
        }

        private object _infSelectedItem;

        public object InfusionsSelectedItem
        {
            get { return _infSelectedItem; }
            set { _infSelectedItem = value; OnPropertyChanged(); }
        }


        private int _upgradeLvl;

        public int UpgradeLevel
        {
            get { return _upgradeLvl; }
            set { _upgradeLvl = value; OnPropertyChanged(); }
        }
        private int _maxUpgradeLvl;

        public int MaxUpgradeLevel
        {
            get { return _maxUpgradeLvl; }
            set { _maxUpgradeLvl = value; OnPropertyChanged(); }
        }
        private string _upgradeLvlString;

        public string UpgradeLevelString
        {
            get { return _upgradeLvlString; }
            set { _upgradeLvlString = value; OnPropertyChanged(); }
        }


        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged(); }
        }
        private int _maxQuantity;

        public int MaxQuantity
        {
            get { return _maxQuantity; }
            set { _maxQuantity = value; OnPropertyChanged(); }
        }
        private string _quantityString;

        public string QuantityString
        {
            get { return _quantityString; }
            set { _quantityString = value; OnPropertyChanged(); }
        }
        public ICommand AddItem { get; set; }
        #endregion
        #endregion
        public enum InventoryState
        {
            Weapons,
            Armors,
            Talismans
        }
        private ErdHook Hook;

        private bool refreshing = false;
        public PrefabCreatorViewModel(ErdHook hook)
        {
            Hook = hook;
            SetupCommands();

            MaxUpgradeLevel = 25;
            UpgradeLevel = 0;
            UpgradeLevelString = "0";

            MaxQuantity = 10;
            Quantity = 0;
            QuantityString = "0";
            InventoryItems = new();

            Load = new LoadPrefab(hook, this);
            RefreshBuilds = new RelayCommand((o) => 
            {
                refreshing = true;
                BuildItemsSource = BuildSaver.getBuilds();
                SelectedBuild = null;
            });
            CreateNew = new RelayCommand((o) =>
            {
                refreshing = true;
                BuildItemsSource = BuildSaver.getBuilds();
                SelectedBuild = null;

                weaponItems.Clear();
                weaponPrefabs.Clear();

                armorItems.Clear();
                armorPrefabs.Clear();

                talismanItems.Clear();
                talismanPrefabs.Clear();

                InventoryItems.Clear();
            });
            Save = new SavePrefab(this);
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Melee Weapons");
            ItemCategory category1 = ItemCategory.All.FirstOrDefault(x => x.Name == "Ranged Weapons");
            ItemCategory category2 = ItemCategory.All.FirstOrDefault(x => x.Name == "Armor");
            ItemCategory category3 = ItemCategory.All.FirstOrDefault(x => x.Name == "Spell Tools");
            ItemCategory category4 = ItemCategory.All.FirstOrDefault(x => x.Name == "Shields");
            ItemCategory category5 = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");

            List<ItemCategoryOption> categorys = new();
            categorys.Add(new(category.Name, category));
            categorys.Add(new(category1.Name, category1));
            categorys.Add(new(category2.Name, category2));
            categorys.Add(new(category3.Name, category3));
            categorys.Add(new(category4.Name, category4));
            categorys.Add(new(category5.Name, category5));

            List<InventoryStateOption> inventoryStates = new();
            inventoryStates.Add(new(InventoryState.Weapons));
            inventoryStates.Add(new(InventoryState.Armors));
            inventoryStates.Add(new(InventoryState.Talismans));

            CategoryItemsSource = categorys;
            SelectedInventoryItemsSource = inventoryStates;

            BuildItemsSource = BuildSaver.getBuilds();
        }
        private void SetupCommands()
        {
            AddItem = new RelayCommand((o) => { AddItemToInventory(); });
        }
        public List<WeaponPrefab> weaponPrefabs = new();
        public List<InventoryItem> weaponItems = new();

        public List<TalismanPrefab> talismanPrefabs = new();
        public List<InventoryItem> talismanItems = new();

        public List<ArmorPrefab> armorPrefabs = new();
        public List<InventoryItem> armorItems = new();

        private void AddItemToInventory()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (WeaponsSelectedItem == null)
                return;
            string itempicfolder = "PvPHelper.Resources.Images.Items";
            string infusionpicfolder = "PvPHelper.Resources.Images.Infusions";

            var item = WeaponsSelectedItem as ItemGibOption;

            for (int i = 0; i < Quantity; i++)
            {
                if (item.item is Weapon wep)
                {
                    SelectedInventoryIndex = 0;
                    InfusionOption inf = InfusionsSelectedItem != null ? InfusionsSelectedItem as InfusionOption : null;
                    GemOption gem = AshesSelectedItem as GemOption;
                    WeaponPrefab newWeapon = new(wep.Name, wep.ID, inf == null ? Infusion.Standard : inf.infusion, gem == null ? -1 : gem.gem.ID, UpgradeLevel);
                    weaponPrefabs.Add(newWeapon);
                    CreateNewBtn(item.item.Name,
                        Helpers.GetImageSource(item.item.Name, itempicfolder, true, true), "+" + UpgradeLevel.ToString(),
                        Helpers.GetImageSource(inf == null ? Infusion.Standard.ToString() : inf.infusion.ToString(), infusionpicfolder, false, false), weaponItems,
                        wpnPrefab: newWeapon);
                }
                else
                {
                    switch (item.item.ItemCategory)
                    {
                        case Item.Category.Protector:
                            {
                                SelectedInventoryIndex = 1;
                                ArmorPrefab prefab = new(item.item.ID);
                                armorPrefabs.Add(prefab);
                                CreateNewBtn(item.item.Name,
                                    Helpers.GetImageSource(item.item.Name, itempicfolder, true, true), "", null, armorItems, armPrefab: prefab);
                                break;
                            }
                        case Item.Category.Accessory:
                            {
                                SelectedInventoryIndex = 2;
                                TalismanPrefab prefab = new(item.item.ID);
                                talismanPrefabs.Add(prefab);
                                CreateNewBtn(item.item.Name,
                                    Helpers.GetImageSource(item.item.Name, itempicfolder, true, true), "", null, talismanItems, talPrefab: prefab);
                                break;
                            }
                    }
                }
            }
        }

        private void CreateNewBtn(string name, ImageSource icon, string upgradelevel, ImageSource infusionicon, List<InventoryItem> list, WeaponPrefab wpnPrefab = null, ArmorPrefab armPrefab = null, TalismanPrefab talPrefab = null, bool add = true)
        {
            InventoryItem newInvItem = new();
            newInvItem.ItemName = name;
            newInvItem.ItemIconPath = icon;
            newInvItem.UpgradeLevel = upgradelevel;
            newInvItem.InfusionIconPath = infusionicon;

            newInvItem.WeaponPrefab = wpnPrefab == null ? null : wpnPrefab;
            newInvItem.ArmorPrefab = armPrefab == null ? null : armPrefab;
            newInvItem.TalismanPrefab = talPrefab == null ? null : talPrefab;

            newInvItem.MouseRightButtonDown += (o, e) => 
            {
                if (InventoryItems.Contains(newInvItem))
                    InventoryItems.Remove(newInvItem);

                list.Remove(newInvItem);

                if (wpnPrefab != null)
                    weaponPrefabs.Remove(wpnPrefab);

                if (talPrefab != null)
                    talismanPrefabs.Remove(talPrefab);

                if (armPrefab != null)
                    armorPrefabs.Remove(armPrefab);
            };

            list.Add(newInvItem);
            if (add)
                InventoryItems.Add(newInvItem);
        }

        public void Update(InventoryState state, bool updateDrag = true)
        {
            
            switch (state)
            {
                case InventoryState.Weapons:
                    {
                        weaponItems.Clear();
                        weaponPrefabs.Clear();
                        foreach (InventoryItem item in InventoryItems)
                        {
                            weaponItems.Add(item);
                            weaponPrefabs.Add(item.WeaponPrefab);
                        }
                        break;
                    }
                case InventoryState.Armors:
                    {
                        armorItems.Clear();
                        armorPrefabs.Clear();
                        foreach (InventoryItem item in InventoryItems)
                        {
                            armorItems.Add(item);
                            armorPrefabs.Add(item.ArmorPrefab);
                        }
                        break;
                    }
                case InventoryState.Talismans:
                    {
                        talismanItems.Clear();
                        talismanPrefabs.Clear();
                        foreach (InventoryItem item in InventoryItems)
                        {
                            talismanItems.Add(item);
                            talismanPrefabs.Add(item.TalismanPrefab);
                        }
                        break;
                    }
            }
        }
        public InventoryState CurrState = InventoryState.Weapons;
        private void OnInventoryChanged(bool updateDrag = false)
        {
            if (SelectedInventory == null)
                return;

            if (refreshing)
            {
                SelectedInventory = null;
                refreshing = false;
                return;
            }
            if (updateDrag)
                Update(state: CurrState);

            InventoryStateOption state = SelectedInventory as InventoryStateOption;

            switch(state != null ? state.State : InventoryState.Weapons)
            {
                case InventoryState.Weapons:
                    {
                        InventoryItems.Clear();
                        foreach (var item in weaponItems)
                            InventoryItems.Add(item);
                        CurrState = InventoryState.Weapons;
                        break;
                    }
                case InventoryState.Armors:
                    {
                        InventoryItems.Clear();
                        foreach (var item in armorItems)
                            InventoryItems.Add(item);
                        CurrState = InventoryState.Armors;
                        break;
                    }
                case InventoryState.Talismans:
                    {
                        InventoryItems.Clear();
                        foreach (var item in talismanItems)
                            InventoryItems.Add(item);
                        CurrState = InventoryState.Talismans;
                        break;
                    }
            }
        }

        private void OnBuildChanged()
        {
            if (SelectedBuild == null || refreshing)
            {
                weaponItems.Clear();
                weaponPrefabs.Clear();

                armorItems.Clear();
                armorPrefabs.Clear();

                talismanItems.Clear();
                talismanPrefabs.Clear();

                InventoryItems.Clear();
                refreshing = false;
                return;
            }
                

            BuildPrefab build = SelectedBuild as BuildPrefab;
            SelectedInventory = 0;
            
            weaponPrefabs = build.weapons;
            armorPrefabs = build.armors;
            talismanPrefabs = build.talismans;

            weaponItems.Clear();
            armorItems.Clear();
            talismanItems.Clear();
            InventoryItems.Clear();

            string itempicfolder = "PvPHelper.Resources.Images.Items";
            string infusionpicfolder = "PvPHelper.Resources.Images.Infusions";

            if (armorPrefabs.Count > 0)
            {
                foreach (ArmorPrefab prefab in armorPrefabs)
                {
                    ItemCategory itemCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Armor");
                    if (itemCat != null)
                    {
                        Item item = itemCat.Items.FirstOrDefault(x => x.ID == prefab.ID);
                        CreateNewBtn(item.Name,
                            Helpers.GetImageSource(item.Name, itempicfolder, true, true), "",null, armorItems, armPrefab: prefab, add: false);
                    }
                }
            }

            if (talismanPrefabs.Count > 0)
            {
                foreach (TalismanPrefab prefab in talismanPrefabs)
                {
                    ItemCategory itemCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");
                    if (itemCat != null)
                    {
                        Item item = itemCat.Items.FirstOrDefault(x => x.ID == prefab.ID);
                        CreateNewBtn(item.Name,
                            Helpers.GetImageSource(item.Name, itempicfolder, true, true), "", null, talismanItems, talPrefab: prefab, add: false);
                    }
                }
            }
            if (weaponPrefabs.Count > 0)
            {
                foreach (var prefab in weaponPrefabs)
                {
                    ItemCategory itemCat = ItemCategory.All.FirstOrDefault(x => x.Items.Any(x => x.Name == prefab.Name));
                    if (itemCat != null)
                    {
                        Item item = itemCat.Items.FirstOrDefault(x => x.ID == prefab.ID);
                        CreateNewBtn(item.Name,
                            Helpers.GetImageSource(item.Name, itempicfolder, true, true), "+" + prefab.UpgradeLevel.ToString(),
                            Helpers.GetImageSource(((Infusion)prefab.Infusion).ToString(), infusionpicfolder, false, false), weaponItems, wpnPrefab: prefab, add: false);
                    }
                }
            }

            foreach (var item in weaponItems)
                InventoryItems.Add(item);
        }
        private void OnCategoryChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (CategorySelectedItem == null)
                return;
            AshesItemsSource = Enumerable.Empty<object>();
            InfusionsItemsSource = Enumerable.Empty<object>();
            WeaponsItemsSource = Enumerable.Empty<object>();
            OriginAshes = null;
            OriginInfusions = null;
            OriginWeapons = null;

            ItemCategoryOption option = CategorySelectedItem as ItemCategoryOption;
            List<ItemGibOption> gibOptions = new();

            foreach(Item item in option.category.Items)
                gibOptions.Add(new(item.Name, option.Name, item));

            WeaponsItemsSource = gibOptions;
        }
        private void OnItemSelectedChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;
            InfusionsItemsSource = Enumerable.Empty<object>();
            OriginInfusions = null;

            AshesItemsSource = Enumerable.Empty<object>();
            OriginAshes = null;
            if (WeaponsSelectedItem == null)
                return;

            ItemGibOption option = WeaponsSelectedItem as ItemGibOption;
            if (option.item is Weapon weapon)
            {
                if (weapon.Infusible)
                {
                    List<GemOption> gemOptions = new();
                    foreach (Gem gem in Gem.All)
                    {
                        if (gem.WeaponTypes.Contains(weapon.Type))
                        {
                            gemOptions.Add(new(gem.Name.Contains("Ash of War: ") ? gem.Name.Substring(12) : gem.Name, gem));
                        }
                    }
                    AshesItemsSource = gemOptions;
                    MaxUpgradeLevel = 25;
                }
                else
                {
                    MaxUpgradeLevel = 10;
                    if (UpgradeLevel > MaxUpgradeLevel)
                    {
                        UpgradeLevel = MaxUpgradeLevel;
                        UpgradeLevelString = MaxUpgradeLevel.ToString();
                    }
                }
            }
        }
        private void OnAshSelectedChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (AshesSelectedItem == null)
                return;

            OriginInfusions = null;
            GemOption option = AshesSelectedItem as GemOption;
            List<InfusionOption> infusionOptions = new();
            foreach (Infusion infusion in option.gem.Infusions)
                infusionOptions.Add(new(infusion.ToString(), infusion));
            InfusionsItemsSource = infusionOptions;
        }
    }
}
