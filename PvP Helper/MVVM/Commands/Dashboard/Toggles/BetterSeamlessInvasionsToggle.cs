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

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    public class BetterSeamlessInvasionsToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        private ErdHook hook;

        private bool isNewSession = false;
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
                if (!isNewSession && !string.IsNullOrEmpty(hostPlayer.Name))
                {
                    isNewSession = true;

                    foreach(var player in NetPlayerList)
                    {
                        if (!string.IsNullOrEmpty(player.Name) && player.Health > 0)
                        {
                            localPlayer.PhantomID = Settings.Default.InvasionPhantomID;

                            SetInvasionState(false);

                            if (Settings.Default.CustomInvasionSpawn)
                            {
                                Thread.Sleep(1000);
                                animationData.WriteInt32(0x18, Settings.Default.SpawnAnimation);
                            }
                            return;
                        }
                    }
                }
                
            }
            else
            {
                isNewSession = false;
                SetInvasionState(true);
            }

        }

        public override void Execute(object? parameter)
        {
            if (!hook.Hooked || !hook.Loaded || !Helpers.GetIfModuleExists(hook.Process, "ersc.dll"))
            {
                State = false;
                return;
            }

            if (!Helpers.SeamlessItemsInitialized)
                Helpers.InitializeSeamlessItems();

            if (State)
                timer.Start();
            else
                timer.Stop();

            Row leaveItem = hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == (int)Helpers.SeamlessItems.LeavingItem);
            if (leaveItem == null)
                CommandManager.Log("Seamless Items Are Null.");
            else
            {
                SetInvasionState(true);
                var fieldOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "goodsUseAnim");
                leaveItem.Param.Pointer.WriteByte(leaveItem.DataOffset + fieldOffset.FieldOffset, State ? (byte)16 : (byte)9);
            }

            isNewSession = _state;
        }

        private void SetInvasionState(bool state)
        {
            Row invasionItem = hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == (int)Helpers.SeamlessItems.BreakInItem);
            Row runeArc = hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == 190);

            if (invasionItem == null)
                return;

            var fieldOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "goodsUseAnim");

            invasionItem.Param.Pointer.WriteByte(invasionItem.DataOffset + fieldOffset.FieldOffset, state ? (byte)16 : (byte)66);
            runeArc.Param.Pointer.WriteByte(runeArc.DataOffset + fieldOffset.FieldOffset, state ? (byte)16 : (byte)65);
        }

        // Credit to Indura for the original script: https://github.com/lndura/SeamlessRespawnScript/blob/main/script 
        public static void RespawnPlayer(NetPlayer teleportingPlayer)
        {
            SetFlags(true); // stop everything
            Thread.Sleep(5000); // Load in delay, prevents void out

            inputData2.WriteByte(0x531, Helpers.SetBit(inputData2.ReadByte(0x531), 0, false)); //resume updating

            localPlayer.TeleportToPlayer(teleportingPlayer); //Teleport to specified player
            animationData.WriteInt32(0x18, Settings.Default.SpawnAnimation); //spawn animation
            currPlayer.Hp = currPlayer.HpMax; //Reset health incase damage was taken at some point

            CommandManager.Log($"Teleported to host: {teleportingPlayer.Name}");

            Thread.Sleep(2000);

            SetFlags(false); //resume everything
        }

        private static void SetFlags(bool state)
        {
            int i = state? 1 : 0;
            PlayerData1.WriteByte(0x1D3, (byte)i);
            
            inputData.WriteByte(0x19B, Helpers.SetBit(inputData.ReadByte(0x19B), 0, state));
            inputData.WriteByte(0x19B, Helpers.SetBit(inputData.ReadByte(0x19B), 1, state));
            inputData2.WriteByte(0x530, Helpers.SetBit(inputData2.ReadByte(0x530), 5, state));
        }
    }
}
