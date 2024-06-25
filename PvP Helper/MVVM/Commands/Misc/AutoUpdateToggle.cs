using PvPHelper.Core;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class AutoUpdateToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        public AutoUpdateToggle() 
        {
            State = Settings.Default.AutoUpdate;
        }
        public override void Execute(object? parameter)
        {
            Settings.Default.AutoUpdate = State;
            Settings.Default.Save();
        }
    }
}
