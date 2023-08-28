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
        public TestModal()
        {
            CommandString = "/test";
        }
        protected override void OnTriggerCommand()
        {
            Helpers.GetImageSource("fire", "PvPHelper.Resources.Images.Items", true, true);
        }
    }
}
