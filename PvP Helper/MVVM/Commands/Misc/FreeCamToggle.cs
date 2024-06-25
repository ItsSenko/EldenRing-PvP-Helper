using Erd_Tools;
using PropertyHook;
using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class FreeCamToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook Hook;
        private PHPointer Base;
        private PHPointer Camera;

        public FreeCamToggle(ErdHook hook)
        {
            Hook = hook;

            Camera = hook.CreateChildPointer(CustomPointers.FieldArea2, 0x20);
        }
        public override void Execute(object? parameter)
        {
            if (!Hook.Hooked || !Hook.Loaded)
            {
                State = false;
                return;
            }

            if (Base == null)
                Base = Hook.CreateBasePointer(Hook.Process.MainModule.BaseAddress);

            Base.WriteByte(0x44ff72e, State ? (byte)1 : (byte)0);
            Base.WriteBytes(0x229df5, State ? new byte[] { 0xB0, 0x01 } : new byte[] { 0x32, 0xC0 });

            if (!State)
                Camera.WriteInt32(0xC8, 0);

            if (State)
            {
                InformationDialog dialog = new("Use X+L3 or A+RightStick to use.");
                dialog.ShowDialog();
            }
        }
    }
}
