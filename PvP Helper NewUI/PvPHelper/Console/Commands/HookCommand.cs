using Erd_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Console.Commands
{
    internal class HookCommand : CommandBase
    {
        private ErdHook _hook { get; set; }
        public HookCommand(ErdHook hook)
        {
            Name = "Hook Command";
            Description = "Begins or Ends the hook.";
            CommandString = "/hook";
            HasParams = true;
            RequireParams = true;
            _hook = hook;
            RequiresParamsString = new string[]{"True = t", "False = f"};
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (parameters[0].ToLower() == "t")
                _hook.Start();
            else if (parameters[0].ToLower() == "f")
                _hook.Stop();
            else
                throw new InvalidCommandException("Invalid Parameter");
        }
    }
}
