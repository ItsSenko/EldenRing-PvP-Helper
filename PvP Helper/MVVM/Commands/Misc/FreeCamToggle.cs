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

        private string[] patchAOBs = {
        "E8 ?? ?? ?? ?? 84 C0 74 ?? 41 B0 01 BA ?? ?? ?? ?? 48 8B CE E8",
        "E8 ?? ?? ?? ?? 84 C0 74 ?? 8B 83 ?? ?? ?? ?? FF C8",
        "E8 ?? ?? ?? ?? 84 C0 0F 85 ?? ?? ?? ?? 38 03"};

        private List<PHPointer> pointers = new();

        public FreeCamToggle(ErdHook hook)
        {
            Hook = hook;

            Camera = hook.CreateChildPointer(CustomPointers.FieldArea2, 0x20);

            pointers.Add(hook.RegisterRelativeAOB(patchAOBs[0], 1, 5));
            pointers.Add(hook.RegisterRelativeAOB(patchAOBs[1], 11, 10));
            pointers.Add(hook.RegisterRelativeAOB(patchAOBs[2], 11, 10));
        }
        public override void Execute(object? parameter)
        {
            if (!Hook.Hooked || !Hook.Loaded)
            {
                State = false;
                return;
            }

            /*if (Base == null)
                Base = Hook.CreateBasePointer(Hook.Process.MainModule.BaseAddress);

            Base.WriteByte(0x44ff72e, State ? (byte)1 : (byte)0);
            Base.WriteBytes(0x229df5, State ? new byte[] { 0xB0, 0x01 } : new byte[] { 0x32, 0xC0 });*/

            SetCamera(State);

            if (!State)
                Camera.WriteInt32(0xC8, 0);

            if (State)
            {
                InformationDialog dialog = new("Use X+L3 or A+RightStick to use.");
                dialog.ShowDialog();
            }
        }

        private void SetCamera(bool state)
        {
            byte[] bytes = { 0xB0, state ? (byte)1 : (byte)0, 0xC3 };
            foreach (PHPointer pointer in pointers)
            {
                pointer.WriteBytes(0x0, bytes);
            }
        }
    }
}
