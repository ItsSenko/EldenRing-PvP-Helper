using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Entities;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using static Erd_Tools.Models.Param;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class BetterSeamlessInvasionsToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        private ErdHook hook;

        private bool isNewSession = false;
        private int hostIndex = 1;
        private int prevHostIndex = 1;
        public static Player currPlayer;

        private DispatcherTimer timer;

        public static PHPointer PlayerData1;
        public static PHPointer animationData;
        public static PHPointer inputData;
        public static PHPointer inputData2;
        public static PHPointer Session;

        NetPlayer hostPlayer;
        public static NetPlayer localPlayer;

        public static NetPlayer NetPlayer2;
        public static NetPlayer NetPlayer3;
        public static NetPlayer NetPlayer4;
        public static NetPlayer NetPlayer5;


        private List<NetPlayer> NetPlayerList = new();

        public BetterSeamlessInvasionsToggle(ErdHook hook)
        {
            this.hook = hook;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick; ;
            currPlayer = new(hook.PlayerIns, hook);

            PlayerData1 = hook.CreateChildPointer(hook.WorldChrMan, 0x10EF8, 0x0, 0x190, 0x68);
            inputData = hook.CreateChildPointer(hook.WorldChrMan, 0x10EF8, 0x0, 0x190, 0x0);
            inputData2 = hook.CreateChildPointer(hook.WorldChrMan, 0x10EF8, 0x0);
            animationData = CustomPointers.idleAnimation;
            Session = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8 });
            localPlayer = new(hook, Session, 0x0 * 10);
            hostPlayer = new(hook, Session, 0x10);

            NetPlayer2 = new(hook, Session, 0x20);
            NetPlayer3 = new(hook, Session, 0x30);
            NetPlayer4 = new(hook, Session, 0x40);
            NetPlayer5 = new(hook, Session, 0x50);

            NetPlayerList.Add(localPlayer);
            NetPlayerList.Add(hostPlayer);
            NetPlayerList.Add(NetPlayer2);
            NetPlayerList.Add(NetPlayer3);
            NetPlayerList.Add(NetPlayer4);
            NetPlayerList.Add(NetPlayer5);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!hook.Hooked || !hook.Setup)
            {
                State = false;
                return;
            }

            
            if (!string.IsNullOrEmpty(localPlayer.Name))
            {
                if (!string.IsNullOrEmpty(hostPlayer.Name))
                {
                    foreach (var player in NetPlayerList)
                    {

                    }
                }
                if ((isNewSession || prevHostIndex != hostIndex) && !string.IsNullOrEmpty(hostPlayer.Name))
                {
                    isNewSession = false;
                    prevHostIndex = hostIndex;
                    RespawnPlayer(hostPlayer);
                }
            }
            else
            {
                isNewSession = true;
                hostIndex = 1;
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
                timer.Start();
            else
                timer.Stop();

            isNewSession = _state;
        }

        public static void RespawnPlayer(NetPlayer teleportingPlayer)
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
            localPlayer.TeleportToPlayer(teleportingPlayer);
            CommandManager.Log($"Teleported to host: {teleportingPlayer.Name}");
            inputData2.WriteByte(0x531, Helpers.SetBit(inputData2.ReadByte(0x530), 5, true));
            //CommandManager.Log("Joystick Movement Disabled");
            animationData.WriteInt32(0x18, 63021);
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
