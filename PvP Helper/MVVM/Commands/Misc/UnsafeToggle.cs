using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class UnsafeToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        public UnsafeToggle()
        {
            State = Settings.Default.AllowUnsafe;
        }
        public override void Execute(object? parameter) 
        {
            if (State)
            {
                InformationDialog dialog = new("Are you sure you want to Allow Unsafe Options? Enabling this allows changing things that are saved to your save file and can get you banned if brought online.");
                dialog.OnOk += () => {
                    Settings.Default.AllowUnsafe = State;
                    Settings.Default.Save();
                };
                dialog.OnCancel += () => {
                    State = false;
                    Settings.Default.AllowUnsafe = State;
                    Settings.Default.Save();
                };
                dialog.ShowDialog();
            }
        }
    }
}
