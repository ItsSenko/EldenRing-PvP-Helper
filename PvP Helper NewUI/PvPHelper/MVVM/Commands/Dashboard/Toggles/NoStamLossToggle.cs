using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Entities;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    internal class NoStamLossToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook _hook;

        public NoStamLossToggle(ErdHook hook)
        {
            _hook = hook;
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Hooked || !_hook.Loaded)
            {
                State = false;
                return;
            }

            byte b = CustomPointers.ChrFlags.ReadByte(0x19B);
            CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 3, State));

            CommandManager.Log($"No Stam Loss Toggled {State}");
        }
    }
}
