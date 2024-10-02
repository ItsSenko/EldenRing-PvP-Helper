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
using PvPHelper.Core.Extensions;

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

        private const string ItemGibCallAOB = "8B 02 83 F8 0A";

        private PHPointer ItemGibCall;
        private IntPtr newmem;

        private byte[] originalBytes;
        private byte[] newBytes;
        private byte[] backupBytes = { 0x40, 0x55, 0x56, 0x57, 0x41, 0x54 };

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

            ItemGibCall = hook.RegisterAbsoluteAOB(ItemGibCallAOB);
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

            if (!setup)
                Setup();

            byte[] bytes = State ? newBytes : backupBytes;

            Kernel32.WriteBytes(hook.Handle, ItemGibCall.Resolve() - 0x52, bytes);

            isNewSession = _state;
        }

        private bool setup = false;
        private void Setup()
        {
            if (setup)
                return;

            string asm = Helpers.GetEmbededResource("Resources.Assembly.ItemGibDisable.asm");
            string newMemAsm = "ret";

            newBytes = Helpers.GetAssembledBytes(newMemAsm);
            newmem = hook.GetPrefferedIntPtr(newBytes.Length);

            string formatted = string.Format(asm, newmem);
            byte[] asmBytes = Helpers.GetAssembledBytes(formatted);

            originalBytes = Kernel32.ReadBytes(hook.Handle, ItemGibCall.Resolve() - 0x52, (uint)asmBytes.Length);

            if (newBytes == originalBytes)
            {
                originalBytes = backupBytes;
            }

            Kernel32.WriteBytes(hook.Handle, newmem, newBytes);

            if (Settings.Default.DebugLogs)
            {
                CommandManager.Log(newmem.ToString("X"));
                CommandManager.Log(newBytes.BytesToString());

                CommandManager.Log("asm");
                CommandManager.Log(formatted);
                CommandManager.Log(asmBytes.BytesToString());

                CommandManager.Log((ItemGibCall.Resolve() - 0x52).ToString("X"));
                CommandManager.Log(originalBytes.BytesToString());
            }

            setup = true;
        }
    }
}
