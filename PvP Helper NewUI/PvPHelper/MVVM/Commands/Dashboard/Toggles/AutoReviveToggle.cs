using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Console;
using PvPHelper.Core;
using System;
using System.Threading;
using System.Windows.Threading;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    internal class AutoReviveToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook _hook;
        private Player _player;
        private DispatcherTimer autoReviveTimer;
        public AutoReviveToggle(ErdHook hook, Player player)
        {
            _hook = hook;
            _player = player;

            autoReviveTimer = new DispatcherTimer();
            autoReviveTimer.Interval = TimeSpan.FromMilliseconds(10);
            autoReviveTimer.Tick += AutoReviveTimer_Tick;
        }

        private void AutoReviveTimer_Tick(object? sender, EventArgs e)
        {
            int anim = CustomPointers.animPointer.ReadInt32(0x90);
            if (anim == 70000 || anim == 70010)
            {
                byte b = CustomPointers.ChrFlags.ReadByte(0x19B);

                CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, true));
                if (_player.Hp <= 1)
                {
                    Thread.Sleep(1000);
                    Revive();
                }
            }
            else
            {
                byte b = CustomPointers.ChrFlags.ReadByte(0x19B);

                CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, false));
            }

            if (_player.Hp == 0)
            {
                Thread.Sleep(1000);
                Revive();
                CommandManager.Log("Revived Player.");
            }
        }

        public override void Execute(object? parameter)
        {
            if (!_hook.Hooked || !_hook.Loaded)
            {
                State = false;
                return;
            }

            if (State)
                autoReviveTimer.Start();
            else
                autoReviveTimer.Stop();

            CommandManager.Log($"Toggled AutoRevived to {State}");
        }

        private void Revive()
        {
            _player.Hp = _player.HpMax;
            _player.Fp = _player.FpMax;
            CustomPointers.idleAnimation.WriteInt32(0x18, 0);
        }
    }
}
