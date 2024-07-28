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
using PvPHelper.Core.Extensions;

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

        private BindingList<InventoryItemModel> _inventoryItems;

        public BindingList<InventoryItemModel> InventoryItems
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

        private SearchAlgorithm<Item> weaponSearch;
        private SearchAlgorithm<Gem> gemSearch;
        private SearchAlgorithm<NamedObject<Infusion>> infusionSearch;

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

            weaponSearch = new(new List<Item>(), new AlphabeticalSort<Item>());
            weaponSearch.OnItemsChanged += (items) => { WeaponsSelectedItem = null; WeaponsItemsSource = items; };
            gemSearch = new(new List<Gem>(), new AlphabeticalSort<Gem>());
            gemSearch.OnItemsChanged += (items) => { AshesSelectedItem = null; AshesItemsSource = items; };
            infusionSearch = new(new List<NamedObject<Infusion>>(), new AlphabeticalSort<NamedObject<Infusion>>());
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
                if (WeaponsSelectedItem is Item item)
                {
                    if (item is Weapon weapon)
                    {
                        if (selectedInventory.Name.ToLower() != "weapons")
                        {
                            SelectedInventoryIndex = 0;
                        }
                        if ((SelectedInventory as Inventory).Name.ToLower() != "weapons")
                            return;
                        var gem = AshesSelectedItem as Gem;
                        var infusion = InfusionsSelectedItem as NamedObject<Infusion>;

                        WeaponItem wpnItem = new(weapon.Name, weapon.ID, weapon.IconID, weapon.ItemCategory.ToString(),
                            infusion != null ? (int)infusion.Value : (int)Infusion.Standard,
                            infusion != null ? infusion.ToString() : "",
                            gem != null ? gem.ID : -1,
                            gem != null ? gem.IconID : (short)0,
                            UpgradeLevel);

                        CreateNewBtn(weapon.Name,
                                    Helpers.GetImageSource(Helpers.GetFullIconID(wpnItem.IconID)),
                                    wpnItem.UpgradeLevel.ToString(),
                                    string.IsNullOrWhiteSpace(wpnItem.InfusionIconID) ? null : Helpers.GetImageSource(wpnItem.InfusionIconID),
                                    wpnItem.GemIconID != 0 ? Helpers.GetImageSource(Helpers.GetFullIconID(wpnItem.GemIconID)) : null, wpnItem);
                    }
                    else
                    {
                        BuildItem buildItem = new(item.Name, item.ID, item.IconID, item.ItemCategory.ToString());
                        if (item.ItemCategory == Item.Category.Accessory)
                        {
                            if (selectedInventory.Name.ToLower() != "talismans")
                            {
                                SelectedInventoryIndex = 1;
                            }
                            if ((SelectedInventory as Inventory).Name.ToLower() != "talismans")
                                return;
                            CreateNewBtn(item.Name,
                                     Helpers.GetImageSource(Helpers.GetFullIconID(item.IconID)),
                                     string.Empty, null, null, buildItem);
                        }
                        else if (item.ItemCategory == Item.Category.Protector)
                        {
                            if (selectedInventory.Name.ToLower() != "armors")
                            {
                                SelectedInventoryIndex = 2;
                            }
                            if ((SelectedInventory as Inventory).Name.ToLower() != "armors")
                                return;
                            CreateNewBtn(item.Name,
                                     Helpers.GetImageSource(Helpers.GetFullIconID(item.IconID)),
                                     string.Empty, null, null, buildItem);
                        }


                    }


                }
            }
        }

        public void OnSaveEditedItem(WeaponItem prefab, InventoryItemModel item)
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
            InventoryItemModel newInvItem = new(item, icon, ashIcon, infusionicon, item.Name, upgradelevel);
            
            newInvItem.OnRightClick += (item) => 
            {
                if (item == null)
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

            newInvItem.OnMiddleClick += (item) => 
            {
                if (InventoryItems.Contains(newInvItem))
                    InventoryItems.Remove(newInvItem);
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
            List<Item> items = new();

            foreach (var item in option.category.Items)
                items.Add(item);

            weaponSearch.Items = items;
            WeaponsSearchText = string.Empty;
        }

        private bool lastWasInfusible = false;
        private void OnItemSelectedChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (WeaponsSelectedItem == null)
                return;

            gemSearch.Items = null;
            infusionSearch.Items = null;

            Item item = WeaponsSelectedItem as Item;
            if (item is Weapon weapon)
            {
                MaxUpgradeLevel = weapon.MaxUpgrade;
                if (weapon.GemAttachType == GemMountType.Any)
                {
                    List<Gem> gemOptions = new();
                    foreach (Gem gem in Gem.All)
                    {
                        if (gem.WeaponTypes.Contains(weapon.Type))
                        {
                            gemOptions.Add(gem);
                        }
                    }
                    gemSearch.Items = gemOptions;
                }

                if (lastWasInfusible && weapon.IsSomber())
                {
                    UpgradeLevel = Helpers.GetSomberLevel(UpgradeLevel);
                }
                else if (!lastWasInfusible && !weapon.IsSomber())
                {
                    UpgradeLevel = Helpers.GetSmithLevel(UpgradeLevel);
                }

                lastWasInfusible = !weapon.IsSomber();

                UpgradeLevelString = UpgradeLevel.ToString();
            }
        }

        private void OnAshSelectedChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (AshesSelectedItem == null)
                return;

            infusionSearch.Items = null;

            Gem option = AshesSelectedItem as Gem;
            List<NamedObject<Infusion>> infusionOptions = new();
            foreach (Infusion infusion in option.Infusions)
                infusionOptions.Add(new(infusion, infusion.ToString()));
            infusionSearch.Items = infusionOptions;

        }
    }
}
