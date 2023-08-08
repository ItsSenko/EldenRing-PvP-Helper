using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Console;
using PvPHelper.Console.Commands;
using PvPHelper.Core;
using PvPHelper.MVVM.Views;
using System.Collections.Generic;
using System.Windows;

namespace PvPHelper.MVVM.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region RelayCommands
        public RelayCommand DashboardCommand { get; set; }
        public RelayCommand ItemsCommand { get; set; }
        public RelayCommand PrefabCreatorCommand { get; set; }
        public RelayCommand YourPrefabsCommand { get; set; }
        public RelayCommand MiscCommand { get; set; }
        #endregion
        private Dictionary<string, ViewModelBase> _viewModels = new();
        private object _currentView;

        private ErdHook _hook;
        public static Player player;
        public bool HookCompletelyLoaded 
        {
            get { return _hook is not null && _hook.Hooked && _hook.Loaded; }
        }
        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value; 
                OnPropertyChanged();
            }
        }
        
        public CommandManager commandManager;
        public MainWindowViewModel()
        {
            _hook = new(5000, 1000, new(x => x.MainWindowTitle is "ELDEN RING™" or "ELDEN RING"));
            commandManager = new();
            _hook.OnSetup += _hook_OnSetup;
            _hook.OnUnhooked += _hook_OnUnhooked;
            player = new Player(_hook.PlayerIns, _hook);

            CustomPointers.Initialize(_hook);
            SetupViewModels();
            RegisterCommands();
        }

        private void _hook_OnUnhooked(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => { CommandManager.Log("No longer hooked to Elden Ring."); });
        }

        private void _hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => { CommandManager.Log("Setup Hook for Elden Ring"); });
        }

        private void SetupViewModels()
        {
            _viewModels.Add(nameof(DashboardView), new DashboardViewModel(_hook));
            _viewModels.Add(nameof(ItemsView), new ItemsViewModel(_hook));

            DashboardCommand = new(o => 
            {
                CurrentView = _viewModels[nameof(DashboardView)];
            });
            ItemsCommand = new(o => 
            {
                CurrentView = _viewModels[nameof(ItemsView)];
            });
            PrefabCreatorCommand = new(o => 
            {
                CurrentView = _viewModels["PrefabCreatorView"];
            });
            YourPrefabsCommand = new(o => 
            {
                CurrentView = _viewModels["YourPrefabsView"];
            });
            MiscCommand = new(o => 
            {
                CurrentView = _viewModels["MiscView"];
            });

            CurrentView = _viewModels[nameof(DashboardView)];
        }

        private void RegisterCommands()
        {
            commandManager.RegisterCommand(new DiscordInviteCommand());
            commandManager.RegisterCommand(new HookCommand(_hook));
            commandManager.RegisterCommand(new SetStatsCommand(_hook, new Player(_hook.PlayerIns, _hook)));
        }
    }
}
