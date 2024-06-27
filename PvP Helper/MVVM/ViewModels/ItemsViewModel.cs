using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using PvPHelper.MVVM.Models;
using Erd_Tools.Models;
using System.IO;
using System.Reflection;
using System.Threading;
using Erd_Tools.Models.Items;
using static Erd_Tools.Models.Weapon;
using PvPHelper.MVVM.Commands.Items;
using PvPHelper.Core.Extensions;

namespace PvPHelper.MVVM.ViewModels
{
    internal class ItemsViewModel : ViewModelBase
    {
        #region Data Bindings
        #region stats
        private string _chrName = "Name";

        public string ChrName
        {
            get { return _chrName; }
            set { _chrName = value; OnPropertyChanged(); }
        }

        private string _chrLevel = "Lvl: ";

        public string ChrLevel
        {
            get { return _chrLevel; }
            set { _chrLevel = value; OnPropertyChanged(); }
        }
        private string _vigor = "Vigor: ";

        public string Vigor
        {
            get { return _vigor; }
            set { _vigor = value; OnPropertyChanged(); }
        }
        private string _mind = "Mind: ";

        public string Mind
        {
            get { return _mind; }
            set { _mind = value; OnPropertyChanged(); }
        }
        private string _endurance = "Endurance: ";

        public string Endurance
        {
            get { return _endurance; }
            set { _endurance = value; OnPropertyChanged(); }
        }
        private string _strength = "Strength: ";

        public string Strength
        {
            get { return _strength; }
            set { _strength = value; OnPropertyChanged(); }
        }
        private string _dexterity = "Dexterity: ";

        public string Dexterity
        {
            get { return _dexterity; }
            set { _dexterity = value; OnPropertyChanged(); }
        }
        private string _int = "Intelligence: ";

        public string Intelligence
        {
            get { return _int; }
            set { _int = value; OnPropertyChanged(); }
        }
        private string _faith = "Faith: ";

        public string Faith
        {
            get { return _faith; }
            set { _faith = value; OnPropertyChanged(); }
        }
        private string _arcane = "Arcane: ";

        public string Arcane
        {
            get { return _arcane; }
            set { _arcane = value; OnPropertyChanged(); }
        }
        #endregion
        public ICommand RefreshStats { get; set; }
        public ICommand AddRunes { get; set; }
        public ICommand Gib { get; set; }
        #region CustomGibs
        public ICommand AllFlaskUpgrades { get; set; }
        public ICommand AllTalismanPouches { get; set; }
        public ICommand AllMemoryStones { get; set; }
        public ICommand AllMapsGraces { get; set; }
        public ICommand AllLimitedItems { get; set; }
        public ICommand AllWhetblades { get; set; }
        #endregion
        #region NormalMassGib Commands
        public ICommand AllCookbooks { get; set; }
        public ICommand AllTools { get; set; }
        public ICommand AllMerchantItems { get; set; }
        public ICommand AllCraftingMaterials { get; set; }
        public ICommand AllCrystalTears { get; set; }
        public ICommand AllAshes { get; set; }
        public ICommand AllConsumables { get; set; }
        public ICommand AllSpells { get; set; }
        public ICommand AllAmmo { get; set; }
        public ICommand AllUpgradeMaterials { get; set; }
        #endregion
        #region SecondGroup ItemGibs
        public ICommand AllMeleeWeapons { get; set; }
        public ICommand AllRangedWeapons { get; set; }
        public ICommand AllShields { get; set; }
        public ICommand AllArmor { get; set; }
        public ICommand AllSpellTools { get; set; }
        public ICommand AllTalismans { get; set; }
        #endregion

        private int _massGibUpgLevel = 25;

        public int MassGibUpgLevel
        {
            get { return _massGibUpgLevel; }
            set
            {
                _massGibUpgLevel = value;
                MassUpgradeLevel = value;
                OnPropertyChanged();
            }
        }
        private string _massGibUpgString;

        public string MassGibUpgLvlString
        {
            get { return _massGibUpgString; }
            set { _massGibUpgString = value; OnPropertyChanged(); }
        }

