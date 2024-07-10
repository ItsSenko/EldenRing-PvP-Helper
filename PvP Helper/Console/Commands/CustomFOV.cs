using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Erd_Tools;
using Erd_Tools.Models;
using static Erd_Tools.Models.Param;

namespace PvPHelper.Console.Commands
{
    internal class CustomFOV : CommandBase
    {
        private ErdHook Hook;
        private Param LockCamParam;
        
        public CustomFOV(ErdHook hook)
        {
            Hook = hook;
            Hook.OnSetup += Hook_OnSetup;
            Name = "FOV Command";
            Description = "Set your FOV to a specefic value";
            CommandString = "/fov";
            HasParams = true;
            RequireParams = true;
            RequiresParamsString = new string[] { "number" };
        }

        private void Hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() => { LockCamParam = Hook.Params.FirstOrDefault(x => x.Name == "LockCamParam"); });
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (parameters.Count < RequiresParamsString.Length || parameters.Count > RequiresParamsString.Length)
                throw new InvalidCommandException($"Parameter Count Invalid. This command requires {RequiresParamsString.Length} parameters.");

            var offset = LockCamParam.Fields.FirstOrDefault(x => x.InternalName == "camFovY").FieldOffset;

            if (!int.TryParse(parameters[0], out var newfov))
            {
                if (parameters[0].ToLower() == "reset")
                {
                    LockCamParam.RestoreParam();
                    return;
                }
                else
                    throw new InvalidCommandException("Invalid Parameter.");
            }

            if (newfov > 156 || newfov < 5)
                throw new InvalidCommandException("The value you set was either too high or low and would cause the camera to make to game unplayable.");

            foreach (Row row in LockCamParam.Rows)
            {
                LockCamParam.Pointer.WriteSingle(row.DataOffset + offset, newfov);
            }
        }
    }
}
