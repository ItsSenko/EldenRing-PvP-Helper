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
        private PHPointer Base { get; set; }
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

            State = false;
        }

        private void Hook_OnSetup(object? sender, PHEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() => { Base = Hook.CreateBasePointer(Hook.Process.MainModule.BaseAddress); });
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
            Base.WriteByte(0x44ff72e, State ? (byte)1 : (byte)0);
            Base.WriteBytes(0x229df5, State ? new byte[] { 0xB0, 0x01 } : new byte[] { 0x32, 0xC0 });

            if (!State)
                Camera.WriteInt32(0xC8, 0);

            CommandManager.Log($"FreeCam Toggled {State}");
            if (State)
                CommandManager.Log("Use X+L3 or A+RightStick to use.");
        }
    }
}
