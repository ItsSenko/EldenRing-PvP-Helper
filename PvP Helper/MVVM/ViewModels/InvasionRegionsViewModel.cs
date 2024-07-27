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
using PropertyHook;
using PvPHelper.MVVM.Models.Regions;
using System.Runtime.InteropServices;
using System;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace PvPHelper.MVVM.ViewModels
{
    public class InvasionRegionsViewModel : ViewModelBase
    {
        #region Data Bindings
        private List<RegionToggle> _regionsItemsSource;

        public List<RegionToggle> RegionsItemsSource
        {
            get { return _regionsItemsSource; }
            set { _regionsItemsSource = value; OnPropertyChanged(); }
        }
        private List<string> _continentItemsSource;

        public List<string> ContinentItemsSource
        {
            get { return _continentItemsSource; }
            set { _continentItemsSource = value; OnPropertyChanged(); }
        }
        private object _selectedContinent;

        public object SelectedContinent
        {
            get { return _selectedContinent; }
            set { _selectedContinent = value; OnPropertyChanged(); OnContinentChanged(); }
        }

        private bool _allOpenWorldChecked;

        public bool AllOpenWorldChecked
        {
            get { return _allOpenWorldChecked; }
            set { _allOpenWorldChecked = value; OnPropertyChanged(); }
        }
        public RelayCommand AllOpenWorldCommand { get; set; }

        private bool _allRegionsChecked;

        public bool AllRegionsChecked
        {
            get { return _allRegionsChecked; }
            set { _allRegionsChecked = value; OnPropertyChanged(); }
        }
        public RelayCommand AllRegionsCommand { get; set; }
        private bool _allDungeonsChecked;

        public bool AllDungeonsChecked
        {
            get { return _allDungeonsChecked; }
            set { _allDungeonsChecked = value; OnPropertyChanged(); }
        }
        public RelayCommand AllDungeonsCommand { get; set; }
        private bool _noFogwallsChecked;

        public bool NoFogwallsChecked
        {
            get { return _noFogwallsChecked; }
            set { _noFogwallsChecked = value; OnPropertyChanged(); }
        }
        public RelayCommand NoFogwallsCommand { get; set; }

        public RelayCommand SaveRegionsCommand { get; set; }
        public RelayCommand SavePresetCommand { get; set; }
        public RelayCommand RefreshPresetsCommand { get; set; }
        public RelayCommand InfoCommand { get; set; }

        private List<SavedRegion> _presetItemsSource;

        public List<SavedRegion> PresetsItemsSource
        {
            get { return _presetItemsSource; }
            set { _presetItemsSource = value; OnPropertyChanged(); }
        }

        private object _selectedPreset;

        public object SelectedPreset
        {
            get { return _selectedPreset; }
            set { _selectedPreset = value; OnPropertyChanged(); OnPresetChanged(); }
        }



        #endregion

        private ErdHook Hook;

        private PHPointer ArrayStartPtr;
        private PHPointer ArrayEndPtr;
        private PHPointer Array;

        private RegionManager RegionManager;

        private Dictionary<string, List<RegionToggle>> BaseRegions = new();
        private Dictionary<string, List<RegionToggle>> _regions = new();

        private bool HasLoaded = false;
        private DispatcherTimer isLoadedTimer;

        private bool IsRefreshing = false;
        public InvasionRegionsViewModel(ErdHook hook)
        {
            Hook = hook;

            ArrayStartPtr = hook.CreateChildPointer(hook.GameDataMan, new int[] { 0x8, 0x938 });
            ArrayEndPtr = hook.CreateChildPointer(hook.GameDataMan, new int[] { 0x8, 0x940 });
            Array = hook.CreateChildPointer(hook.GameDataMan, 0x8);

            RegionManager = new RegionManager();
            RegionManager.Regions = RegionManager.GetRegions();

            foreach (PlayRegion region in RegionManager.Regions)
            {
                RegionToggle toggle = new();
                toggle.PlayRegion = region;
                toggle.Label.Text = region.Name;

                if (!_regions.ContainsKey(region.Map))
                    _regions.Add(region.Map, new());

                _regions[region.Map].Add(toggle);
            }

            BaseRegions = _regions;

            RegionsItemsSource = new();
            ContinentItemsSource = _regions.Keys.ToList();

            SetupCommands();

            isLoadedTimer = new();
            isLoadedTimer.Interval = TimeSpan.FromSeconds(1);
            isLoadedTimer.Tick += IsLoadedTimer_Tick;
            isLoadedTimer.Start();

            PresetsItemsSource = RegionManager.GetSavedRegions();
        }

        private void SetupCommands()
        {
            AllRegionsCommand = new(o => 
            {
                if (!Hook.Hooked || !Hook.Loaded)
                {
                    AllRegionsChecked = false;
                    return;
                }
                AllOpenWorldChecked = false;
                AllDungeonsChecked = false;
                NoFogwallsChecked = false;

                foreach(var regionList in _regions.Values)
                {
                    foreach(var region in regionList)
                    {
                            region.PlayRegion.IsActive = AllRegionsChecked;
                    }
                }

                RegionsItemsSource = new();
            });

            AllOpenWorldCommand = new(o => 
            {
                if (!Hook.Hooked || !Hook.Loaded)
                {
                    AllOpenWorldChecked = false;
                    return;
                }
                AllRegionsChecked = false;
                AllDungeonsChecked = false;
                NoFogwallsChecked = false;

                foreach (var regionList in _regions.Values)
                {
                    foreach (var region in regionList)
                    {
                        if (AllOpenWorldChecked)
                        {
                            region.PlayRegion.IsActive = region.PlayRegion.IsOpenWorld;
                        }
                        else
                        {
                            if (region.PlayRegion.IsOpenWorld)
                                region.PlayRegion.IsActive = false;
                        }
                    }
                }

                RegionsItemsSource = new();
            });

            AllDungeonsCommand = new(o =>
            {
                if (!Hook.Hooked || !Hook.Loaded)
                {
                    AllDungeonsChecked = false;
                    return;
                }
                AllRegionsChecked = false;
                AllOpenWorldChecked = false;
                NoFogwallsChecked = false;

                foreach (var regionList in _regions.Values)
                {
                    foreach (var region in regionList)
                    {
                        if (AllDungeonsChecked)
                        {
                            region.PlayRegion.IsActive = region.PlayRegion.IsDungeon;
                        }
                        else
                        {
                            if (region.PlayRegion.IsDungeon)
                                region.PlayRegion.IsActive = false;
                        }
                    }
                }

                RegionsItemsSource = new();
            });

            NoFogwallsCommand = new(o => 
            {
                if (!Hook.Hooked || !Hook.Loaded)
                {
                    NoFogwallsChecked = false;
                    return;
                }

                foreach (var regionList in _regions.Values)
                {
                    foreach (var region in regionList)
                    {
                        if (region.PlayRegion.IsBoss)
                            region.PlayRegion.IsActive = NoFogwallsChecked;
                    }
                }

                RegionsItemsSource = new();
            });

            SavePresetCommand = new(o => 
            {
                if (!Hook.Hooked || !Hook.Loaded)
                    return;

                CreateBuildDialog dialog = new();
                dialog.OnSave += (name) =>
                {
                    List<BaseRegion> regions = new List<BaseRegion>();
                    foreach (var regionList in _regions.Values)
                    {
                        foreach (var region in regionList)
                        {
                            PlayRegion playRegion = region.PlayRegion;
                            regions.Add(new(playRegion.Map, playRegion.Name, playRegion.PlayRegionID, playRegion.IsActive));
                        }
                    }

                    string directory = Path.Combine(Directory.GetCurrentDirectory(), "Saved Regions");
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    string path = Path.Combine(Directory.GetCurrentDirectory(), $"Saved Regions/{name}.json");
                    RegionManager.SaveRegion(path, new(name, regions));
                };
                dialog.ShowDialog();
            });

            RefreshPresetsCommand = new(o => 
            {
                IsRefreshing = true;
                PresetsItemsSource = RegionManager.GetSavedRegions();
                SelectedPreset = null;

                if (Hook.Loaded)
                {
                    foreach(var regionList in _regions.Values)
                    {
                        foreach(var region in regionList)
                        {
                            region.PlayRegion.IsActive = false;
                        }
                    }
                    ReadRegions();
                }
            });

            InfoCommand = new(o => 
            {
                var ps = new ProcessStartInfo("https://github.com/ItsSenko/EldenRing-PvP-Helper/wiki/Region-Manager")
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            });

            SaveRegionsCommand = new(o => 
            {
                if (!Hook.Loaded || !Hook.Hooked)
                    return;

                SaveRegions();
            });
        }

        
        private void IsLoadedTimer_Tick(object? sender, EventArgs e)
        {
            if (!Hook.Loaded)
            {
                if (HasLoaded)
                {
                    foreach(var regionList in _regions.Values)
                    {
                        foreach(var region in regionList)
                        {
                            try
                            {
                                region.Toggle.IsChecked = false;
                                region.PlayRegion.IsActive = false;
                            }
                            catch { }
                        }
                    }
                    RegionsItemsSource = new();
                    HasLoaded = false;
                }
                return;
            }

            if (!HasLoaded)
            {
                ReadRegions();
                HasLoaded = true;
            }
        }

        private void OnPresetChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (SelectedPreset == null)
                return;

            if (IsRefreshing)
            {
                IsRefreshing = false;
                return;
            }

            SavedRegion savedRegion = SelectedPreset as SavedRegion;

            foreach(var regionList in _regions.Values)
            {
                foreach(var region in regionList)
                {
                    var playRegion = region.PlayRegion;
                    var matchingRegion = savedRegion.Regions.FirstOrDefault(x => x.Map == playRegion.Map && x.Name == playRegion.Name && x.PlayRegionID == playRegion.PlayRegionID);

                    if (matchingRegion != null)
                        playRegion.IsActive = matchingRegion.IsActive;
                }
            }

            RegionsItemsSource = new();
        }
        private void OnContinentChanged()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (SelectedContinent == null)
                return;

            string Map = SelectedContinent as string;

            RegionsItemsSource = _regions[Map];
        }

        private void SaveRegions()
        {
            CustomPointers.CSMenuMan.WriteByte(0x13C, 1); //Disable Save
            List<int> savedRegionIds = new();
            List<string> namesAndIds = new();
            foreach (var regionList in _regions.Values)
            {
                foreach(var region in regionList)
                {
                    if (region.PlayRegion.IsActive)
                    {
                        savedRegionIds.Add(region.PlayRegion.PlayRegionID);
                        namesAndIds.Add($"{region.PlayRegion.Name} {region.PlayRegion.PlayRegionID}");
                    }
                }
            }

            string finalStr = "";
            foreach(string str in namesAndIds)
            {
                finalStr = finalStr + $"\n {str}";
            }

            MessageBox.Show(finalStr);
            Span<byte> bytes = MemoryMarshal.Cast<int, byte>(savedRegionIds.ToArray().AsSpan());

            if (bytes.Length >= 1024)
            {
                MessageBox.Show("Critical Error", "There is an error with the PlayRegion IDs Length", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ArrayStartPtr.WriteBytes(0x0, bytes.ToArray()); //Write Bytes
            Array.WriteInt64(0x940, ArrayStartPtr.Resolve().ToInt64() + bytes.Length);
            CustomPointers.CSMenuMan.WriteByte(0x13C, 0); //Reenable Save
            Hook.GameMan.WriteByte(0xB72, 1); //Trigger Save Request
        }

        private void ReadRegions()
        {
            var arraySize = ArrayEndPtr.Resolve().ToInt64() - ArrayStartPtr.Resolve().ToInt64();

            byte[] bytes = ArrayStartPtr.ReadBytes(0x0, (uint)arraySize);

            Span<int> integerArray = MemoryMarshal.Cast<byte, int>(bytes.AsSpan());

            foreach(int playRegionId in integerArray)
            {
                if (!_regions.ContainsKey("Unknown"))
                    _regions.Add("Unknown", new());

                if (!ContainsRegion(playRegionId))
                {
                    RegionToggle toggle = new();
                    toggle.PlayRegion = new("Unknown", $"?Unknown? {playRegionId}", playRegionId, new int[] { }, isActive: true);
                    toggle.Label.Text = toggle.PlayRegion.Name;
                    _regions["Unknown"].Add(toggle);
                }
                else
                {
                    SetRegionState(playRegionId, true);
                }
            }

            ContinentItemsSource = _regions.Keys.ToList();
            RegionsItemsSource = new();
        }

        private bool ContainsRegion(int id)
        {
            foreach(var regionList in _regions)
            {
                if (regionList.Value.FirstOrDefault(x => x.PlayRegion.PlayRegionID == id) != null)
                    return true;
            }

            return false;
        }

        private RegionToggle GetRegion(int id)
        {
            foreach (var regionList in _regions)
            {
                if (regionList.Value.FirstOrDefault(x => x.PlayRegion.PlayRegionID == id) != null)
                    return regionList.Value.FirstOrDefault(x => x.PlayRegion.PlayRegionID == id);
            }

            return null;
        }

        private void SetRegionState(int id, bool state)
        {
            if (!ContainsRegion(id))
                return;
            var region = GetRegion(id);
            region.PlayRegion.IsActive = state;
        }
    }
}
