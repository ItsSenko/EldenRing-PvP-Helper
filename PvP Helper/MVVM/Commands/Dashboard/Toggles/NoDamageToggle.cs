using Erd_Tools;
using PvPHelper.Console;
using PvPHelper.Core;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    internal class NoDamageToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State 
        { 
            get => _state; 
            set => SetField(ref _state, value); 
        }
        private ErdHook _hook;
        public NoDamageToggle(ErdHook hook)
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
            CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 1, State));

            CommandManager.Log($"NoDamage toggled to {State}");
        }
    }
}
