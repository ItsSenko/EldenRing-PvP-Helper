using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Console;
using PvPHelper.Core;
using PvPHelper.MVVM.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommandManager = PvPHelper.Console.CommandManager;
using PvPHelper.MVVM.Commands.Dashboard.Toggles;
using System.Windows.Threading;
using PvPHelper.MVVM.Models;
using System.Drawing;
using System.Collections.ObjectModel;

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
        #endregion

        private ErdHook _hook;
        private Player _player;
        public ItemsViewModel(ErdHook hook)
        {
            _hook = hook;
            _player = new(hook.PlayerIns, hook);

            _hook.OnSetup += _hook_OnSetup;
            SetupCommands();
        }

        private void _hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => { SetStats(); });
        }

        private void SetupCommands()
        {
            RefreshStats = new RelayCommand((o) => { SetStats(); });
        }
        public void SetStats()
        {
            if (!_hook.Loaded)
                return;

            ChrName = _player.PlayerName;
            ChrLevel = "Lvl: "+_player.SoulLevel.ToString();

            Vigor = "Vigor: " + _player.Vigor.ToString();
            Mind = "Mind: " + _player.Mind.ToString();
            Endurance = "Endurance: " + _player.Endurance.ToString();
            Strength = "Strength: " + _player.Strength.ToString();
            Dexterity = "Dexterity: " + _player.Dexterity.ToString();
            Intelligence = "Intelligence: " + _player.Intelligence.ToString();
            Faith = "Faith: " + _player.Faith.ToString();
            Arcane = "Arcane: " + _player.Arcane.ToString();
        }
    }
}
