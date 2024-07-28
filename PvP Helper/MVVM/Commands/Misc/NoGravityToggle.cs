using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erd_Tools;
using PropertyHook;
using PvPHelper.Core;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class NoGravityToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        private ErdHook hook;
        private PHPointer gravityPointer;

        public NoGravityToggle(ErdHook hook)
        {
            this.hook = hook;
            gravityPointer = hook.CreateChildPointer(hook.WorldChrMan, 0x10EF8, 0x0, 0x190, 0x68);
        }

        public override void Execute(object? parameter)
        {
            if (!hook.Hooked || !hook.Loaded)
            {
                State = false;
                return;
            }

            gravityPointer.WriteByte(0x1D3, State ? (byte)1 : (byte)0);
        }
    }
}
