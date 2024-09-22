using Erd_Tools;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Misc
{
    //Original code author: Pav
    public class FreeCamToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook Hook;
        private PHPointer Camera;

        private string[] patchAOBs = {
        "E8 ?? ?? ?? ?? 84 C0 74 ?? 41 B0 01 BA ?? ?? ?? ?? 48 8B CE E8",
        "E8 ?? ?? ?? ?? 84 C0 74 ?? 8B 83 ?? ?? ?? ?? FF C8",
        "E8 ?? ?? ?? ?? 84 C0 0F 85 ?? ?? ?? ?? 38 03"};

        private List<PHPointer> pointers = new();

        public FreeCamToggle(ErdHook hook)
        {
            Hook = hook;
            Hook.OnSetup += Hook_OnSetup;
            Camera = hook.CreateChildPointer(CustomPointers.FieldArea2, 0x20);
            foreach (string aob in patchAOBs)
                pointers.Add(hook.RegisterRelativeAOB(aob, 1, 5));
        }

        private void Hook_OnSetup(object? sender, PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                pointers[0] = Hook.CreateBasePointer(getCallTarget(pointers[0].Resolve() + 0x9));
                pointers[1] = Hook.CreateBasePointer(getCallTarget(pointers[1].Resolve() + 0x9));
            });
        }

        public override void Execute(object? parameter)
        {
            if (!Hook.Hooked || !Hook.Loaded)
            {
                State = false;
                return;
            }

            if (!State)
            {
                Camera.WriteInt32(0xC8, 0);
                Hook.GameMan.WriteByte(0xBC4, 0);
            }

            if (State)
            {
                InformationDialog dialog = new("Use X+L3 or A+RightStick to use.");
                dialog.ShowDialog();
            }

            ApplyPatches(State);
        }

        private IntPtr getCallTarget(IntPtr address)
        {
            int i = Kernel32.ReadInt32(Hook.Handle, address + 1);
            return address + i + 5;
        }

        private void ApplyPatches(bool state)
        {
            byte[] bytes = { 0xB0, state ? (byte)1 : (byte)0, 0xC3 };
            foreach (PHPointer p in pointers)
                p.WriteBytes(0, bytes);
        }
    }
}
