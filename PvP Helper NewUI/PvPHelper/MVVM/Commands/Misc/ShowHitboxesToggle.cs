using Erd_Tools;
using PvPHelper.Console;
using PvPHelper.Core;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class ShowHitboxesToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook Hook;
        public ShowHitboxesToggle(ErdHook hook)
        {
            Hook = hook;
        }
        public override void Execute(object? parameter)
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            CustomPointers.dHitbox.WriteByte(0xA1, State ? (byte)1 : (byte)0);
            CommandManager.Log($"Hitboxes {(State ? "Shown" : "Hidden")}");
        }
    }
}
