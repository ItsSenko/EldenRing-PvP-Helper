﻿using Erd_Tools;
using Erd_Tools.Models.Entities;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.Console.Commands;
using PvPHelper.Core;
using PvPHelper.Core.Achievements;
using PvPHelper.Core.Extensions;
using PvPHelper.Core.Hotkeys;
using PvPHelper.MVVM.Dialogs;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PvPHelper.MVVM.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region RelayCommands
        public RelayCommand DashboardCommand { get; set; }
        public RelayCommand ItemsCommand { get; set; }
        public RelayCommand ItemGiveCommand { get; set; }
        public RelayCommand PrefabCreatorCommand { get; set; }
        public RelayCommand YourPrefabsCommand { get; set; }
        public RelayCommand MiscCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand LobbyManagerCommand { get; set; }
        public RelayCommand RegionManagerCommand { get; set; }
        public RelayCommand Discord { get; set; }
        public RelayCommand Kofi { get; set; }
        public RelayCommand CreditsCommand { get; set; }
        public RelayCommand AuthorLinkCommand { get; set; }

        private string _versionText;
        public string VersionText
        {
            get { return _versionText; }
            set { _versionText = value; OnPropertyChanged(); }
        }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        #endregion
        #region Bindings
        private Visibility _unsafeVisibility;

        public Visibility UnsafeVisibility
        {
            get { return _unsafeVisibility; }
            set { _unsafeVisibility = value; OnPropertyChanged(); }
        }

        #endregion
        public Dictionary<string, ViewModelBase> _viewModels = new();

        
        private static ErdHook _hook;
        private VersionController _vController;
        public static Player player;
        public bool HookCompletelyLoaded 
        {
            get { return _hook is not null && _hook.Hooked && _hook.Loaded; }
        }
        
        public static ErdHook GetMainHook() => _hook;
        
        public CommandManager commandManager;
        public PHPointer LocalPlayer;
        public MainWindowViewModel()
        {
            _hook = new(5000, 1000, 
                p => p.MainWindowTitle is "ELDEN RING™" 
                     || (p.MainWindowTitle is "ELDEN RING" && p.ProcessName == "eldenring"));

            UnsafeVisibility = Settings.Default.AllowUnsafe ? Visibility.Visible : Visibility.Hidden;
            Settings.Default.SettingsSaving += (s, e) =>
            {
                UnsafeVisibility = Settings.Default.AllowUnsafe ? Visibility.Visible : Visibility.Hidden;
            };

            ExtensionsCore.Initialize();
            commandManager = new();
            _vController = new(Directory.GetCurrentDirectory());
            VersionText = _vController.CurrentLocalVersion;

            if (Settings.Default.AutoUpdate && _vController.UpdateAvailable)
            {
                _vController.Update(_vController._releaseUrl, _vController.CurrentVersion);
            }

            Views.UserControls.Console.LogLoaded += Console_LogLoaded;

            _hook.OnSetup += _hook_OnSetup;
            _hook.OnUnhooked += _hook_OnUnhooked;
            player = new Player(_hook.PlayerIns, _hook);

            CloseCommand = new((o) => { Application.Current.Shutdown(); });
            Discord = new((o) =>
            {
                var ps = new ProcessStartInfo("https://www.discord.gg/VmyGAS24Gf")
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            });
            Kofi = new((o) =>
            {
                var ps = new ProcessStartInfo("https://ko-fi.com/senko")
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            });
            AuthorLinkCommand = new((o) =>
            {
                var ps = new ProcessStartInfo("https://linktr.ee/senkopur")
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            });
            LocalPlayer = _hook.CreateChildPointer(_hook.WorldChrMan, new int[] { 0x1E508 });

            CustomPointers.Initialize(_hook);
            SetupViewModels();
            RegisterCommands();
            Blacklist.Initialize();
            //Helpers.LoadImages();
            Achievements.Initialize(_hook);

            DashboardView.OnLoaded += () =>
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Icons")) || _vController.IsUpdateAvailableForIcons())
                {
                    InformationDialog dialog = new("You are missing the Elden Ring Icons. Would you like to download them now?");
                    dialog.OnOk += () =>
                    {
                        _vController.Update(_vController._iconsReleaseUrl, _vController.CurrentIconsVersion);
                    };
                    dialog.ShowDialog();
                }
            };
        }

        private bool HasLoaded = false;
        private void Console_LogLoaded()
        {
            if (HasLoaded)
                return;

            CommandManager.Log("Welcome to the PvP Helper for Elden Ring!");
            CommandManager.Log("Created by Senko and Catshark");
            if (_vController.UpdateAvailable)
            {
                CommandManager.Log("There is an Update Available!");
                CommandManager.Log($"Your version is {_vController.CurrentLocalVersion}");
                CommandManager.Log($"New version is {_vController.CurrentVersion}");
                CommandManager.Log($"Do /update if you would like to update!");
            }
            else
            {
                CommandManager.Log("Looks like you're up to date!");
            }

            HasLoaded = true;
        }

        private void _hook_OnUnhooked(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => { CommandManager.Log("No longer hooked to Elden Ring."); });
        }

        private void _hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => 
            { 
                CommandManager.Log("Setup Hook for Elden Ring");

                //CustomPointers.GetEquipParamGoodsFunc = _hook.CreateBasePointer(_hook.Process.MainModule.BaseAddress + 0xD39CD0);
                if (Helpers.GetIfModuleExists(_hook.Process, "ersc.dll"))
                    Helpers.InitializeSeamlessItems();
            });
        }

        private void SetupViewModels()
        {
            _viewModels.Add(nameof(DashboardView), new DashboardViewModel(_hook, LocalPlayer));
            _viewModels.Add(nameof(ItemsView), new ItemsViewModel(_hook));
            _viewModels.Add(nameof(ItemGiveView), new ItemGiveViewModel(_hook));
            _viewModels.Add(nameof(PrefabCreatorView), new PrefabCreatorViewModel(_hook));
            //_viewModels.Add(nameof(LobbyManagerView), new LobbyManagerViewModel(_hook));
            _viewModels.Add(nameof(MiscView), new MiscViewModel(_hook, _vController));
            _viewModels.Add(nameof(InvasionRegionsView), new InvasionRegionsViewModel(_hook));
            _viewModels.Add(nameof(CreditsView), new CreditViewModel());

            DashboardCommand = new(o => 
            {
                CurrentView = _viewModels[nameof(DashboardView)];
            });
            ItemsCommand = new(o => 
            {
                CurrentView = _viewModels[nameof(ItemsView)];
            });
            ItemGiveCommand = new(o =>
            {
                CurrentView = _viewModels[nameof(ItemGiveView)];
            });
            PrefabCreatorCommand = new(o => 
            {
                if (CurrentView != _viewModels[nameof(PrefabCreatorView)])
                {
                    PrefabCreatorViewModel model = _viewModels[nameof(PrefabCreatorView)] as PrefabCreatorViewModel;
                    model.SelectedInventoryIndex = 0;
                }
                CurrentView = _viewModels[nameof(PrefabCreatorView)];
            });
            LobbyManagerCommand = new(o =>
            {
                CurrentView = _viewModels[nameof(LobbyManagerView)];
            });
            RegionManagerCommand = new(o => 
            {
                CurrentView = _viewModels[nameof(InvasionRegionsView)];
            });
            MiscCommand = new(o => 
            {
                CurrentView = _viewModels[nameof(MiscView)];
            });
            CreditsCommand = new(o =>
            {
                CurrentView = _viewModels[nameof(CreditsView)];
            });

            CurrentView = _viewModels[nameof(DashboardView)];
        }

        private void RegisterCommands()
        {
            commandManager.RegisterCommand(new DiscordInviteCommand());
            commandManager.RegisterCommand(new HookCommand(_hook));
            commandManager.RegisterCommand(new SetStatsCommand(_hook, new Player(_hook.PlayerIns, _hook)));
            commandManager.RegisterCommand(new HitboxCommand(_hook));
            commandManager.RegisterCommand(new CustomFPS(_hook));
            commandManager.RegisterCommand(new CustomFOV(_hook));
            commandManager.RegisterCommand(new Update(_vController));
            commandManager.RegisterCommand(new TestModal(_hook, this));
            commandManager.RegisterCommand(new TeamTypeChangeCommand(_hook));
            commandManager.RegisterCommand(new NetPlayerCommand(_hook));
            commandManager.RegisterCommand(new ChrTypeChange(_hook));
            commandManager.RegisterCommand(new HelpCommand(commandManager));
            commandManager.RegisterCommand(new OpenMenuCommand(_hook));
            commandManager.RegisterCommand(new FreeCamCommand(_hook));
            commandManager.RegisterCommand(new MassGibConsoleCommand(_hook));
            commandManager.RegisterCommand(new UpdateBuildCommand(_hook));
            //commandManager.RegisterCommand(new BetterSeamlessInvasionsCommand(_hook));
            commandManager.RegisterCommand(new SettingsCommand());
            commandManager.RegisterCommand(new SpecialEffectsCommand(_hook));
            commandManager.RegisterCommand(new AchievementCommand());
        }
    }
}
