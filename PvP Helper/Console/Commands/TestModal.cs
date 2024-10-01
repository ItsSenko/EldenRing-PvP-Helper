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
using Microsoft.Toolkit.Uwp.Notifications;
using PvPHelper.Core.Achievements;

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private ErdHook hook;

        private MainWindowViewModel viewModel;

        private Player player;

        private Hotkey hotkey;
        private PHPointer Session;
        private NetPlayer LocalNetPlayer;
        private NetPlayer NetPlayer1;

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
            Session = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8 });
            LocalNetPlayer = new(hook, Session, 0x0 * 10);
            NetPlayer1 = new(hook, Session, 0x10);
        }
        
        private void HotKeyPressed()
        {
            if (!hook.Hooked)
                return;

            player.ResetPlayerToDefault(hook);
        }

        protected override void OnTriggerCommand()
        {
            /*new ToastContentBuilder()
                .AddAppLogoOverride(new(Path.Combine(Directory.GetCurrentDirectory(), "Resources/Images/shunter.png")))
                .AddText("test")
                .AddText("test")
                .AddArgument("action", "focusApp")
                .Show();
            AchievementList.Default.WildSenko = false;
            AchievementList.Default.Save();*/
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded");

            hook.Warp(int.Parse(parameters[0]));
        }
    }
}
