using Erd_Tools;
using Erd_Tools.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Console.Commands
{
    public class SetStatsCommand : CommandBase
    {
        private Player _player;
        private ErdHook _hook;
        public SetStatsCommand(ErdHook hook, Player player)
        {
            Name = "Set Stats";
            Description = "Set supported stat to value.";
            CommandString = "/set";
            HasParams = true;
            RequireParams = true;
            _player = player;
            _hook = hook;
            RequiresParamsString = new string[] { "supportedStat", "value" };
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!_hook.Loaded || !_hook.Hooked)
                throw new InvalidCommandException($"Currently not hooked to Elden Ring.");

            if (parameters.Count < 2 || parameters.Count > 2)
                throw new InvalidCommandException($"Inproper Parameter Count. This command requires {RequiresParamsString.Length} parameters.");

            if (!IsSupportedStat(parameters[0]))
                throw new InvalidCommandException($"The inputed stat is either invalid or not supported.");

            if (!int.TryParse(parameters[1], out int value))
                throw new InvalidCommandException($"Invalid Value. Try again.");

            switch(parameters[0].ToLower())
            {
                case "fp": 
                    {
                        var newValue = value >= _player.FpMax ? _player.FpMax : value;
                        _player.Fp = newValue;
                        CommandManager.Log($"Set FP to {newValue}.");
                        break; 
                    }
                case "hp": 
                    {
                        var newValue = value >= _player.HpMax ? _player.HpMax : value;
                        _player.Hp = newValue;
                        CommandManager.Log($"Set FP to {newValue}");
                        break; 
                    }
            }
        }

        private bool IsSupportedStat(string input)
        {
            return input.ToLower() == "fp" || input.ToLower() == "hp";
        }
    }
}
