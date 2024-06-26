using Erd_Tools;
using PropertyHook;
using PvPHelper.Core;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using System;
using Erd_Tools.Models;
using System.Linq;
using static Erd_Tools.Models.Weapon;
using PvPHelper.MVVM.ViewModels;
using PvPHelper.MVVM.Models.Regions;
using System.Runtime.InteropServices;
using PvPHelper.MVVM.Views;
using Erd_Tools.Models.Entities;
using PvPHelper.Core.Extensions;

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private ErdHook hook;
        private DispatcherTimer timer;
        private PHPointer ArrayStartPtr;
        private PHPointer ArrayEndPtr;

        private MainWindowViewModel viewModel;
        public TestModal(ErdHook hook, MainWindowViewModel viewModel)
        {
            CommandString = "/test";
            //RequireParams = true;
            HasParams = true;
            this.hook = hook;
            this.viewModel = viewModel;
            ArrayStartPtr = hook.CreateChildPointer(hook.GameDataMan, new int[] { 0x8, 0x918 });
            ArrayEndPtr = hook.CreateChildPointer(hook.GameDataMan, new int[] { 0x8, 0x920 });
        }

        protected override void OnTriggerCommand()
        {
            Player player = new(hook.PlayerIns, hook);

            CommandManager.Log(player._instance.Resolve().ToString("X"));
            CommandManager.Log(player.Data.Resolve().ToString("X"));
            CommandManager.Log(player.ChrData.Resolve().ToString("X"));
            CommandManager.Log(player.GetRunes().ToString());
        }
        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            switch(parameters[1])
            {
                case "r":
                    {
                        int.TryParse(parameters[0], out int result);
                        CommandManager.Log(hook.IsEventFlag(result).ToString());
                        break;
                    }
                case "s":
                    {
                        int.TryParse(parameters[0], out int result);
                        bool.TryParse(parameters[2], out bool state);
                        hook.SetEventFlag(result, state);
                        break;
                    }
            }
        }
    }
}
