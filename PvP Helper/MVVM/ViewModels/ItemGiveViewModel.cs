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
using PvPHelper.MVVM.Models.Database;
using System;
using PvPHelper.MVVM.Models.Database.ItemsBases;
using System.Windows.Data;
using PvPHelper.Core.Extensions;
using Erd_Tools.Models.System.Dlc;
using CommandManager = PvPHelper.Console.CommandManager;

namespace PvPHelper.MVVM.ViewModels
{
    public class ItemGiveViewModel : ViewModelBase
    {
        private ObservableCollection<ItemGibModel> _inventoryItems;

        public ObservableCollection<ItemGibModel> InventoryItems
        {
            get { return _inventoryItems; }
            set { _inventoryItems = value; OnPropertyChanged(); }
        }

       


        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set 
            { 
                _searchText = value; 
                OnPropertyChanged();

                if (dataBase != null && dataBase.searchAlg.Items.Count() > 0)
                    dataBase.searchAlg.SearchString = SearchText;
            }
        }

        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }

        private string _pageText;

        public string PageText
        {
            get { return _pageText; }
            set { _pageText = value; OnPropertyChanged(); }
        }

        private int _addAmount;

        public int AddAmount
        {
            get { return _addAmount; }
            set { _addAmount = value; OnPropertyChanged(); }
        }

        private string _amountText;

        public string AmountText
        {
            get { return _amountText; }
            set { _amountText = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ISortOrder<Item>> _sortOrderItemsSource;

        public ObservableCollection<ISortOrder<Item>> SortOrderItemsSource
        {
            get { return _sortOrderItemsSource; }
            set { _sortOrderItemsSource = value; OnPropertyChanged(); }
        }

        private object _selectedSortOrder;

        public object SelectedSortOrder
        {
            get { return _selectedSortOrder; }
            set 
            { 
                if (dataBase == null || !hook.Hooked || !hook.Setup)
                {
                    _selectedSortOrder = null;
                    OnPropertyChanged();
                    return;
                }
                _selectedSortOrder = value; 
                OnPropertyChanged(); 

                OnSortOrderChanged(SelectedSortOrder == null ? null : (ISortOrder<Item>)SelectedSortOrder);
            }
        }

        private ObservableCollection<ItemTypeItems> _ItemTypeItemsSource;

        public ObservableCollection<ItemTypeItems> ItemTypeItemsSource
        {
            get { return _ItemTypeItemsSource; }
            set { _ItemTypeItemsSource = value; OnPropertyChanged(); }
        }

        private object _selectedItemType;

        public object SelectedItemType
        {
            get { return _selectedItemType; }
            set { _selectedItemType = value; OnPropertyChanged(); OnSelectedItemTypeChanged(); }
        }

        private ObservableCollection<WeaponClassItems> _weaponClassItems;

        public ObservableCollection<WeaponClassItems> WeaponClassItemsSource
        {
            get { return _weaponClassItems; }
            set { _weaponClassItems = value; OnPropertyChanged(); }
        }

        private object _selectedWeaponClass;

        public object SelectedWeaponClass
        {
            get { return _selectedWeaponClass; }
            set { _selectedWeaponClass = value; OnPropertyChanged(); OnWeaponClassChanged(); }
        }

        private ObservableCollection<NamedObject<List<Item>>> _contentItemsSource;

        public ObservableCollection<NamedObject<List<Item>>> ContentItemsSource
        {
            get { return _contentItemsSource; }
            set { _contentItemsSource = value; OnPropertyChanged(); }
        }

        private object _selectedContent;

        public object SelectedContent
        {
            get { return _selectedContent; }
            set { _selectedContent = value; OnPropertyChanged(); OnSelectedContentChanged(); }
        }

        private Visibility _mainVisibility;

        public Visibility MainVisibility
        {
            get { return _mainVisibility; }
            set { _mainVisibility = value; OnPropertyChanged(); }
        }

        private Visibility _editItemVisbility;

        public Visibility EditItemVisibility
        {
            get { return _editItemVisbility; }
            set { _editItemVisbility = value; OnPropertyChanged(); }
        }


        private readonly object ashCollectionLock = new object();
        private ObservableCollection<ItemGibModel> _ashItems;

        public ObservableCollection<ItemGibModel> AshItems
        {
            get { return _ashItems; }
            set 
            { 
                _ashItems = value;
                //BindingOperations.EnableCollectionSynchronization(_ashItems, ashCollectionLock);
                OnPropertyChanged(); 
            }
        }

        private string _ashSearchText;

        public string AshSearchText
        {
            get { return _ashSearchText; }
            set 
            { 
                _ashSearchText = value; 
                OnPropertyChanged();
                if (GemAlg != null)
                    GemAlg.SearchString = value;
            }
        }

        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; OnPropertyChanged(); }
        }

        private ImageSource _iconSource;

        public ImageSource IconSource
        {
            get { return _iconSource; }
            set { _iconSource = value; OnPropertyChanged(); }
        }

        private ImageSource _ashIconSource;

        public ImageSource  AshIconSource
        {
            get { return _ashIconSource; }
            set { _ashIconSource = value; OnPropertyChanged(); }
        }

        private ImageSource _infusionIconSource;

        public ImageSource InfusionIconSource
        {
            get { return _infusionIconSource; }
            set { _infusionIconSource = value; OnPropertyChanged(); }
        }

        private int _editAmount;

        public int EditAmount
        {
            get { return _editAmount; }
            set { _editAmount = value; OnPropertyChanged(); }
        }

        private int _editAmountMax;

        public int EditAmountMax
        {
            get { return _editAmountMax; }
            set { _editAmountMax = value; OnPropertyChanged(); }
        }

        private string _editAmountText;

        public string EditAmountText
        {
            get { return _editAmountText; }
            set { _editAmountText = value; OnPropertyChanged(); }
        }


        private int _currUpgradeValue;

        public int CurrUpgradeValue
        {
            get { return _currUpgradeValue; }
            set { _currUpgradeValue = value; OnPropertyChanged(); }
        }

        private int _maxUpgrade;

        public int MaxUpgrade
        {
            get { return _maxUpgrade; }
            set { _maxUpgrade = value; OnPropertyChanged(); }
        }


        private string _currUpgradeText;

        public string CurrUpgradeText
        {
            get { return _currUpgradeText; }
            set { _currUpgradeText = value; OnPropertyChanged(); }
        }


        private ObservableCollection<ItemGibModel> _infusionItems;

        public ObservableCollection<ItemGibModel> InfusionItems
        {
            get { return _infusionItems; }
            set { _infusionItems = value; OnPropertyChanged(); }
        }

        public ICommand AddItem { get; set; }
        public ICommand Cancel { get; set; }

        private SearchableDataBase<Item> dataBase;
        private SearchAlgorithm<Gem> GemAlg;
        public ICommand RefreshBuilds { get; set; }
        private ErdHook hook;

        public ItemGiveViewModel(ErdHook hook)
        {
            this.hook = hook;
            InventoryItems = new();
            AshItems = new();
            InfusionItems = new();
            SortOrderItemsSource = new();
            ItemTypeItemsSource = new();
            WeaponClassItemsSource = new();
            ContentItemsSource = new();

            GemAlg = new(new List<Gem>(), new ClosestMatchSort<Gem>());
            GemAlg.OnItemsChanged += OnAshShownItemsChanged;

            SortOrderItemsSource.Add(new AlphabeticalSort<Item>());
            SortOrderItemsSource.Add(new ClosestMatchSort<Item>());
            bk.RunWorkerCompleted += Bk_RunWorkerCompleted;
            bk.DoWork += Bk_DoWork;

            MainVisibility = Visibility.Visible;
            EditItemVisibility = Visibility.Hidden;

            RefreshBuilds = new RelayCommand(o =>
            {
                Load();
            });
            ;

            NextPageCommand = new RelayCommand((o) => 
            {
                if(dataBase != null)
                    dataBase.NextPage(); 
            });
            PrevPageCommand = new RelayCommand((o) => 
            {
                if (dataBase != null)
                    dataBase.PrevPage();
            });

            PageText = "0/0";

            AddAmount = 1;
            AmountText = "1";

            AddItem = new RelayCommand((o) => TryAddItem());
            Cancel = new RelayCommand((o) => CancelItem());

            hook.OnSetup += Hook_OnSetup;
            ashWorker.DoWork += AshWorkerDoWork;
            ashWorker.RunWorkerCompleted += AshWorker_RunWorkerCompleted;

            
        }
        

        public void OnSortOrderChanged(ISortOrder<Item> sortOrder)
        {
            if (sortOrder == null || dataBase == null)
                return;

            dataBase.searchAlg.Order = sortOrder;
        }

        private void Hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Load();
            });
        }

        IEnumerable<Item> lastItems;
        BackgroundWorker bk = new();

        List<IEnumerable<Item>> queue = new();
        private void OnShownItemsChanged(IEnumerable<Item> items)
        {
            if (items == lastItems)
            {
                lastItems = items;
                return;
            }
            lastItems = items;

            queue.Add(items);

            foreach (var invitem in InventoryItems)
                invitem.SetupFromItem(null);

            if (bk.IsBusy)
                return;

            bk.RunWorkerAsync();
        }

        private void Bk_DoWork(object? sender, DoWorkEventArgs e)
        {
            foreach (var invItem in InventoryItems)
            {
                Item item = queue[0].ToList().ElementAtOrDefault(InventoryItems.IndexOf(invItem));
                invItem.SetupFromItem(item);
            }
            queue.RemoveAt(0);
        }

        private void Bk_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (queue.Count > 0)
                bk.RunWorkerAsync();
        }
        private void OnClick(Item item, ItemGibModel model)
        {
            if (!hook.Hooked || !hook.Setup || item == null)
                return;

            int amount = AddAmount;

            if (item is Weapon && amount > 9)
                amount = 10;


            hook.GetItem(new(item.ID, item.ItemCategory, amount, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID));
        }
        private bool lastWasSomber = false;
        private void OnRightClick(Item item, ItemGibModel model)
        {
            if (!hook.Hooked || !hook.Setup || item == null)
                return;

            MainVisibility = Visibility.Hidden;
            EditItemVisibility = Visibility.Visible;

            if (item is Weapon wpn)
            {
                MaxUpgrade = wpn.MaxUpgrade;
                if (lastWasSomber && !wpn.IsSomber())
                {
                    CurrUpgradeValue = Helpers.GetSmithLevel(CurrUpgradeValue);
                    CurrUpgradeText = CurrUpgradeValue.ToString();
                }
                else if (!lastWasSomber && wpn.IsSomber())
                {
                    CurrUpgradeValue = Helpers.GetSomberLevel(CurrUpgradeValue);
                    CurrUpgradeText = CurrUpgradeValue.ToString();
                }

                lastWasSomber = wpn.IsSomber();

                EditAmountMax = 100;
                EditAmount = 1;
                EditAmountText = "1";

                ItemName = item.Name;

                if (wpn.GemAttachType == GemMountType.Any)
                {
                    List<Gem> gems = new();
                    foreach (Gem gem in Gem.All)
                    {
                        if (gem.WeaponTypes.Contains(wpn.Type))
                        {
                            gems.Add(gem);
                        }
                    }

                    GemAlg.Items = gems;
                }

                FinalInfo = new(item.ID, item.ItemCategory, 0, item.MaxQuantity, 0, 0, -1, item.EventID);

                IconSource = Helpers.GetImageSource(Helpers.GetFullIconID(item.IconID));
            }
            else
            {
                MaxUpgrade = 0;
                CurrUpgradeValue = 0;
                CurrUpgradeText = "0";

                EditAmountMax = 999;
                EditAmount = 1;
                EditAmountText = "1";

                ItemName = item.Name;

                FinalInfo = new(item.ID, item.ItemCategory, 0, item.MaxQuantity, 0, 0, -1, item.EventID);

                IconSource = Helpers.GetImageSource(Helpers.GetFullIconID(item.IconID));
            }

            if (Settings.Default.DebugLogs)
            {
                CommandManager.Log($"Name: {item.Name}");
                CommandManager.Log($"ID: {item.ID}");
                CommandManager.Log($"Category: {item.ItemCategory}");
                CommandManager.Log($"IconID: {item.IconID}");
            }
        }
        BackgroundWorker ashWorker = new();
        List<List<Gem>> ashQueue = new();
        private void AshWorkerDoWork(object? sender, DoWorkEventArgs e)
        {
            foreach(ItemGibModel gemModel in AshItems)
            {
                Gem gem = ashQueue[0].ElementAtOrDefault(AshItems.IndexOf(gemModel));
                gemModel.SetupFromItem(gem, false);
            }
            ashQueue.RemoveAt(0);
        }

        private void AshWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (ashQueue.Count > 0)
                ashWorker.RunWorkerAsync();
        }

        IEnumerable<Gem> lastGemItems;
        private void OnAshShownItemsChanged(IEnumerable<Gem> items)
        {
            if (items == lastGemItems)
            {
                lastGemItems = items;
                return;
            }

            lastGemItems = items;

            ashQueue.Add(items.ToList());

            foreach (ItemGibModel model in AshItems)
                model.SetupFromItem(null);

            if (ashWorker.IsBusy)
                return;

            ashWorker.RunWorkerAsync();
        }

        private ItemSpawnInfo _finalInfo;

        public ItemSpawnInfo FinalInfo
        {
            get { return _finalInfo; }
            set { _finalInfo = value; }
        }

        private void OnAshClicked(Item item, ItemGibModel model)
        {
            if (item is Gem gem)
            {
                FinalInfo = new(FinalInfo.ID, FinalInfo.Category, FinalInfo.Quantity, FinalInfo.MaxQuantity, (int)Infusion.Standard, FinalInfo.Upgrade, gem.ID, FinalInfo.EventID);

                AshIconSource = Helpers.GetImageSource(Helpers.GetFullIconID(gem.IconID));

                InfusionItems.Clear();
                InfusionIconSource = null;

                foreach(Infusion inf in gem.Infusions)
                {
                    ItemGibModel newModel = new(inf, Helpers.GetImageSource(inf.ToString()), inf.ToString());
                    newModel.OnLeftClick += OnInfusionClicked;
                    InfusionItems.Add(newModel);
                }

                
            }
        }

        public void OnInfusionClicked(Item iten, ItemGibModel model)
        {
            FinalInfo = new(FinalInfo.ID, FinalInfo.Category, FinalInfo.Quantity, FinalInfo.MaxQuantity, (int)model.Infusion, FinalInfo.Upgrade, FinalInfo.Gem, FinalInfo.EventID);
            InfusionIconSource = Helpers.GetImageSource(model.Infusion.ToString());
        }
        public void TryAddItem()
        {
            if (!hook.Hooked || !hook.Setup || FinalInfo == null)
                return;

            if (EditAmount > FinalInfo.MaxQuantity)
            {
                List<ItemSpawnInfo> Items = new();

                int stacks = EditAmount / FinalInfo.MaxQuantity;
                int extra = EditAmount % FinalInfo.MaxQuantity;

                for (int i = 0; i < stacks; i++)
                {
                    Items.Add(new(FinalInfo.ID, FinalInfo.Category, FinalInfo.MaxQuantity, FinalInfo.MaxQuantity, (int)FinalInfo.Infusion, CurrUpgradeValue, FinalInfo.Gem, FinalInfo.EventID));
                }
                if (extra > 0)
                {
                    Items.Add(new(FinalInfo.ID, FinalInfo.Category, extra, FinalInfo.MaxQuantity, (int)FinalInfo.Infusion, CurrUpgradeValue, FinalInfo.Gem, FinalInfo.EventID));
                }

                hook.GetItem(Items, CancellationToken.None);
                return;
            }
            FinalInfo = new(FinalInfo.ID, FinalInfo.Category, EditAmount, FinalInfo.MaxQuantity, (int)FinalInfo.Infusion, CurrUpgradeValue, FinalInfo.Gem, FinalInfo.EventID);

            hook.GetItem(FinalInfo);
        }

        public void CancelItem()
        {
            FinalInfo = null;

            EditAmountMax = 100;
            EditAmount = 0;
            EditAmountText = "0";

            MainVisibility = Visibility.Visible;
            EditItemVisibility = Visibility.Hidden;

            InfusionItems.Clear();
            foreach (ItemGibModel model in AshItems)
                model.SetupFromItem(null);

            AshIconSource = null;
            InfusionIconSource = null;

            ItemName = "";
        }

        private void Load()
        {
            ContentItemsSource.Clear();
            if (!Helpers.SeamlessItemsInitialized && Helpers.GetIfModuleExists(hook.Process, "ersc.dll"))
                Helpers.InitializeSeamlessItems();
            List<Item> allContent = new();
            List<Item> vanilla = new();
            List<Item> dlc = new();
            List<Item> vanillaDLC = new();
            List<Item> seamlessCoop = new();

            foreach (var cat in ItemCategory.All)
            {
                if (!hook.CSDlc.DlcAvailable(DlcName.ShadowOfTheErdtree) && cat.Name.StartsWith("DLC"))
                    continue;

                allContent.AddRange(cat.Items);

                if (cat.Name.StartsWith("DLC") && hook.CSDlc.DlcAvailable(DlcName.ShadowOfTheErdtree))
                {
                    dlc.AddRange(cat.Items);
                    vanillaDLC.AddRange(cat.Items);
                }
                else if (cat.Name != "Seamless Coop")
                {
                    vanilla.AddRange(cat.Items);

                    if (hook.CSDlc.DlcAvailable(DlcName.ShadowOfTheErdtree))
                        vanillaDLC.AddRange(cat.Items);
                }
                else if (Helpers.GetIfModuleExists(hook.Process, "ersc.dll"))
                {
                    seamlessCoop.AddRange(cat.Items);
                }
            }
            dataBase = new("itemGibDataBase", allContent, 32);
            dataBase.OnShownItemsChanged += OnShownItemsChanged;

            ContentItemsSource.Add(new(vanilla, "Vanilla"));

            if (hook.CSDlc.DlcAvailable(DlcName.ShadowOfTheErdtree))
                ContentItemsSource.Add(new(dlc, "Shadow Of the Erdtree"));

            if (hook.CSDlc.DlcAvailable(DlcName.ShadowOfTheErdtree))
                ContentItemsSource.Add(new(vanillaDLC, "Vanilla + SoTE"));

            if (Helpers.GetIfModuleExists(hook.Process, "ersc.dll"))
                ContentItemsSource.Add(new(seamlessCoop, "Seamless co-op"));
            ContentItemsSource.Add(new(allContent, "All"));

            SelectedContent = ContentItemsSource[0];

            ObservableCollection<ItemGibModel> List = new();
            for (int i = 0; i < dataBase.MaxPerPage; i++)
            {
                ItemGibModel model = new(null, null, null, string.Empty, string.Empty);
                model.OnLeftClick += OnClick;
                model.OnRightClick += OnRightClick;
                model.SetupFromItem(null);
                List.Add(model);

            }
            PageText = $"0/{(dataBase.Pages == 0 ? 0 : dataBase.Pages - 1)}";
            dataBase.OnCurrentPageChanged += (page) =>
            {
                PageText = $"{page}/{(dataBase.Pages == 0 ? 0 : dataBase.Pages - 1)}";
            };
            dataBase.OnPagesChanged += (pages) =>
            {
                PageText = $"{dataBase.CurrentPage}/{(dataBase.Pages == 0 ? 0 : dataBase.Pages - 1)}";
            };

            InventoryItems = List;
            

            var categories = Enum.GetValues(typeof(Item.Category));
            ItemTypeItemsSource.Clear();
            ItemTypeItemsSource.Add(new("All", allContent, Item.Category.Accessory));
            for (int i = 0; i < categories.Length; i++)
            {
                ItemTypeItemsSource.Add(new(categories.GetValue(i).ToString(), (List<Item>)dataBase.Items, (Item.Category)categories.GetValue(i)));
            }

            var wpnTypes = Enum.GetValues(typeof(Weapon.WeaponType));
            WeaponClassItemsSource.Clear();
            WeaponClassItemsSource.Add(new("All", allContent, WeaponType.Katana));

            for (int i = 0; i < wpnTypes.Length; i++)
            {
                WeaponClassItemsSource.Add(new(wpnTypes.GetValue(i).ToString(), allContent, (Weapon.WeaponType)wpnTypes.GetValue(i)));
            }

            SelectedWeaponClass = null;
            SelectedItemType = null;
            SelectedContent = null;

            for (int i = 0; i < 67; i++)
            {
                AshItems.Add(new(null, null, null, ""));
                AshItems[i].OnLeftClick += OnAshClicked;
                AshItems[i].SetupFromItem(null);
            }
            OnShownItemsChanged(vanilla);
        }

        private void OnSelectedItemTypeChanged()
        {
            if (SelectedItemType == null)
                return;

            if (!hook.Hooked || !hook.Setup)
            {
                SelectedItemType = null;
                return;
            }

            ItemTypeItems items = SelectedItemType as ItemTypeItems;
            items.Update(null, dataBase);

            if (items.Name == "Weapons" && SelectedWeaponClass != null)
                OnWeaponClassChanged();
        }

        private void OnWeaponClassChanged()
        {
            if (SelectedWeaponClass == null)
                return;

            if (!hook.Hooked || !hook.Setup || SelectedItemType == null)
                return;

            ItemTypeItems selectedItemTypes = SelectedItemType as ItemTypeItems;
            if (selectedItemTypes.Name != "All" && selectedItemTypes.Name != "Weapons")
            {
                return;
            }

            WeaponClassItems items = SelectedWeaponClass as WeaponClassItems;
            items.Update(null, dataBase);
        }

        private void OnSelectedContentChanged()
        {
            if (!hook.Hooked || !hook.Setup || SelectedContent == null)
                return;

            if (dataBase == null)
                return;

            var content = SelectedContent as NamedObject<List<Item>>;

            dataBase.Items = content.Value;
            dataBase.searchAlg.Items = content.Value;

            if (SelectedItemType != null)
                OnSelectedItemTypeChanged();
        }
    }
}
