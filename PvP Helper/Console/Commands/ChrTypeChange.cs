using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Console.Commands
{
    internal class ChrTypeChange : CommandBase
    {
        private ErdHook Hook { get; set; }
        private Player player { get; set; }
        public ChrTypeChange(ErdHook hook)
        {
            Name = "ChrType Change";
            Description = "Change your ChrType";
            CommandString = "/chrtype";
            RequireParams = true;
            HasParams = true;
            RequiresParamsString = new string[] { "typeInt", "matchBool" };
            Hook = hook;

            player = new(hook.PlayerIns, hook);
        }
        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!Hook.Loaded || !Hook.Hooked)
                return;

            if (parameters.Count < 1 || parameters.Count > 2)
                throw new InvalidCommandException("Invlaid Param");

            if (!Settings.Default.AllowUnsafe)
                throw new InvalidCommandException("Allow Unsafe Options disabled.");

            if (int.TryParse(parameters[0], out int id))
            {
                player.ChrType = (byte)id;

                if (parameters.Count > 1 && bool.TryParse(parameters[1], out bool match))
                    player.ChrType = id;
            }
            else if (parameters[0].ToLower() == "reset")
            {
                player.ChrType = 0;

                if (bool.TryParse(parameters[1], out bool match))
                    player.TeamType = 1;
            }
            else
                throw new InvalidCommandException("Invalid Param");
        }
    }
}
