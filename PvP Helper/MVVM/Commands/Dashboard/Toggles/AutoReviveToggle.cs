using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Console;
using PvPHelper.Core;
using PvPHelper.Core.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    //Original code author: Jouta Kujo
    internal class AutoReviveToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook _hook;
        private Player _player;
        private DispatcherTimer autoReviveTimer;
        private DispatcherTimer noDeadTimer;
        public AutoReviveToggle(ErdHook hook, Player player)
        {
            _hook = hook;
            _player = player;

            autoReviveTimer = new DispatcherTimer();
            autoReviveTimer.Interval = TimeSpan.FromMilliseconds(1);
            autoReviveTimer.Tick += AutoReviveTimer_Tick;
            noDeadTimer = new();
            noDeadTimer.Interval = TimeSpan.FromMilliseconds(1);
            noDeadTimer.Tick += noDeadTick;
        }
        private void noDeadTick(object? sender, EventArgs e)
        {
            byte b = CustomPointers.ChrFlags.ReadByte(0x19B);

            float currentHealthPercent = ((float)_player.Hp / (float)_player.HpMax) * 100;

            if (currentHealthPercent < 21)
            {
                bool hasTear = _player.GetAllSpecialEffects().Contains(3512);
                if (hasTear)
                    CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, false));
                else
                    CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, true));
            }
            else
                CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, true));
        }
        private void AutoReviveTimer_Tick(object? sender, EventArgs e)
        {
            byte b = CustomPointers.ChrFlags.ReadByte(0x19B);
            int anim = CustomPointers.animPointer.ReadInt32(0x90);

            // Check for throw
            if (anim == 70000 || anim == 70010)
            {
                CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, true));
                if (_player.Hp <= 1)
                {
                    CustomPointers.idleAnimation.WriteInt32(0x18, 60502);
                    Thread.Sleep(500);
                    Revive();
                }
            }

            if (_player.Hp == 1)
            {
                CustomPointers.idleAnimation.WriteInt32(0x18, 60502);
                Thread.Sleep(500);
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
            {
                autoReviveTimer.Start();
                noDeadTimer.Start();
            }
            else
            {
                autoReviveTimer.Stop();
                noDeadTimer.Stop();

                byte b = CustomPointers.ChrFlags.ReadByte(0x19B);
                CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, false));
            }

            CommandManager.Log($"Toggled AutoRevived to {State}");
        }

        private void Revive()
        {
            _player.ResetPlayerToDefault(_hook);
            CustomPointers.idleAnimation.WriteInt32(0x18, 0);
        }
    }
}