        #region RunesSection
        private string _currHeldRunes;

        public string CurrHeldRunes
        {
            get { return _currHeldRunes; }
            set { _currHeldRunes = value; OnPropertyChanged(); }
        }

        private int _maxRune;

        public int MaxRunes
        {
            get { return _maxRune; }
            set { _maxRune = value; OnPropertyChanged(); }
        }
        private int _runesToAdd;

        public int CurrRunesToAdd
        {
            get { return _runesToAdd; }
            set
            {
                _runesToAdd = value;
                OnPropertyChanged();
                CurrRunesToAddText = value.ToString();
                if (value < 999999999 - _player.GetRunes())
                    MaxChecked = false;
            }
        }
        private string _currRunesToAddText;

        public string CurrRunesToAddText
        {
            get { return _currRunesToAddText; }
            set { _currRunesToAddText = value; OnPropertyChanged(); }
        }

        private bool _maxChecked;

        public bool MaxChecked
        {
            get { return _maxChecked; }
            set
            {
                if (value && (!_hook.Loaded || !_hook.Hooked))
                    MaxChecked = false;
                _maxChecked = value;
                OnPropertyChanged();
                if (value && _hook.Loaded && _hook.Hooked)
                {
                    CurrRunesToAdd = 999999999 - _player.GetRunes();
                }

                if (_hook.Loaded && _hook.Hooked)
                    MaxRunes = 999999999 - _player.GetRunes();
            }
        }
        #endregion

        #region ItemGib Section
        private IEnumerable<object> _categoryItemsSource;

        public IEnumerable<object> CategoryItemsSource
        {
            get { return _categoryItemsSource; }
            set { _categoryItemsSource = value; OnPropertyChanged(); }
        }

        private IEnumerable<object> _weaponsItemsSource;

        public IEnumerable<object> WeaponsItemsSource
        {
            get { return _weaponsItemsSource; }
            set 
            { 
                _weaponsItemsSource = value;
                if (OriginWeapons == null)
                    OriginWeapons = value.ToList();
                OnPropertyChanged(); 
            }
        }
        private IEnumerable<object> _infusionsItemsSource;

        public IEnumerable<object> InfusionsItemsSource
        {
            get { return _infusionsItemsSource; }
            set
            {
                _infusionsItemsSource = value;
                OnPropertyChanged();
                if (OriginInfusions == null)
                    OriginInfusions = value.ToList();
            }
        }
        private IEnumerable<object> _ashesItemsSource;

        public IEnumerable<object> AshesItemsSource
        {
            get { return _ashesItemsSource; }
            set 
            { 
                _ashesItemsSource = value; 
                OnPropertyChanged();
                if (OriginAshes == null)
                    OriginAshes = value.ToList();
            }
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

        private object _weaponsSelectedItem;

        public object WeaponsSelectedItem
        {
            get { return _weaponsSelectedItem; }
            set
            {
                _weaponsSelectedItem = value;
                OnPropertyChanged();
                OnItemChanged();
            }
        }
        private object _infusionsSelectedItem;

        public object InfusionsSelectedItem
        {
            get { return _infusionsSelectedItem; }
            set
            {
                _infusionsSelectedItem = value;
                OnPropertyChanged();
                OnInfusionChanged();
            }
        }
        private object _ashesSelectedItem;

        public object AshesSelectedItem
        {
            get { return _ashesSelectedItem; }
            set
            {
                _ashesSelectedItem = value;
                OnPropertyChanged();
                OnAshChanged();
            }
        }
        private int _categorySelectedIndex;

        public int CategorySelectedIndex
        {
            get { return _categorySelectedIndex; }
            set { _categorySelectedIndex = value; OnPropertyChanged(); }
        }
        private int _upgradeLvl;

        public int UpgradeLevel
        {
            get { return _upgradeLvl; }
            set { _upgradeLvl = value; OnPropertyChanged(); }
        }
        private int _maxUpgradeLvl;

        public int MaxUpgradeLvl
        {
            get { return _maxUpgradeLvl; }
            set 
            { 
                _maxUpgradeLvl = value; 
                OnPropertyChanged();
                if (UpgradeLevel > MaxUpgradeLvl)
                {
                    UpgradeLevel = MaxUpgradeLvl;
                    UpgradeLvlText = MaxUpgradeLvl.ToString();
                }
            }
        }

        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set 
            { 
                _quantity = value; 
                OnPropertyChanged();
            }
        }
        private int _maxQuantity;

