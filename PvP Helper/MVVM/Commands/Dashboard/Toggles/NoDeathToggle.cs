using Erd_Tools;
using PvPHelper.Console;
using PvPHelper.Core;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    internal class NoDeathToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook _hook;

        public NoDeathToggle(ErdHook hook)
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
            CustomPointers.ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, State));

            CommandManager.Log($"NoDeath toggled to {State}");
        }
    }
}
