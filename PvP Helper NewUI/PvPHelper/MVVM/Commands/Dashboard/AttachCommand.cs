using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Entities;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.Core;
using PvPHelper.MVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard
{
    internal class AttachCommand : CommandBase
    {
        private ErdHook _hook;

        public AttachCommand(ErdHook hook)
        {
            _hook = hook;
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Hooked)
            {
                _hook.Start();
            }
        }
    }
}