        public int MaxQuantity
        {
            get { return _maxQuantity; }
            set 
            { 
                _maxQuantity = value; 
                OnPropertyChanged();
                if (Quantity > MaxQuantity)
                {
                    Quantity = MaxQuantity;
                    QuantityText = MaxQuantity.ToString();
                }
            }
        }
        private string _upgradeLvlText;

        public string UpgradeLvlText
        {
            get { return _upgradeLvlText; }
            set { _upgradeLvlText = value; OnPropertyChanged(); }
        }
        private string _quantityText;

        public string QuantityText
        {
            get { return _quantityText; }
            set { _quantityText = value; OnPropertyChanged(); }
        }
        private List<object> _originWeapons;

        public List<object> OriginWeapons
        {
            get { return _originWeapons; }
            set { _originWeapons = value; OnPropertyChanged(); }
        }
        private List<object> _originInfusions;

        public List<object> OriginInfusions
        {
            get { return _originInfusions; }
            set { _originInfusions = value; OnPropertyChanged(); }
        }
        private List<object> _originAshes;

        public List<object> OriginAshes
        {
            get { return _originAshes; }
            set { _originAshes = value; OnPropertyChanged(); }
        }

        private int _ashesSelectedIndex;

        public int AshesSelectedIndex
        {
            get { return _ashesSelectedIndex; }
            set { _ashesSelectedIndex = value; OnPropertyChanged(); }
        }

        private bool _isOverrideChecked;

        public bool IsOverrideChecked
        {
            get { return _isOverrideChecked; }
            set { _isOverrideChecked = value; OnPropertyChanged(); }
        }

        #endregion
        #endregion

        private ErdHook _hook;
        private Player _player;

        private List<int> mapEventFlagIDs = new();
        public static int MassUpgradeLevel = 25;
        private DispatcherTimer runesTimer;

        private List<ItemCategoryOption> allItemCategorys = new();
        public ItemsViewModel(ErdHook hook)
        {
            _hook = hook;
            _player = new(hook.PlayerIns, hook);

            _hook.OnSetup += _hook_OnSetup;
            _hook.OnUnhooked += _hook_OnUnhooked;
            SetupCommands();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "PvPHelper.Resources.MapEventFlagIds.txt";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                mapEventFlagIDs = Helpers.EnumerateLines(reader).Select(x => Convert.ToInt32(x.Substring(0, 5))).ToList();
            }

            runesTimer = new();
            runesTimer.Interval = TimeSpan.FromSeconds(1);
            runesTimer.Tick += RunesTimer_Tick;

            MaxRunes = 999999999;

            foreach (ItemCategory category in ItemCategory.All)
            {
                allItemCategorys.Add(new(category.Name, category));
            }
            CategoryItemsSource = allItemCategorys;
            CategorySelectedItem = null;

            Quantity = 0;
            QuantityText = "0";

            UpgradeLevel = 0;
            UpgradeLvlText = "0";

            MassGibUpgLevel = 0;
            MassGibUpgLvlString = "0";
            CurrRunesToAdd = 0;
            CurrRunesToAddText = "0";
        }

        private void RunesTimer_Tick(object? sender, EventArgs e)
        {
            if (_hook.Loaded && _hook.Hooked)
            {
                CurrHeldRunes = _player.GetRunes().ToString();
            }
        }

