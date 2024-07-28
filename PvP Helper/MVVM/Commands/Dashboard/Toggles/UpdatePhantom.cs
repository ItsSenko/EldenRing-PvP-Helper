using Erd_Tools;
using Erd_Tools.Models.Entities;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.Core;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    public class UpdatePhantom : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        private ErdHook hook;
        private PHPointer LocalPlayer;

        private DashboardViewModel viewModel;
        private DispatcherTimer updateTimer;

        private int SelectedPhantomID;

        public UpdatePhantom(ErdHook hook, PHPointer localPlayer, DashboardViewModel viewModel)
        {
            this.hook = hook;
            LocalPlayer = localPlayer;
            this.viewModel = viewModel;

            updateTimer = new();
            updateTimer.Interval = TimeSpan.FromSeconds(2);
            updateTimer.Tick += UpdateTimer_Tick;
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (!hook.Hooked)
            {
                State = false;
                updateTimer.Stop();
                return;
            }

            if (hook.Loaded)
            {
                var id = ((PhantomIDOption)viewModel.PhantomIDItemsSource.ToArray()[viewModel.PhantomIDSelectedIndex]).ID;
                if (LocalPlayer.ReadInt32(0x538) != id)
                    LocalPlayer.WriteInt32(0x538, id);
            }
        }

        public override void Execute(object? parameter)
        {
            if (!hook.Hooked || !hook.Loaded)
            {
                State = false;
                return;
            }

            if (State)
                updateTimer.Start();
            else
                updateTimer.Stop();
        }
    }
}
