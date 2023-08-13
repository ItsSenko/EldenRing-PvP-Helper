using System.Collections.Generic;
using Erd_Tools;
using PvPHelper.Core;

namespace PvPHelper.Console.Commands
{
    internal class HitboxCommand : CommandBase
    {
        private ErdHook Hook;
        public bool State = false;
        public HitboxCommand(ErdHook hook)
        {
            Hook = hook;

            Name = "HitboxCommand";
            Description = "Show and Hide hitboxes";
            CommandString = "/hitbox";
            HasParams = true;
            RequiresParamsString = new string[] { "value" };
        }
        protected override void OnTriggerCommand()
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            State = !State;
            CustomPointers.dHitbox.WriteByte(0xA1, State ? (byte)1 : (byte)0);
            CommandManager.Log($"Hitboxes {(State ? "Shown":"Hidden")}");
        }
        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (parameters.Count < RequiresParamsString.Length || parameters.Count > RequiresParamsString.Length)
                throw new InvalidCommandException($"Parameter Count Invalid. This command requires {RequiresParamsString.Length} parameters.");

            switch(parameters[0])
            {
                case "show":
                    {
                        CustomPointers.dHitbox.WriteByte(0xA1, 1);
                        State = true;
                        break;
                    }
                case "hide":
                    {
                        CustomPointers.dHitbox.WriteByte(0xA1, 0);
                        State = false;
                        break;
                    }
                case "s":
                    {
                        CustomPointers.dHitbox.WriteByte(0xA1, 1);
                        State = true;
                        break;
                    }
                case "h":
                    {
                        CustomPointers.dHitbox.WriteByte(0xA1, 0);
                        State = false;
                        break;
                    }
                default: throw new InvalidCommandException("The parameter you input is invalid");
            }
            CommandManager.Log($"Hitboxes {(State ? "Shown" : "Hidden")}");
        }
    }
}
