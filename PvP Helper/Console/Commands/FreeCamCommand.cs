using Erd_Tools;
using PropertyHook;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PvPHelper.Console.Commands
{
    internal class FreeCamCommand : CommandBase
    {
        private ErdHook Hook { get; set; }
        private bool State { get; set; }

        private PHPointer Camera { get; set; }

        private string[] patchAOBs = {
        "E8 ?? ?? ?? ?? 84 C0 74 ?? 41 B0 01 BA ?? ?? ?? ?? 48 8B CE E8",
        "E8 ?? ?? ?? ?? 84 C0 74 ?? 8B 83 ?? ?? ?? ?? FF C8",
        "E8 ?? ?? ?? ?? 84 C0 0F 85 ?? ?? ?? ?? 38 03"};

        private List<PHPointer> pointers = new();
        public FreeCamCommand(ErdHook hook)
        {
            Hook = hook;
            Hook.OnSetup += Hook_OnSetup;

            Name = "FreeCam Command";
            Description = "Control your camera";
            CommandString = "/freecam";
            HasParams = true;
            RequiresParamsString = new string[] { "state" };

            Camera = hook.CreateChildPointer(CustomPointers.FieldArea2, 0x20);
            foreach (string aob in patchAOBs)
                pointers.Add(hook.RegisterRelativeAOB(aob, 1, 5));

            State = false;
        }

        private void Hook_OnSetup(object? sender, PHEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() => 
            {
                pointers[0] = Hook.CreateBasePointer(getCallTarget(pointers[0].Resolve() + 0x9));
                pointers[1] = Hook.CreateBasePointer(getCallTarget(pointers[1].Resolve() + 0x9));
            });
        }

        protected override void OnTriggerCommand()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                throw new InvalidCommandException("Not hooked or loaded.");

            State = !State;
            ToggleFreeCam();
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!Hook.Loaded || !Hook.Hooked)
                throw new InvalidCommandException("Not hooked or loaded.");

            if (!bool.TryParse(parameters[0], out bool newState))
                throw new InvalidCommandException("Invalid State");

            State = newState;
            ToggleFreeCam();
        }

        private void ToggleFreeCam()
        {
            if (!State)
            {
                Camera.WriteInt32(0xC8, 0);
                Hook.GameMan.WriteByte(0xBC4, 0);
            }

            ApplyPatches(State);

            CommandManager.Log($"FreeCam Toggled {State}");
            if (State)
                CommandManager.Log("Use X+L3 or A+RightStick to use.");
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
