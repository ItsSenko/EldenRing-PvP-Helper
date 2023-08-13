using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var task = Task.Run(async () => await _vController.Update());
            task.GetAwaiter().GetResult();
        }
    }
}
