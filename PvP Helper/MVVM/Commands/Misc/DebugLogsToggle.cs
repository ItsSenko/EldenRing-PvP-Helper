using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class DebugLogsToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        public DebugLogsToggle()
        {
            State = Settings.Default.DebugLogs;
        }

        public override void Execute(object? parameter)
        {
            Settings.Default.DebugLogs = State;
            Settings.Default.Save();
        }
    }
}
