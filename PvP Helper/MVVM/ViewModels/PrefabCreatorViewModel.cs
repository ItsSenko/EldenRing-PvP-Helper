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
using PvPHelper.MVVM.Dialogs;
using PvPHelper.MVVM.Models.Search.SortOrders;
using PvPHelper.MVVM.Models.Search;
using PvPHelper.MVVM.Models.Builds;
using System.ComponentModel;
using System.Windows;
using System.Threading;
using Erd_Tools.Models.Items;

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
            set { _selectedInventory = value; OnPropertyChanged(); OnSelectedInventoryChanged(); }
        }
        private int _selectedInvIdx;

        public int SelectedInventoryIndex
        {
            get { return _selectedInvIdx; }
            set { _selectedInvIdx = value; OnPropertyChanged(); }
        }

        private BindingList<InventoryItem> _inventoryItems;

        public BindingList<InventoryItem> InventoryItems
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
        private string _weaponSearchString;

        public string WeaponsSearchText
        {
            get { return _weaponSearchString; }
            set
            {
                _weaponSearchString = value;
                if (weaponSearch != null)
                    weaponSearch.SearchString = _weaponSearchString;
                OnPropertyChanged();
            }
        }

        private string _gemSearchText;

        public string GemSearchText
        {
            get { return _gemSearchText; }
            set
            {
                _gemSearchText = value;
                if (gemSearch != null)
                    gemSearch.SearchString = _gemSearchText;
                OnPropertyChanged();
            }
        }

        private string _infusionSearchText;

        public string InfusionsSearchText
        {
            get { return _infusionSearchText; }
            set
            {
                _infusionSearchText = value;
                if (infusionSearch != null)
                    infusionSearch.SearchString = _infusionSearchText;
                OnPropertyChanged();
            }
        }
        private IEnumerable<object> _weaponsItemsSource;

        public IEnumerable<object> WeaponsItemsSource
        {
            get { return _weaponsItemsSource; }
            set 
            { 
                _weaponsItemsSource = value; 
                OnPropertyChanged();
            }
        }

        private object _weaponsSelectedItem;

        public object WeaponsSelectedItem
        {
            get { return _weaponsSelectedItem; }
            set { _weaponsSelectedItem = value;
                if (value == null && !string.IsNullOrEmpty(WeaponsSearchText) && weaponSearch != null && weaponSearch.ShownItems != null)
                {
                    _weaponsSelectedItem = weaponSearch.ShownItems.FirstOrDefault(x => x.ToString() == WeaponsSearchText);
                }
                OnPropertyChanged(); OnItemSelectedChanged(); }
        }


        private IEnumerable<object> _ashesItemSource;

        public IEnumerable<object> AshesItemsSource
        {
            get { return _ashesItemSource; }
            set 
            { 
                _ashesItemSource = value; 
                OnPropertyChanged();
            }
        }

        private object _ashesSelectedItem;

        public object AshesSelectedItem
        {
            get { return _ashesSelectedItem; }
            set { _ashesSelectedItem = value;
                if (value == null && !string.IsNullOrEmpty(GemSearchText) && gemSearch != null && gemSearch.ShownItems != null)
                {
                    _ashesSelectedItem = gemSearch.ShownItems.FirstOrDefault(x => x.ToString() == GemSearchText);
                }
                OnPropertyChanged(); OnAshSelectedChanged(); }
        }


        private IEnumerable<object> _infItemsSource;

        public IEnumerable<object> InfusionsItemsSource
        {
            get { return _infItemsSource; }
            set
            {
                _infItemsSource = value; 
                OnPropertyChanged();
            }
        }

        private object _infSelectedItem;

        public object InfusionsSelectedItem
        {
            get { return _infSelectedItem; }
            set { _infSelectedItem = value;
                if (value == null && !string.IsNullOrEmpty(InfusionsSearchText) && infusionSearch != null && infusionSearch.ShownItems != null)
                {
                    _infSelectedItem = infusionSearch.ShownItems.FirstOrDefault(x => x.ToString() == InfusionsSearchText);
                }
                OnPropertyChanged(); }
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
        private ErdHook Hook;

        Dictionary<int, int> SmithyToSomber = new();
        Dictionary<int, int> SomberToSmithy = new();

        private SearchAlgorithm<Item> weaponSearch;
        private SearchAlgorithm<Gem> gemSearch;
        private SearchAlgorithm<Infusion> infusionSearch;

        private Build _currentBuild;

        public Build CurrentBuild
        {
            get { return _currentBuild; }
            set { _currentBuild = value; OnCurrentBuildChanged(_currentBuild); }
        }

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
            InventoryItems.ListChanged += OnInventoryChanged;

            Load = new RelayCommand((o) =>
            {
                if (!hook.Loaded || !hook.Hooked)
                    return;

                List<ItemSpawnInfo> info = new();
                foreach (var inventory in CurrentBuild.Inventories)
                {
                    foreach (var item in inventory.Items)
                    {
                        if (item is WeaponItem weaponItem)
                        {
                            Weapon weapon = Helpers.GetWeaponFromID(weaponItem.ID);
                            if (weapon == null)
                                throw new System.Exception($"Couldnt find weapon at ID: {weaponItem.ID}");
                            info.Add(new(weapon.ID, weapon.ItemCategory, 1, weapon.MaxQuantity, weaponItem.Infusion, weaponItem.UpgradeLevel, weaponItem.SwordArtID, weapon.EventID));
                        }
                        else
                        {
                            Item item1 = Helpers.GetItemFromID(item.ID, item.Category);

                            if (item1 == null)
                                throw new System.Exception($"Couldnt find item at ID: {item.ID} With name: {item.Name}");

                            info.Add(new(item1.ID, item1.ItemCategory, 1, item1.MaxQuantity, (int)Infusion.Standard, 0, -1, item1.EventID));
                        }
                    }
                }
                hook.GetItem(info, CancellationToken.None);
            });
            RefreshBuilds = new RelayCommand((o) => 
            {
                CurrentBuild = new("New Build", new List<Inventory>() { new("Weapons", new()), new("Talismans", new()), new("Armors", new()) });
                SelectedBuild = null;
                List<Build> builds = new();
                builds.Add(new("New Build", new List<Inventory>() { new("Weapons", new()), new("Talismans", new()), new("Armors", new()) }));
                builds.AddRange(BuildSaver.getBuilds());
                BuildItemsSource = builds;
            });
            CreateNew = new RelayCommand((o) =>
            {
                BuildItemsSource = BuildSaver.getBuilds();
                CurrentBuild = new("New Build", new List<Inventory>() { new("Weapons", new()), new("Talismans", new()), new("Armors", new()) });
                SelectedBuild = null;
            });
            Save = new SavePrefab(this);

            List<ItemCategoryOption> categorys = new();
            categorys.Add(new(GetCategoryByName("Melee Weapons").Name, GetCategoryByName("Melee Weapons")));
            categorys.Add(new(GetCategoryByName("Ranged Weapons").Name, GetCategoryByName("Ranged Weapons")));
            categorys.Add(new(GetCategoryByName("Armor").Name, GetCategoryByName("Armor")));
            categorys.Add(new(GetCategoryByName("Spell Tools").Name, GetCategoryByName("Spell Tools")));
            categorys.Add(new(GetCategoryByName("Shields").Name, GetCategoryByName("Shields")));
            categorys.Add(new(GetCategoryByName("Talismans").Name, GetCategoryByName("Talismans")));
            categorys.Add(new(GetCategoryByName("DLC Melee Weapons").Name, GetCategoryByName("DLC Melee Weapons")));
            categorys.Add(new(GetCategoryByName("DLC Ranged Weapons").Name, GetCategoryByName("DLC Ranged Weapons")));
            categorys.Add(new(GetCategoryByName("DLC Armor").Name, GetCategoryByName("DLC Armor")));
            categorys.Add(new(GetCategoryByName("DLC Spell Tools").Name, GetCategoryByName("DLC Spell Tools")));
            categorys.Add(new(GetCategoryByName("DLC Shields").Name, GetCategoryByName("DLC Shields")));
            categorys.Add(new(GetCategoryByName("DLC Talismans").Name, GetCategoryByName("DLC Talismans")));


            CategoryItemsSource = categorys;

            BuildItemsSource = BuildSaver.getBuilds();

            SmithyToSomber.Add(0, 0);
            SmithyToSomber.Add(1, 0);
            SmithyToSomber.Add(2, 1);
            SmithyToSomber.Add(3, 1);
            SmithyToSomber.Add(4, 1);
            SmithyToSomber.Add(5, 2);
            SmithyToSomber.Add(6, 2);
            SmithyToSomber.Add(7, 3);
            SmithyToSomber.Add(8, 3);
            SmithyToSomber.Add(9, 3);
            SmithyToSomber.Add(10, 4);
            SmithyToSomber.Add(11, 4);
            SmithyToSomber.Add(12, 5);
            SmithyToSomber.Add(13, 5);
            SmithyToSomber.Add(14, 5);
            SmithyToSomber.Add(15, 6);
            SmithyToSomber.Add(16, 6);
            SmithyToSomber.Add(17, 7);
            SmithyToSomber.Add(18, 7);
            SmithyToSomber.Add(19, 7);
            SmithyToSomber.Add(20, 8);
            SmithyToSomber.Add(21, 8);
            SmithyToSomber.Add(22, 9);
            SmithyToSomber.Add(23, 9);
            SmithyToSomber.Add(24, 9);
            SmithyToSomber.Add(25, 10);

            SomberToSmithy.Add(0, 0);
            SomberToSmithy.Add(1, 4);
            SomberToSmithy.Add(2, 6);
            SomberToSmithy.Add(3, 9);
            SomberToSmithy.Add(4, 11);
            SomberToSmithy.Add(5, 13);
            SomberToSmithy.Add(6, 16);
            SomberToSmithy.Add(7, 19);
            SomberToSmithy.Add(8, 21);
            SomberToSmithy.Add(9, 24);
            SomberToSmithy.Add(10, 25);

            weaponSearch = new(new(), new AlphabeticalSort<Item>());
            weaponSearch.OnItemsChanged += (items) => { WeaponsSelectedItem = null; WeaponsItemsSource = items; };
            gemSearch = new(new(), new AlphabeticalSort<Gem>());
            gemSearch.OnItemsChanged += (items) => { AshesSelectedItem = null; AshesItemsSource = items; };
            infusionSearch = new(new(), new AlphabeticalSort<Infusion>());
            infusionSearch.OnItemsChanged += (items) => { InfusionsSelectedItem = null; InfusionsItemsSource = items; };

            CurrentBuild = new("New Build", new List<Inventory>() { new("Weapons", new()), new("Talismans", new()), new("Armors", new()) });
            SelectedInventoryItemsSource = CurrentBuild.Inventories;
        }

        private void OnCurrentBuildChanged(Build build)
        {
            InventoryItems.Clear();
            SelectedInventoryItemsSource = build.Inventories;
            SelectedInventoryIndex = 0;
            OnSelectedInventoryChanged();
        }
        private bool settingUpInventory = false;
        private void OnSelectedInventoryChanged()
        {
            if (SelectedInventory == null)
                return;

            if (SelectedInventory is Inventory selectedInventory)
            {
                settingUpInventory = true;
                InventoryItems.Clear();

                if (selectedInventory.Items != null)
                    foreach (var item in selectedInventory.Items)
                        if (item is WeaponItem wpnItem)
                            CreateNewBtn(item.Name,
                                        Helpers.GetImageSource(Helpers.GetFullIconID(item.IconID)),
                                        wpnItem.UpgradeLevel.ToString(),
                                        string.IsNullOrWhiteSpace(wpnItem.InfusionIconID) ? null : Helpers.GetImageSource(wpnItem.InfusionIconID.ToString()),
                                        Helpers.GetImageSource(Helpers.GetFullIconID(wpnItem.GemIconID)), item);
                        else
                            CreateNewBtn(item.Name, 
                                        Helpers.GetImageSource(Helpers.GetFullIconID(item.IconID)),
                                        string.Empty, null, null, item);
                settingUpInventory = false;
            }
        }


        public ItemCategory GetCategoryByName(string name)
        {
            return ItemCategory.All.FirstOrDefault(x => x.Name == name);
        }
        private void SetupCommands()
        {
            AddItem = new RelayCommand((o) => { AddItemToInventory(); });
        }

        private void AddItemToInventory()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (SelectedInventory == null)
                return;

            if (WeaponsSelectedItem == null)
                return;

            Inventory selectedInventory = SelectedInventory as Inventory;
            for (int i = 0; i < Quantity; i++)
            {
                if (WeaponsSelectedItem is SearchItem<Item> item)
                {
                    if (item.Item is Weapon weapon)
                    {
                        if (selectedInventory.Name.ToLower() != "weapons")
                        {
                            SelectedInventoryIndex = 0;
                        }
                        if ((SelectedInventory as Inventory).Name.ToLower() != "weapons")
                            return;
                        var gem = AshesSelectedItem as SearchItem<Gem>;
                        var infusion = InfusionsSelectedItem as SearchItem<Infusion>;

                        WeaponItem wpnItem = new(weapon.Name, weapon.ID, weapon.IconID, weapon.ItemCategory.ToString(),
                            infusion != null ? (int)infusion.Item : (int)Infusion.Standard,
                            infusion != null ? infusion.Item.ToString() : "",
                            gem != null ? gem.Item.ID : -1,
                            gem != null ? gem.Item.IconID : (short)0,
                            UpgradeLevel);

                        CreateNewBtn(weapon.Name,
                                    Helpers.GetImageSource(Helpers.GetFullIconID(wpnItem.IconID)),
                                    wpnItem.UpgradeLevel.ToString(),
                                    string.IsNullOrWhiteSpace(wpnItem.InfusionIconID) ? null : Helpers.GetImageSource(wpnItem.InfusionIconID),
                                    wpnItem.GemIconID != 0 ? Helpers.GetImageSource(Helpers.GetFullIconID(wpnItem.GemIconID)) : null, wpnItem);
                    }
                    else
                    {
                        BuildItem buildItem = new(item.Item.Name, item.Item.ID, item.Item.IconID, item.Item.ItemCategory.ToString());
                        if (item.Item.ItemCategory == Item.Category.Accessory)
                        {
                            if (selectedInventory.Name.ToLower() != "talismans")
                            {
                                SelectedInventoryIndex = 1;
                            }
                            if ((SelectedInventory as Inventory).Name.ToLower() != "talismans")
                                return;
                            CreateNewBtn(item.Item.Name,
                                     Helpers.GetImageSource(Helpers.GetFullIconID(item.Item.IconID)),
                                     string.Empty, null, null, buildItem);
                        }
                        else if (item.Item.ItemCategory == Item.Category.Protector)
                        {
                            if (selectedInventory.Name.ToLower() != "armors")
                            {
                                SelectedInventoryIndex = 2;
                            }
                            if ((SelectedInventory as Inventory).Name.ToLower() != "armors")
                                return;
                            CreateNewBtn(item.Item.Name,
                                     Helpers.GetImageSource(Helpers.GetFullIconID(item.Item.IconID)),
                                     string.Empty, null, null, buildItem);
                        }


                    }


                }
            }
        }

        public void OnSaveEditedItem(WeaponItem prefab, InventoryItem item)
        {
            if (prefab == null)
                return;

            if (item == null)
                return;

            if (!InventoryItems.Contains(item))
                return;

            item.Item = prefab;
            item.ItemIconPath = Helpers.GetImageSource(Helpers.GetFullIconID(prefab.IconID));
            item.UpgradeLevel = prefab.UpgradeLevel.ToString();
            item.InfusionIconPath = string.IsNullOrEmpty(prefab.InfusionIconID) ? null : Helpers.GetImageSource(prefab.InfusionIconID);
            item.AshOfWarIcon = prefab.GemIconID == 0 ? null : Helpers.GetImageSource(Helpers.GetFullIconID(prefab.GemIconID));

            OnInventoryChanged(null, null);
        }
        private void CreateNewBtn(string name, ImageSource icon, string upgradelevel, ImageSource infusionicon, ImageSource ashIcon, BuildItem item)
        {
            InventoryItem newInvItem = new();
            newInvItem.ItemName = name;
            newInvItem.ItemIconPath = icon;
            newInvItem.UpgradeLevel = upgradelevel;
            newInvItem.InfusionIconPath = infusionicon;
            newInvItem.AshOfWarIcon = ashIcon;
            newInvItem.Item = item;
            
            newInvItem.MouseRightButtonDown += (o, e) => 
            {
                if (newInvItem.Item == null)
                    return;

                if (newInvItem.Item is WeaponItem wpn)
                {
                    if (!Hook.Loaded || !Hook.Setup)
                    {
                        InformationDialog info = new("You cannot edit items while you are not attached. Please attach to Elden Ring first.");
                        info.ShowDialog();
                        return;
                    }

                    EditItemDialog dialog = new();
                    dialog.Prefab = newInvItem.Item as WeaponItem;
                    dialog.ItemIcon.Source = icon;
                    dialog.OnSubmit += (p) =>
                    {
                        OnSaveEditedItem(p, newInvItem);
                    };

                    dialog.ShowDialog();
                }
            };

            newInvItem.MouseDown += (s, e) => 
            {
                if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
                {
                    if (InventoryItems.Contains(newInvItem))
                        InventoryItems.Remove(newInvItem);
                }
            };
            InventoryItems.Add(newInvItem);
        }

        private void OnInventoryChanged(object? sender, ListChangedEventArgs e)
        {
            if (settingUpInventory)
                return;

            if (SelectedInventory == null)
                return;

            if (InventoryItems == null)
                return;

            Inventory selectedInventory = SelectedInventory as Inventory;
            selectedInventory.Items = new();
            foreach(var invItem in InventoryItems)
            {
                selectedInventory.Items.Add(invItem.Item);
            }
        }

        private void OnBuildChanged()
        {
            if (SelectedBuild == null)
                return;

            CurrentBuild = SelectedBuild as Build;
        }

        private void OnCategoryChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            

            if (CategorySelectedItem == null)
                return;

            weaponSearch.Items = null;
            gemSearch.Items = null;
            infusionSearch.Items = null;

            ItemCategoryOption option = CategorySelectedItem as ItemCategoryOption;
            List<SearchItem<Item>> items = new();

            foreach (var item in option.category.Items)
                items.Add(new(item, item.Name));

            weaponSearch.Items = items;
            WeaponsSearchText = string.Empty;
        }

        private bool lastWasInfusible = true;
        private void OnItemSelectedChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (WeaponsSelectedItem == null)
                return;

            gemSearch.Items = null;
            infusionSearch.Items = null;

            SearchItem<Item> item = WeaponsSelectedItem as SearchItem<Item>;
            if (item.Item is Weapon weapon)
            {
                MaxUpgradeLevel = weapon.MaxUpgrade;
                if (weapon.Infusible)
                {
                    List<SearchItem<Gem>> gemOptions = new();
                    foreach (Gem gem in Gem.All)
                    {
                        if (gem.WeaponTypes.Contains(weapon.Type))
                        {
                            gemOptions.Add(new(gem, gem.Name.Contains("Ash of War: ") ? gem.Name.Substring(12) : gem.Name));
                        }
                    }
                    MaxUpgradeLevel = 25;
                    gemSearch.Items = gemOptions;
                    if (!lastWasInfusible)
                        UpgradeLevel = SomberToSmithy[UpgradeLevel];
                    UpgradeLevelString = UpgradeLevel.ToString();
                    lastWasInfusible = true;
                }
                else
                {
                    MaxUpgradeLevel = 10;
                    if (lastWasInfusible)
                        UpgradeLevel = SmithyToSomber[UpgradeLevel];
                    UpgradeLevelString = UpgradeLevel.ToString();
                    lastWasInfusible = false;
                }
            }
        }

        private void OnAshSelectedChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (AshesSelectedItem == null)
                return;

            infusionSearch.Items = null;

            SearchItem<Gem> option = AshesSelectedItem as SearchItem<Gem>;
            List<SearchItem<Infusion>> infusionOptions = new();
            foreach (Infusion infusion in option.Item.Infusions)
                infusionOptions.Add(new(infusion, infusion.ToString()));
            infusionSearch.Items = infusionOptions;
        }
    }
}
