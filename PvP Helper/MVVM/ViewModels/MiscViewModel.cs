using Erd_Tools;
using PvPHelper.Core;
using PvPHelper.MVVM.Commands.Misc;
using PvPHelper.MVVM.Models;
using System;
using System.Collections.ObjectModel;

namespace PvPHelper.MVVM.ViewModels
{
    public class MiscViewModel : ViewModelBase
    {
        #region DataBindings
        public ShowHitboxesToggle ShowHitboxesToggle { get; set; }
        public AutoUpdateToggle AutoUpdateToggle { get; set; }
        public CheckForUpdates CheckForUpdates { get; set; }
        public UnsafeToggle UnsafeToggle { get; set; }
        public FreeCamToggle FreeCamToggle { get; set; }
        public CustomFOVToggle CustomFOVToggle { get; set; }
        public CustomFPSToggle CustomFPSToggle { get; set; }
        public NoMaterialCostToggle NoMaterialCostToggle { get; set; }
        public RelayCommand AllArenas { get; set; }
        public RelayCommand AllGestures { get; set; }

        private ObservableCollection<MenuItem> _menuItemsSource;

        public ObservableCollection<MenuItem> MenuItemsSource
        {
            get { return  _menuItemsSource; }
            set {  _menuItemsSource = value; OnPropertyChanged(); }
        }

        private object _selectedMenu;

        public object SelectedMenu
        {
            get { return _selectedMenu; }
            set { _selectedMenu = value; OnPropertyChanged(); OnMenuChanged(); }
        }

        private int _selectedMenuIndex;

        public int SelectedMenuIndex
        {
            get { return _selectedMenuIndex; }
            set { _selectedMenuIndex = value; OnPropertyChanged(); }
        }


        #endregion
        private ErdHook hook;

        private int[] GestureIDs = new int[] { 60800, 60801, 60802, 60803, 60804, 60805, 60806, 60807, 60808, 60809,
        60810,60811,60812,60813,60814,60815,60816,60817,60818,60819,60820,60821,60822,60823,60824,60826,60827,60828,
        60829,60830,60831,60832,60833,60834,60835,60836,60837,60839,60840,60841,60842,60843,60844,60845,60846,60847,60848,60849};
        public MiscViewModel(ErdHook hook, VersionController versionController)
        {
            this.hook = hook;

            ShowHitboxesToggle = new(hook);
            AutoUpdateToggle = new();
            CheckForUpdates = new(versionController);
            UnsafeToggle = new();
            FreeCamToggle = new(hook);
            CustomFOVToggle = new(hook);
            CustomFPSToggle = new(hook);
            NoMaterialCostToggle = new(hook);
            AllArenas = new RelayCommand((o) => 
            {
                if (!hook.Loaded || !hook.Hooked)
                    return;

                hook.SetEventFlag(60350, true);
                hook.SetEventFlag(60360, true);
                hook.SetEventFlag(60370, true);
            });
            AllGestures = new((o) => 
            {
                if (!hook.Loaded || !hook.Hooked)
                    return;
                foreach (int id in GestureIDs)
                {
                    hook.SetEventFlag(id, true);
                }
            });

            MenuItemsSource = new();
            MenuItemsSource.Add(new("Memorize Spells", 0x80f600));
            MenuItemsSource.Add(new("Sort Chest", 0x8104c0));
            MenuItemsSource.Add(new("Level", 0x810160));
            MenuItemsSource.Add(new("Ashes of War", 0x80eee0));
            MenuItemsSource.Add(new("Flask Allocation", 0x80e080));
            MenuItemsSource.Add(new("Wondrous Physick Mix", 0x80de30));
            MenuItemsSource.Add(new("Great Runes", 0x80d940));
            MenuItemsSource.Add(new("Rebirth", 0x810bc0));
            MenuItemsSource.Add(new("Cosmetics (Mirror)", 0x80dcb0));
            MenuItemsSource.Add(new("Spirit Tuning", 0x80daf0));
            MenuItemsSource.Add(new("Blacksmith", 0x80ebb0));
            MenuItemsSource.Add(new("Smithing Table", 0x80f4f0));
            MenuItemsSource.Add(new("Sell Item", 0x80edd0));
            MenuItemsSource.Add(new("Shop (All)", 0x80e770));
            MenuItemsSource.Add(new("Shop (Rememberance)", 0x810920));
            MenuItemsSource.Add(new("Shop (Dragon Communion)", 0x810a70, 101950, 101999));
            //MenuItemsSource.Add(new("Shop (Champions Equipment)", 0x80e400));
        }

        private void OnMenuChanged()
        {
            if (!hook.Hooked || !hook.Loaded)
                return;

            if (SelectedMenu == null)
                return;

            MenuItem menu = SelectedMenu as MenuItem;

            OpenMenu(menu.Name, GetAddress(menu.Offset), menu.startId, menu.endId); 
        }
        private void OpenMenu(string name, IntPtr address, int startId, int endId)
        {
            bool isShop = name.ToLower().StartsWith("shop");
            string asmStr = Helpers.GetEmbededResource(isShop ? "Resources.Assembly.OpenShopMenu.asm" : "Resources.Assembly.OpenMenu.asm");
            string asm = isShop ? string.Format(asmStr, startId.ToString(), endId.ToString(), address.ToString("X")) : string.Format(asmStr, address.ToString("X"));
            hook.AsmExecute(asm);
        }
        private IntPtr GetAddress(int offset)
        {
            return hook.Process.MainModule.BaseAddress + offset;
        }
    }
}
