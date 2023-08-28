using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private VersionController _vController;
        public TestModal(VersionController versionController)
        {
            CommandString = "/test";
            _vController = versionController;
        }
        protected override void OnTriggerCommand()
        {
            _vController.ApplyUpdate();
        }
    }
}
