using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommandManager = PvPHelper.Console.CommandManager;
using PvPHelper.MVVM.Commands.Dashboard.Toggles;
using System.Windows.Threading;
using PvPHelper.MVVM.Models;
using System.Drawing;
using PvPHelper.MVVM.Views;
using Erd_Tools.Models;
using static Erd_Tools.Models.Weapon;

namespace PvPHelper.MVVM.ViewModels
{
    internal class PrefabCreatorViewModel : ViewModelBase
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
            set { _categorySelectedItem = value; OnPropertyChanged(); OnCategoryChanged(); }
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
            set { _selectedInventory = value; OnPropertyChanged(); }
        }
        private IEnumerable<object> _inventoryItems;

        public IEnumerable<object> InventoryItems
        {
            get { return _inventoryItems; }
            set { _inventoryItems = value; OnPropertyChanged(); }
        }
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

        private ErdHook Hook;

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
        }

        private void SetupCommands()
        {
            AddItem = new RelayCommand((o) => { AddItemToInventory(); });
        }
        private void AddItemToInventory()
        {

        }

        private void OnCategoryChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (CategorySelectedItem == null)
                return;

            AshesItemsSource = Enumerable.Empty<object>();
            InfusionsItemsSource = Enumerable.Empty<object>();

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

            if (WeaponsSelectedItem == null)
                return;

            InfusionsItemsSource = Enumerable.Empty<object>();

            OriginAshes = null;
            OriginInfusions = null;

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
                    MaxUpgradeLevel = 10;
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
