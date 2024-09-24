using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Core;
using PvPHelper.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Console.Commands
{
    public class SpecialEffectsCommand : CommandBase
    {
        ErdHook hook;
        Player player;

        public SpecialEffectsCommand(ErdHook hook)
        {
            Name = "Special Effects Command";
            Description = "Add or Remove special effects";
            CommandString = "/sfx";
            RequireParams = true;
            HasParams = true;
            RequiresParamsString = new string[] { "add/del", "id" };

            this.hook = hook;
            player = new(hook.PlayerIns, hook);
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (parameters.Count < 1)
                throw new InvalidCommandException("Missing Parameters.");

            if (!hook.Hooked || !hook.Setup)
                throw new InvalidCommandException("Not hooked or setup.");

            switch (parameters[0].ToLower())
            {
                case "list":
                    {
                        CommandManager.Log("Active Effects");

                        foreach(int sfx in player.GetAllSpecialEffects())
                        {
                            CommandManager.Log("ID: " + sfx.ToString());
                        }

                        break;
                    }
                case "add":
                    {
                        if (!int.TryParse(parameters[1], out int ID))
                            throw new InvalidCommandException($"'{parameters[1]}' is not a proper ID.");

                        player.AddSpecialEffect(ID);
                        CommandManager.Log("Added special effect at ID: " + parameters[1]);

                        break;
                    }
                case "del":
                    {
                        if (!int.TryParse(parameters[1], out int ID))
                            throw new InvalidCommandException($"'{parameters[1]}' is not a proper ID.");

                        player.RemoveSpecialEffect(ID);
                        CommandManager.Log("Removed special effect at ID: " + parameters[1]);

                        break;
                    }
                default:
                    {
                        throw new InvalidCommandException("Invalid command. You can only do add or del.");
                    }
            }
        }
    }
}
