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

namespace PvPHelper.Console.Commands
{
    internal class BetterSeamlessInvasionsCommand : CommandBase
    {
        private ErdHook hook;
        private bool state = false;
        private bool isNewSession = false;
        private int hostIndex = 1;
        private int prevHostIndex = 1;
        private Player currPlayer;

        private DispatcherTimer timer;

        PHPointer PlayerData1;
        PHPointer animationData;
        PHPointer inputData;
        PHPointer inputData2;
        PHPointer Session;

        NetPlayer hostPlayer;
        NetPlayer localPlayer;
        public BetterSeamlessInvasionsCommand(ErdHook hook)
        {
            CommandString = "/bsi";
            RequireParams = false;
            HasParams = true;
            this.hook = hook;
            RequiresParamsString = new string[] { "state" };

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            currPlayer = new(hook.PlayerIns, hook);

            PlayerData1 = hook.CreateChildPointer(hook.WorldChrMan, 0x10EF8, 0x0, 0x190, 0x68);
            inputData = hook.CreateChildPointer(hook.WorldChrMan, 0x10EF8, 0x0, 0x190, 0x0);
            inputData2 = hook.CreateChildPointer(hook.WorldChrMan, 0x10EF8, 0x0);
            animationData = CustomPointers.idleAnimation;
            Session = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8 });
            localPlayer = new(hook, Session, 0x0*10);
            hostPlayer = new(hook, Session, 0x10);
        }

        protected override void OnTriggerCommand()
        {
            if (!hook.Hooked || !hook.Loaded)
            {
                state = false;
                throw new InvalidCommandException("Not attached to Elden Ring.");
            }

            state = !state;

            if (state)
            {
                timer.Start();
                CommandManager.Log("Better Seamless Invasions Enabled");
            }
            else
            {
                timer.Stop();
                CommandManager.Log("Better Seamless Invasions Disabled");
            }

            isNewSession = state;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!hook.Hooked || !hook.Setup)
            {
                state = false;
                throw new InvalidCommandException("Not attached to Elden Ring.");
            }

            if (!string.IsNullOrEmpty(localPlayer.Name))
            {
                if ((isNewSession || prevHostIndex != hostIndex) && !string.IsNullOrEmpty(hostPlayer.Name))
                {
                    isNewSession = false;
                    prevHostIndex = hostIndex;
                    CommandManager.Log("New Session Detected, Teleporting to Host...");
                    RespawnPlayer();
                }
            }
            else
            {
                isNewSession = true;
                hostIndex = 1;
            }
        }

        private void RespawnPlayer()
        {
            PlayerData1.WriteByte(0x1D3, 1);
            //CommandManager.Log("Gravity Disabled");
            inputData2.WriteByte(0x531, Helpers.SetBit(inputData2.ReadByte(0x531), 0, true));
            //CommandManager.Log("Froze Player");
            Thread.Sleep(2000);

            int health = localPlayer.Health;
            inputData.WriteByte(0x19B, Helpers.SetBit(inputData.ReadByte(0x19B), 0, true));
            inputData.WriteByte(0x19B, Helpers.SetBit(inputData.ReadByte(0x19B), 1, true));
            Thread.Sleep(3000);

            inputData2.WriteByte(0x531, Helpers.SetBit(inputData2.ReadByte(0x531), 0, false));
            //CommandManager.Log("Unfroze Player");
            localPlayer.TeleportToPlayer(hostPlayer);
            CommandManager.Log($"Teleported to host: {hostPlayer.Name}");
            inputData2.WriteByte(0x531, Helpers.SetBit(inputData2.ReadByte(0x530), 5, true));
            //CommandManager.Log("Joystick Movement Disabled");
            animationData.WriteInt32(0x18, 60501);
            //CommandManager.Log("Set Spawn Animation");
            currPlayer.Hp = health;
            Thread.Sleep(500);

            PlayerData1.WriteByte(0x1D3, 0);
            //CommandManager.Log("Gravity Enabled");
            inputData.WriteByte(0x19B, Helpers.SetBit(inputData.ReadByte(0x19B), 1, false));
            inputData.WriteByte(0x19B, Helpers.SetBit(inputData.ReadByte(0x19B), 0, false));
            inputData2.WriteByte(0x531, Helpers.SetBit(inputData2.ReadByte(0x530), 5, false));
            //CommandManager.Log("Joystick Movement Enabled");
        }
    }
}
