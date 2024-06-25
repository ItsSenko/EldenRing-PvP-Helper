using System.Collections.Generic;

namespace PvPHelper.Console
{
    public abstract class CommandBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CommandString { get; set; }
        public bool RequireParams { get; set; }
        public bool HasParams { get; set; }
        public string[] RequiresParamsString { get; set; }

        protected virtual void OnTriggerCommand() { }
        protected virtual void OnTriggerCommandWithParameters(List<string> parameters) { }

        public void Trigger()
        {
            OnTriggerCommand();
        }
        public void TriggerWithParameters(List<string> parameters)
        {
            OnTriggerCommandWithParameters(parameters);
        }
    }
}