        private void _hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => 
            { 
                SetStats(); 
                runesTimer.Start();
                if (CategorySelectedItem != null)
                {
                    List<ItemGibOption> gibOptions = new();
                    foreach (Item item in ItemCategory.All[0].Items)
                    {
                        gibOptions.Add(new(item.Name, ItemCategory.All[0].Name, item));
                    }
                    WeaponsItemsSource = gibOptions;
                }
            });
        }
        private void _hook_OnUnhooked(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => { runesTimer.Stop(); });
        }
        private void SetupCommands()
        {
            RefreshStats = new RelayCommand((o) => { SetStats(); });

            AllFlaskUpgrades = new RelayCommand((o) => { FlaskUpgrades(); });
            AllTalismanPouches = new RelayCommand((o) =>
            {
                if (!_hook.Hooked || !_hook.Loaded)
                    return;
                ItemCategory upgradeMats = ItemCategory.All.FirstOrDefault(x => x.Name == "Upgrade Materials");
                Item tali = upgradeMats.Items.FirstOrDefault(x => x.Name == "Talisman Pouch");

                _hook.GetItem(new(tali.ID, tali.ItemCategory, 3, tali.MaxQuantity, 0, 0, -1, tali.EventID));
            });
            AllMemoryStones = new RelayCommand((o) =>
            {
                if (!_hook.Hooked || !_hook.Loaded)
                    return;
                ItemCategory upgradeMats = ItemCategory.All.FirstOrDefault(x => x.Name == "Upgrade Materials");
                Item stone = upgradeMats.Items.FirstOrDefault(x => x.Name == "Memory Stone");

                _hook.GetItem(new(stone.ID, stone.ItemCategory, 8, stone.MaxQuantity, 0, 0, -1, stone.EventID));
            });
            AllMapsGraces = new RelayCommand((o) =>
            {
                if (!_hook.Hooked || !_hook.Loaded)
                    return;

                foreach (int flag in mapEventFlagIDs)
                {
                    _hook.SetEventFlag(flag, true);
                }

                foreach (Continent continent in Continent.All)
                {
                    foreach (Hub hub in continent.Hubs)
                    {
                        foreach (Grace grace in hub.Graces)
                        {
                            _hook.SetEventFlag(grace.EventFlagID, true);
                        }
                    }
                }
            });
            AllLimitedItems = new RelayCommand((o) => { LimitedItems(); });
            AllWhetblades = new RelayCommand((o) => { Whetblades(); });

            AllCookbooks = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Cookbooks"), this);
            AllTools = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Tools"), this);
            AllMerchantItems = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Merchant Items"), this);
            AllCraftingMaterials = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Crafting Materials"), this, new string[] { "Cracked Pot", "Ritual Pot", "Celestial Dew" }, newMax: 999);
            AllCrystalTears = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Crystal Tears"), this);
            AllAshes = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Gems"), this, single: true);
            AllConsumables = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Consumables"), this,
            new string[] { "Cracked Pot", "Ritual Pot", "Perfume Bottle",
            "Remembrance of the Regal Ancestor","Remembrance of the Naturalborn","Remembrance of the Lichdragon",
            "Remembrance of the Fire Giant", "Remembrance of the Grafted", "Remembrance of the Full Moon Queen",
            "Remembrance of the Blasphemous","Remembrance of the Starscourge","Remembrance of the Omen King",
            "Remembrance of the Blood Lord", "Remembrance of the Rot Goddess", "Remembrance of the Black Blade",
            "Remembrance of Hoarah Loux", "Remembrance of the Dragonlord", "Elden Remembrance", "Shabriri Grape",
            "Baldachin's Blessing", "Radiant Baldachin's Blessing"}, single: true, 999);
            AllSpells = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Magic"), this);
            AllAmmo = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Ammo"), this, single: true, newMax: 999);
            AllUpgradeMaterials = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Upgrade Materials"), this, new string[] { "Golden Seed", "Sacred Tear", "Memory Stone", "Talisman Pouch" }, single: true);

            AllMeleeWeapons = new MassWeaponGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Melee Weapons"));
            AllRangedWeapons = new MassWeaponGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Ranged Weapons"));
            AllShields = new MassWeaponGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Shields"));
            AllArmor = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Armor"), this);
            AllSpellTools = new MassWeaponGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Spell Tools"));
            AllTalismans = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans"), this, single: true);

            AddRunes = new RelayCommand((o) =>
            {
                if (!_hook.Loaded || !_hook.Hooked)
                    return;

                _player.AddRunes(CurrRunesToAdd);
            });
            Gib = new RelayCommand((o) => { GibItem(); });
        }
        public void SetStats()
        {
            if (!_hook.Loaded)
                return;

            ChrName = _player.PlayerName;
            ChrLevel = "Lvl: " + _player.GetLevel().ToString();

            Vigor = "Vigor: " + _player.GetVigor().ToString();
            Mind = "Mind: " + _player.GetMind().ToString();
            Endurance = "Endurance: " + _player.GetEndurance().ToString();
            Strength = "Strength: " + _player.GetStrength().ToString();
            Dexterity = "Dexterity: " + _player.GetDexterity().ToString();
            Intelligence = "Intelligence: " + _player.GetIntelligence().ToString();
            Faith = "Faith: " + _player.GetFaith().ToString();
            Arcane = "Arcane: " + _player.GetArcane().ToString();
        }

        private void FlaskUpgrades()
        {
            if (!_hook.Hooked || !_hook.Loaded)
                return;
            ItemCategory upgradeMats = ItemCategory.All.FirstOrDefault(x => x.Name == "Upgrade Materials");
            ItemCategory flasks = ItemCategory.All.FirstOrDefault(x => x.Name == "Flasks");

            Item goldenSeed = upgradeMats.Items.FirstOrDefault(x => x.Name == "Golden Seed");
            Item sacredTear = upgradeMats.Items.FirstOrDefault(x => x.Name == "Sacred Tear");
            Item physic = flasks.Items.FirstOrDefault(x => x.Name == "Flask of Wondrous Physick (Empty)");

            _hook.GetItem(new(goldenSeed.ID, goldenSeed.ItemCategory, 30, goldenSeed.MaxQuantity, 0, 0, -1, goldenSeed.EventID));
            _hook.GetItem(new(sacredTear.ID, sacredTear.ItemCategory, 12, sacredTear.MaxQuantity, 0, 0, -1, sacredTear.EventID));
            
            if (!Helpers.InventoryContainsItem((List<InventoryEntry>)_hook.GetInventory(), "Flask of Wondrous Physick"))
                _hook.GetItem(new(physic.ID, physic.ItemCategory, 1, physic.MaxQuantity, 0, 0, -1, physic.EventID));
        }
        private void LimitedItems()
        {
            if (!_hook.Hooked || !_hook.Loaded)
                return;
            string[] itemsToAdd = new string[] { "Cracked Pot", "Ritual Pot", "Perfume Bottle", "Duelist's Furled Finger", "White Cipher Ring", "Blue Cipher Ring",
            "Taunter's Tounge", "Recusant Finger", "Bloody Finger", "Crafting Kit"};
            ItemCategory keyItemsCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Key Items");
            ItemCategory consumablesCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Consumables");

            List<ItemSpawnInfo> keyItems = new();
            List<ItemSpawnInfo> consumables = new();

            foreach (Item item in keyItemsCat.Items)
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

            _hook.GetItem(keyItems, CancellationToken.None);
            _hook.GetItem(consumables, CancellationToken.None);
        }
        int[] whetbladeFlags = new int[] { 65600, 65610, 65620, 65630, 65640, 65650, 65660, 65670, 65680, 65690, 65700, 65710, 65720 };
        private void Whetblades()
        {
            if (!_hook.Hooked || !_hook.Loaded)
                return;

            ItemCategory cat = ItemCategory.All.FirstOrDefault(x => x.Name == "Key Items");

            if (cat!= null)
            {
                Item whetstoneKnife = cat.Items.FirstOrDefault(x => x.ID == 8590);
                _hook.GetItem(new(whetstoneKnife.ID, whetstoneKnife.ItemCategory, 1, whetstoneKnife.MaxQuantity, (int)Infusion.Standard, 0, -1, whetstoneKnife.EventID));
            }
            foreach (int whetblade in whetbladeFlags)
                _hook.SetEventFlag(whetblade, true);
        }

        private void OnCategoryChanged()
        {
            if (!_hook.Loaded || !_hook.Hooked)
                return;

            if (CategorySelectedItem == null)
                return;

            AshesItemsSource = Enumerable.Empty<object>();
            InfusionsItemsSource = Enumerable.Empty<object>();
            OriginAshes = null;
            OriginInfusions = null;

            ItemCategoryOption option = CategorySelectedItem as ItemCategoryOption;
            List<ItemGibOption> gibOptions = new();

            foreach (var item in option.category.Items)
                gibOptions.Add(new(item.Name, option.Name, item));

            OriginWeapons = null;
            WeaponsItemsSource = gibOptions;
            
        }
        private string[] allowedCatsForUpgrade = new string[] { "Melee Weapons", "Ranged Weapons", "Spell Tools", "Shields", "DLC Melee Weapons", "DLC Ranged Weapons", "DLC Spell Tools", "DLC Shields" };
        private void OnItemChanged()
        {
           if (!_hook.Loaded || !_hook.Hooked)
                return;

            if (WeaponsSelectedItem == null)
                return;

            AshesItemsSource = Enumerable.Empty<object>();
            OriginAshes = null;
            InfusionsItemsSource = Enumerable.Empty<object>();
            OriginInfusions = null;

            ItemGibOption option = WeaponsSelectedItem as ItemGibOption;
            List<GemOption> gemOptions = new();
            if (option.item is Weapon weapon)
            {
                if (!weapon.Unique)
                {
                    foreach (Gem gem in Gem.All)
                    {
                        if (gem.WeaponTypes.Contains(weapon.Type))
                        {
                            var newGem = new GemOption(gem.Name.Contains("Ash of War: ") ? gem.Name.Substring(12) : gem.Name, gem);
                            gemOptions.Add(newGem);
                        }
                    }
                    MaxUpgradeLvl = 25;
                    AshesItemsSource = gemOptions;
                }
                else
                {
                    MaxUpgradeLvl = 10;
                }
            }
            else
            {
                MaxUpgradeLvl = 0;
                UpgradeLevel = 0;
                UpgradeLvlText = "0";
            }
            MaxQuantity = IsOverrideChecked ? (option.item is Weapon ? 10 : 999) : option.item.MaxQuantity;
        }
        private void OnInfusionChanged()
        {
            if (!_hook.Loaded || !_hook.Hooked)
                return;
        }
        private void OnAshChanged()
        {
            if (!_hook.Loaded || !_hook.Hooked)
                return;

            if (AshesSelectedItem == null)
                return;

            InfusionsItemsSource = Enumerable.Empty<object>();
            OriginInfusions = null;

            GemOption option = AshesSelectedItem as GemOption;
            List<InfusionOption> infOptions = new();
            foreach (var infusion in option.gem.Infusions)
                infOptions.Add(new(infusion.ToString(), infusion));
            InfusionsItemsSource = infOptions;
        }
        private void GibItem()
        {
            if (!_hook.Loaded || !_hook.Hooked)
                return;

            if (WeaponsSelectedItem == null)
                return;

            ItemGibOption option = (ItemGibOption)WeaponsSelectedItem;
            if (option.item is Weapon weapon && allowedCatsForUpgrade.Contains(option.CatName))
            {
                if (weapon.Infusible)
                {
                    GemOption gemOption = AshesSelectedItem as GemOption;
                    InfusionOption infusionOption = InfusionsSelectedItem as InfusionOption;

                    _hook.GetItem(new(weapon.ID, weapon.ItemCategory, Quantity, weapon.MaxQuantity, infusionOption != null ? (int)infusionOption.infusion : (int)Infusion.Standard, UpgradeLevel, gemOption != null ? gemOption.gem.ID : -1, weapon.EventID));
                }
                else
                    _hook.GetItem(new(weapon.ID, weapon.ItemCategory, Quantity, weapon.MaxQuantity, (int)Infusion.Standard, UpgradeLevel, -1, weapon.EventID));
            }
            else
                _hook.GetItem(new(option.item.ID, option.item.ItemCategory, Quantity, option.item.MaxQuantity, (int)Infusion.Standard, 0, -1, option.item.EventID));
        }

    }
}
