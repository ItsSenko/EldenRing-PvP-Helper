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
using PvPHelper.MVVM.Models.Search;
using PvPHelper.MVVM.Models.Search.SortOrders;

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
        #region DLC MassGibg
        public ICommand AllDLCUpgrades { get; set; }
        public ICommand AllDLCMapsGraces { get; set; }
        public ICommand AllDLCCookbooks { get; set; }
        public ICommand AllDLCTools { get; set; }
        public ICommand AllDLCMerchantItems { get; set; }
        public ICommand AllDLCCraftingMaterials { get; set; }
        public ICommand AllDLCCrystalTears { get; set; }
        public ICommand AllDLCAshes { get; set; }
        public ICommand AllDLCConsumables { get; set; }
        public ICommand AllDLCSpells { get; set; }
        public ICommand AllDLCAmmo { get; set; }
        public ICommand AllDLCLimitedItems { get; set; }
        public ICommand AllDLCMeleeWeapons { get; set; }
        public ICommand AllDLCShield { get; set; }
        public ICommand AllDLCSpellTools { get; set; }
        public ICommand AllDLCRangedWeapons { get; set; }
        public ICommand AllDLCArmor { get; set; }
        public ICommand AllDLCTalismans { get; set; }
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
        #region Other
        private Visibility _dlcVisibility;

        public Visibility DLCVisibility
        {
            get { return _dlcVisibility; }
            set { _dlcVisibility = value; OnPropertyChanged(); }
        }

        #endregion
        #endregion

        private ErdHook _hook;
        private Player _player;

        private List<int> mapEventFlagIDs = new();
        public static int MassUpgradeLevel = 25;
        private DispatcherTimer runesTimer;

        private List<ItemCategoryOption> allItemCategorys = new();

        private int[] dlcMapIds = { 62080, 62081, 62082, 62083, 62084, 82002 };

        private int[] baseCookbookFlags = { 60120,67610,67600,67650,67640,67630,67130,68230,67000,67110,67010,67800,67830,67020,67050,67880,67430,67030,67220,67060,67080,67870,
                                            67900,67290,67100,67270,67070,67230,67120,67890,67090,67910,67200,67210,67280,67260,67310,67300,67250,68000,68010,68030,68020,68200,68220,68210,67840,67850,67860,67920,67410,67450,
                                            67480,67400,67420,67460,67470,67440,68400,68410};

        private int[] dlcCookbookFlags = { 68590,68730,68690,68600,68610,68720,68630,68680,68640,68650,68660,68620,68700,68710,68750,68670,68880,68740,68780,68760,68950,68840,68520,68530,
                                           68540,68550,68560,68510,68830,68810,68570,68920,68580,68770,68900,68800,68820,68890,68930,68940,68850,68910,68860,68870,68790};
        public ItemsViewModel(ErdHook hook)
        {
            _hook = hook;
            _player = new(hook.PlayerIns, hook);

            _hook.OnSetup += _hook_OnSetup;
            _hook.OnUnhooked += _hook_OnUnhooked;
            SetupCommands();

            DLCVisibility = Visibility.Hidden;

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

                if (_hook.CSDlc.DlcAvailable(Erd_Tools.Models.System.Dlc.DlcName.ShadowOfTheErdtree))
                    DLCVisibility = Visibility.Visible;
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

                foreach (Continent continent in _hook.GetContinents())
                {
                    if (continent.Dlc == Erd_Tools.Models.System.Dlc.DlcName.None)
                    {
                        foreach (Hub hub in continent.Hubs)
                        {
                            foreach (Grace grace in hub.Graces)
                            {
                                _hook.SetEventFlag(grace.EventFlagID, true);
                            }
                        }
                    }
                }
            });
            AllLimitedItems = new RelayCommand((o) => { LimitedItems(); });
            AllWhetblades = new RelayCommand((o) => { Whetblades(); });

            AllCookbooks = new RelayCommand((o) =>
            {
                var massGib = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Cookbooks"), false);
                massGib.Execute(null);

                foreach(int flag in baseCookbookFlags)
                    _hook.SetEventFlag(flag, true);
            }); 
            AllTools = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Tools"), false);
            AllMerchantItems = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Merchant Items"), false);
            AllCraftingMaterials = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Crafting Materials"));
            AllCrystalTears = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Crystal Tears"), false);
            AllAshes = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Gems"), false);
            AllConsumables = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Consumables"));
            AllSpells = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Magic"), false);
            AllAmmo = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Ammo"));
            AllUpgradeMaterials = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Upgrade Materials"));

            AllMeleeWeapons = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Melee Weapons"), false, MassUpgradeLevel, true);
            AllRangedWeapons = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Ranged Weapons"), false, MassUpgradeLevel, true);
            AllShields = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Shields"), false, MassUpgradeLevel, true);
            AllArmor = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Armor"), false);
            AllSpellTools = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Spell Tools"), false, MassUpgradeLevel, true);
            AllTalismans = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans"));

            AllDLCUpgrades = new RelayCommand((o) =>
            {
                ItemCategory cat = ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Upgrade Materials");

                var scadu = cat.Items[0];
                var ash = cat.Items[1];
                _hook.GetItem(new(scadu.ID, scadu.ItemCategory, 50, scadu.MaxQuantity, (int)Infusion.Standard, 0));
                _hook.GetItem(new(ash.ID, ash.ItemCategory, 25, ash.MaxQuantity, (int)Infusion.Standard, 0));
            });
            AllDLCMapsGraces = new RelayCommand((o) =>
            {
                if (!_hook.Hooked || !_hook.Loaded)
                    return;

                foreach (int flag in dlcMapIds)
                {
                    _hook.SetEventFlag(flag, true);
                }

                foreach (Continent continent in _hook.GetContinents())
                {
                    if (continent.Dlc != Erd_Tools.Models.System.Dlc.DlcName.None)
                    {
                        foreach (Hub hub in continent.Hubs)
                        {
                            foreach (Grace grace in hub.Graces)
                            {
                                _hook.SetEventFlag(grace.EventFlagID, true);
                            }
                        }
                    }
                }
            });
            AllDLCCookbooks = new RelayCommand((o) => 
            {
                var massGib = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Cookbooks"), false);
                massGib.Execute(null);

                foreach (int flag in dlcCookbookFlags)
                    _hook.SetEventFlag(flag, true);
            }); 
            AllDLCTools = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Tools"), false);
            AllDLCMerchantItems = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Merchant Items"), false);
            AllDLCCraftingMaterials = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Crafting Materials"));
            AllDLCCrystalTears = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Crystal Tears"), false);
            AllDLCAshes = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Gems"), false);
            AllDLCConsumables = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Consumables"));
            AllDLCSpells = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Magic"), false);
            AllDLCAmmo = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Ammo"));
            //AllDLCLimitedItems = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Key Items"), 1);
            AllDLCMeleeWeapons = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Melee Weapons"), false, MassUpgradeLevel, true);
            AllDLCShield = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Shields"), false, MassUpgradeLevel, true);
            AllDLCSpellTools = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Spell Tools"), false, MassUpgradeLevel, true);
            AllDLCRangedWeapons = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Ranged Weapons"), false, MassUpgradeLevel, true);
            AllDLCArmor = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Armor"), false);
            AllDLCTalismans = new MassGib(_hook, ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Talismans"), false);

            AddRunes = new RelayCommand((o) =>
            {
                if (!_hook.Loaded || !_hook.Hooked)
                    return;

                _player.AddRunes(CurrRunesToAdd);
            });
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

    }
}
