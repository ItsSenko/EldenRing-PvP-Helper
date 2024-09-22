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


namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private ErdHook hook;

        private MainWindowViewModel viewModel;

        private NetPlayer player;
        private PHPointer Session;

        public TestModal(ErdHook hook, MainWindowViewModel viewModel)
        {
            CommandString = "/test";
            RequireParams = false;
            HasParams = true;
            this.hook = hook;
            this.viewModel = viewModel;

            Session = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8 });
            player = new(hook, Session, 0x0 * 10);
        }

        private void HotKeyTest_HotKeyPressed(object? sender, KeyPressedEventArgs e)
        {
            CommandManager.Log("Resetting");

            /*player.Hp = player.HpMax;
            player.Fp = player.FpMax;

            player.AddSpecialEffect(101);
            player.AddSpecialEffect(1673000);
            player.AddSpecialEffect(1673014);

            player.Poison = player.PoisonMax;
            player.Rot = player.RotMax;
            player.Bleed = player.BleedMax;
            player.Blight = player.BlightMax;
            player.Frost = player.FrostMax;
            player.Sleep = player.SleepMax;
            player.Madness = player.MadnessMax;*/
        }

        protected override void OnTriggerCommand()
        {
            //LeaveLobby();
            //player.Kick();
            
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded");

            hook.Warp(int.Parse(parameters[0]));
        }
    }
}
