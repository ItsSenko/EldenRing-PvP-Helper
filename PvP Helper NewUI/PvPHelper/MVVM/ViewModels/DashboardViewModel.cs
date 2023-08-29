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
using System.Diagnostics;
using PvPHelper.MVVM.Commands.Dashboard;

namespace PvPHelper.MVVM.ViewModels
{
    internal class DashboardViewModel : ViewModelBase
    {
        #region DataBindings
        public ICommand RefillHPCommand { get; set; }
        public ICommand RefillFPCommand { get; set; }
        public ICommand NoDamageToggle { get; set; }
        public ICommand NoFPLossToggle { get; set; }

        public ICommand AutoReviveToggle { get; set; }
        public ICommand MadHealToggle { get; set; }

        public ICommand InfConsumeablesToggle { get; set; }
        public ICommand FastAnimsToggle { get; set; }

        public ICommand NoDeathToggle { get; set; }
        public ICommand NoStamLossToggle { get; set; }

        public ICommand AttachCommand { get; set; }
        public ICommand HelpCommand { get; set; }

        public ICommand ChangeColor { get; set; }
        public ICommand ResetColor { get; set; }

        private string _hpText;

        public string HPText
        {
            get { return _hpText; }
            set { _hpText = value; OnPropertyChanged(); }
        }

        private string _manaText;

        public string FPText
        {
            get { return _manaText; }
            set { _manaText = value; OnPropertyChanged(); }
        }

        private int _chrTypeSelectedIndex;

        public int ChrTypeSelectedIndex
        {
            get { return _chrTypeSelectedIndex; }
            set 
            {
                if (value != 0 && (!hook.Loaded || !hook.Hooked))
                {
                    _chrTypeSelectedIndex = 0;
                    ChrTypeSelectedIndex = 0;
                    OnPropertyChanged();
                    return;
                }
                _chrTypeSelectedIndex = value;
                UpdateChrType();
                OnPropertyChanged();
            }
        }

        private IEnumerable<object> _chrTypeItemsSource;    

        public IEnumerable<object> ChrTypeItemsSource
        {
            get { return _chrTypeItemsSource; }
            set 
            { 
                _chrTypeItemsSource = value;
                OnPropertyChanged();
            }
        }

        private string _attachIcon;

        public string AttachIcon
        {
            get { return _attachIcon; }
            set { _attachIcon = value; OnPropertyChanged(); }
        }

        private string _attachText;

        public string AttachText
        {
            get { return _attachText; }
            set { _attachText = value; OnPropertyChanged(); }
        }

        private ChrType[] ChrTypes = new ChrType[] 
        {
            new ChrType{ChrID = 0,
                    Name = "Normal"},

                new ChrType{ChrID = 1,
                    Name = "Yellow Phantom",
                    ParamID = 61,
                    edgeColor = Color.FromArgb(200, 120, 80),
                    diffMulColor = Color.FromArgb(255, 255, 255)},

                new ChrType{ChrID = 2,
                    Name = "Red Phantom",
                    ParamID = 60,
                    edgeColor = Color.FromArgb(255, 40, 40),
                    diffMulColor = Color.FromArgb(255, 210, 190),
                    frontColor = Color.FromArgb(0, 65, 95)},

                new ChrType{ChrID = 17,
                    Name = "Blue Phantom",
                    ParamID = 70,
                    edgeColor = Color.FromArgb(112, 118, 255),
                    diffMulColor = Color.FromArgb(255, 255, 255),
                    frontColor = Color.FromArgb(62, 65, 0)},
        };

        private ErdHook hook { get; set; }
        private Player player;
        private DispatcherTimer statsTimer;
        #endregion
        
        public DashboardViewModel(ErdHook hook)
        {
            this.hook = hook;
            player = new Player(hook.PlayerIns, hook);
            hook.OnSetup += Hook_OnSetup;
            hook.OnUnhooked += Hook_OnUnhooked;

            SetupCommands();
            ChrTypeItemsSource = ChrTypes;

            statsTimer = new DispatcherTimer();
            statsTimer.Interval = TimeSpan.FromSeconds(1);
            statsTimer.Tick += StatsTimer_Tick;
            

            //CommandManager.LogLoaded += Console_LogLoaded;
        }

        private void SetupCommands()
        {
            RefillHPCommand = new RelayCommand((o) => { player.Hp = player.HpMax; });
            RefillFPCommand = new RelayCommand((o) => { player.Fp = player.FpMax; });

            NoDamageToggle = new NoDamageToggle(hook);
            NoFPLossToggle = new NoFPLossToggle(hook);

            AutoReviveToggle = new AutoReviveToggle(hook, player);
            MadHealToggle = new MadHealToggle(hook);

            InfConsumeablesToggle = new InfiniteConsumeablesToggle(hook);
            FastAnimsToggle = new FastAnimsToggle(hook);

            NoDeathToggle = new NoDeathToggle(hook);
            NoStamLossToggle = new NoStamLossToggle(hook);

            ChangeColor = new ChangeChrTypeColor(hook, this);
            ResetColor = new ResetChrTypeColor(hook, this);

            HelpCommand = new RelayCommand((o) => 
            {
                var ps = new ProcessStartInfo("https://github.com/ItsSenko/EldenRing-PvP-Helper/wiki/Console-Commands")
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            });

            AttachCommand = new RelayCommand((o) => 
            {
                if (!hook.Hooked)
                {
                    CommandManager.Log("Atempting to attach to Elden Ring.");
                    CommandManager.Log("Might take a moment..");

                    hook.Start();
                }
            });

            AttachIcon = "Resources/Images/link-box-outline.svg";
            AttachText = "Not Attached";
        }
        private void Hook_OnUnhooked(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                statsTimer.Stop();
                AttachIcon = "Resources/Images/link-box-outline.svg";
                AttachText = "Not Attached";
            });
        }

        private void Hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                statsTimer.Start();
                AttachIcon = "Resources/Images/link-box.svg";
                AttachText = "Attached";
            });
        }

        private void StatsTimer_Tick(object? sender, EventArgs e)
        {
            if (player != null && hook.Loaded && hook.Hooked)
            {
                HPText = player.Hp.ToString();
                FPText = player.Fp.ToString();
            }
        }

        private void UpdateChrType()
        {
            if (!hook.Loaded || !hook.Hooked)
                return;

            ChrType chrType = (ChrTypeItemsSource.ToArray())[ChrTypeSelectedIndex] as ChrType;
            player.ChrType = chrType.ChrID;

            if (chrType.ChrID != 0)
            {
                CommandManager.Log("Please note that changing ChrType does save.");
                CommandManager.Log("Even when you restart you will be whatever you set it.");
                CommandManager.Log("If brought online, you will likely be banned.");
            }
        }
    }
}
