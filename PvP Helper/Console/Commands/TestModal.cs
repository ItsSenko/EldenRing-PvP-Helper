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
using SoulsFormats;
using System.IO;
using PvPHelper.MVVM.Models.Search;
using PvPHelper.MVVM.Models.Search.SortOrders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PvPHelper.MVVM.Models.Builds;
using Keystone;
using System.Text.RegularExpressions;
using Kernel32 = PropertyHook.Kernel32;
using Architecture = Keystone.Architecture;
using System.Windows;
using System.Windows.Input;
using GlobalHotKey;
using System.Diagnostics;
using Erd_Tools.Models.Items;
using PvPHelper.Core.Hotkeys;

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private ErdHook hook;

        private MainWindowViewModel viewModel;

        private Player player;

        private Hotkey hotkey;

        public TestModal(ErdHook hook, MainWindowViewModel viewModel)
        {
            CommandString = "/test";
            RequireParams = false;
            HasParams = true;
            this.hook = hook;
            this.viewModel = viewModel;

            player = new(hook.PlayerIns, hook);

            hotkey = Hotkeys.Instance.GetSavedHotKey("Reset Player", new(Key.NumPad0, ModifierKeys.None));

            hotkey.OnPressed += HotKeyPressed;
        }
        private void HotKeyPressed()
        {
            if (!hook.Loaded)
            {
                CommandManager.Log("Not Loaded.");
                return;
            }

            CommandManager.Log("Resetting");

            player.Hp = player.HpMax;
            player.Fp = player.FpMax;

            player.AddSpecialEffect(101); // Grace Reset
            player.AddSpecialEffect(1673000); // Law of Regression visual effect
            player.AddSpecialEffect(1673014); // Law of Regression (Remove buffs and debuffs)

            player.Poison = player.PoisonMax;
            player.Rot = player.RotMax;
            player.Bleed = player.BleedMax;
            player.Blight = player.BlightMax;
            player.Frost = player.FrostMax;
            player.Sleep = player.SleepMax;
            player.Madness = player.MadnessMax;
        }

        protected override void OnTriggerCommand()
        {
            foreach(int i in player.GetAllSpecialEffects())
            {
                CommandManager.Log(i.ToString());
            }
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded");

            hook.Warp(int.Parse(parameters[0]));
        }
    }
}
