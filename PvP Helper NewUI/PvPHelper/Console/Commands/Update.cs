﻿using PvPHelper.Core;
using System.Threading.Tasks;

namespace PvPHelper.Console.Commands
{
    class Update : CommandBase
    {
        private VersionController _vController;
        public Update(VersionController controller)
        {
            Name = "Update Command";
            Description = "Will update your version to the latest.";
            CommandString = "/update";

            _vController = controller;
        }
        protected override void OnTriggerCommand()
        {
            _vController.Update(_vController._releaseUrl, _vController.CurrentVersion);
        }
    }
}
