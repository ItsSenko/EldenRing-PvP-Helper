using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erd_Tools;
using PvPHelper.Core;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class CustomInvasionSpawn : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        public CustomInvasionSpawn()
        {
            State = Settings.Default.CustomInvasionSpawn;
        }

        public override void Execute(object? parameter)
        {
            Settings.Default.CustomInvasionSpawn = State;
            Settings.Default.Save();
        }
    }
}