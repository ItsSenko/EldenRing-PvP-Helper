using System.Collections.Generic;
using Erd_Tools;
using PvPHelper.Core;

namespace PvPHelper.Console.Commands
{
    internal class CustomFPS : CommandBase
    {
        private ErdHook Hook;
        
        public CustomFPS(ErdHook hook)
        {
            Hook = hook;

            Name = "FPS Command";
            Description = "Set your FPS to a specefic value";
            CommandString = "/fps";
            HasParams = true;
            RequireParams = true;
            RequiresParamsString = new string[] { "number" };
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (parameters.Count < RequiresParamsString.Length || parameters.Count > RequiresParamsString.Length)
                throw new InvalidCommandException($"Parameter Count Invalid. This command requires {RequiresParamsString.Length} parameters.");

            if (!int.TryParse(parameters[0], out var newfps))
            {
                if (parameters[0].ToLower() == "reset")
                {
                    CustomPointers.CSFlipper.WriteSingle(0x2CC, 60);
                    CustomPointers.CSFlipper.WriteByte(0x2D0, 00);
                    return;
                }
                else
                    throw new InvalidCommandException("Invalid Parameter.");
            }

            if (newfps > 999 || newfps < 1)
                throw new InvalidCommandException("What? Why?");

            CustomPointers.CSFlipper.WriteSingle(0x2CC, newfps);
            CustomPointers.CSFlipper.WriteByte(0x2D0, 01);
        }
    }
}
