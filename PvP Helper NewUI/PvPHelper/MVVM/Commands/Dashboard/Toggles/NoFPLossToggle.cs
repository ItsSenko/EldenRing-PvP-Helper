using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    internal class NoFPLossToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State
        {
            get => _state;
            set => SetField(ref _state, value);
        }
        private ErdHook _hook;
        public NoFPLossToggle(ErdHook hook)
        {
            _hook = hook;
            State = false;
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Loaded || !_hook.Hooked)
            {
                State = false;
                return;
            }

            byte b = CustomPointers.ChrFlags.ReadByte(0x19B);
            CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 2, State));
        }
    }
}
