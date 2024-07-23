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
using System.Diagnostics;
using PvPHelper.MVVM.Commands.Dashboard;
using Erd_Tools.Models;
using PropertyHook;
using System.Threading.Tasks;
using System.Threading;
using PvPHelper.MVVM.Dialogs;
using PvPHelper.MVVM.Commands.Misc;

namespace PvPHelper.MVVM.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        #region DataBindings
        public ICommand RefillHPCommand { get; set; }
        public ICommand RefillFPCommand { get; set; }
        public ICommand NoDamageToggle { get; set; }
        public ICommand NoFPLossToggle { get; set; }

        public ICommand AutoReviveToggle { get; set; }
        public ICommand MadHealToggle { get; set; }

        public ICommand InfConsumeablesToggle { get; set; }
        public ICommand BSI { get; set; }

        public ICommand NoDeathToggle { get; set; }
        public ICommand NoStamLossToggle { get; set; }

        public ICommand AttachCommand { get; set; }
        public ICommand HelpCommand { get; set; }

        public ICommand ChangeColor { get; set; }
        public ICommand ResetColor { get; set; }
        public UpdatePhantom UpdatePhantom { get; set; }

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

        private int _phantomIDSelectedIndex;

        public int PhantomIDSelectedIndex
        {
            get { return _phantomIDSelectedIndex; }
            set 
            {
                if (value != 0 && (!hook.Loaded || !hook.Hooked))
                {
                    _phantomIDSelectedIndex = 0;
                    PhantomIDSelectedIndex = 0;
                    OnPropertyChanged();
                    return;
                }
                _phantomIDSelectedIndex = value;
                UpdatePhantomID();
                OnPropertyChanged();
            }
        }

        private IEnumerable<object> _phantomIDItemsSource;    

        public IEnumerable<object> PhantomIDItemsSource
        {
            get { return _phantomIDItemsSource; }
            set 
            {
                _phantomIDItemsSource = value;
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

        private ErdHook hook { get; set; }
        private Player player;
        private PHPointer LocalPlayer;
        private DispatcherTimer statsTimer;
        private Param PhantomParam => hook.Params.FirstOrDefault(x => x.Name == "PhantomParam");
        #endregion
        
        public DashboardViewModel(ErdHook hook, PHPointer localPlayer)
        {
            this.hook = hook;
            player = new Player(hook.PlayerIns, hook);
            hook.OnSetup += Hook_OnSetup;
            hook.OnUnhooked += Hook_OnUnhooked;
            hook.OnHooked += Hook_OnHooked;
            LocalPlayer = localPlayer;

            SetupCommands();

            statsTimer = new DispatcherTimer();
            statsTimer.Interval = TimeSpan.FromSeconds(1);
            statsTimer.Tick += StatsTimer_Tick;
            //CommandManager.LogLoaded += Console_LogLoaded;
        }

        private void Hook_OnHooked(object? sender, PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => { CommandManager.Log("Hooked but not setup"); });
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
            BSI = new BetterSeamlessInvasionsToggle(hook);

            NoDeathToggle = new NoDeathToggle(hook);
            NoStamLossToggle = new NoStamLossToggle(hook);

            ChangeColor = new ChangeChrTypeColor(hook, this);
            //ResetColor = new ResetChrTypeColor(hook, this);

            UpdatePhantom = new(hook, LocalPlayer, this);

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
                    hook.Start();
                    CommandManager.Log("Attempting to attach to Elden Ring.");
                    CommandManager.Log("Might take a moment..");


                    DispatcherTimer timer = new();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += (s, e) =>
                    {
                        if (!Settings.Default.DebugLogs)
                            return;
                        try
                        {
                            if (hook.Hooked)
                            {
                                timer.Stop();
                                return;
                            }

                            CommandManager.Log($"Proccess ID: {(hook.Process != null ? hook.Process?.Id : "it was null")}");
                            CommandManager.Log($"WorldChrMan: {(hook.WorldChrMan != null ? hook.WorldChrMan?.Resolve().ToInt64().ToString("X2") : "it was null")}");
                            CommandManager.Log($"ItemGib: {(hook.ItemGive != null ? hook.ItemGive?.Resolve().ToInt64().ToString("X2") : "it was null")}");
                            CommandManager.Log($"GameDataMan: {(hook.GameDataMan != null ? hook.GameDataMan?.Resolve().ToInt64().ToString("X2") : "it was null")}");
                            CommandManager.Log($"SoloParamRepository: {(hook.SoloParamRepository != null ? hook.SoloParamRepository?.Resolve().ToInt64().ToString("X2") : "it was null")}");
                        }
                        catch { }
                    };
                    timer.Start();
                }
            });

            AttachIcon = "Resources/Images/not_attached.svg";
            AttachText = "Not Attached";
        }
        private void Hook_OnUnhooked(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                statsTimer.Stop();
                AttachIcon = "Resources/Images/not_attached.svg";
                AttachText = "Not Attached";
            });
        }

       
        private void Hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                statsTimer.Start();
                AttachIcon = "Resources/Images/attached.svg";
                AttachText = "Attached";

                List<PhantomIDOption> phantomIds = new();
                phantomIds.Add(new(Helpers.GetNewPhantomName(-1), -1));
                foreach (var row in PhantomParam.Rows)
                {
                    phantomIds.Add(new(Helpers.GetNewPhantomName(row.ID), row.ID));
                }
                PhantomIDItemsSource = phantomIds;
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

        private void UpdatePhantomID()
        {
            if (!hook.Loaded || !hook.Hooked)
                return;

            PhantomIDOption option = PhantomIDItemsSource.ToArray()[PhantomIDSelectedIndex] as PhantomIDOption;

            if (option == null)
                return;

            LocalPlayer.WriteInt32(0x538, option.ID);

            CommandManager.Log($"Overriden local PhantomID to be {option.Name}");
        }
    }
}
