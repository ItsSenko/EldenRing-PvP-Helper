using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Core;
using System.Collections.Generic;

namespace PvPHelper.Console.Commands
{
    public class SetStatsCommand : CommandBase
    {
        private Player _player;
        private ErdHook _hook;
        public SetStatsCommand(ErdHook hook, Player player)
        {
            Name = "Set Stats Command";
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
                        CommandManager.Log($"Set HP to {newValue}");
                        break; 
                    }
                case "vigor":
                    {
                        if (!Settings.Default.AllowUnsafe)
                        {
                            SendError(parameters[0]);
                            break;
                        }

                        _player.Vigor = value;
                        CommandManager.Log($"Set {parameters[0]} to {value}");
                        break;
                    }
                case "mind":
                    {
                        if (!Settings.Default.AllowUnsafe)
                        {
                            SendError(parameters[0]);
                            break;
                        }

                        _player.Mind = value;
                        CommandManager.Log($"Set {parameters[0]} to {value}");
                        break;
                    }
                case "end":
                    {
                        if (!Settings.Default.AllowUnsafe)
                        {
                            SendError(parameters[0]);
                            break;
                        }

                        _player.Endurance = value;
                        CommandManager.Log($"Set {parameters[0]} to {value}");
                        break;
                    }
                case "str":
                    {
                        if (!Settings.Default.AllowUnsafe)
                        {
                            SendError(parameters[0]);
                            break;
                        }

                        _player.Strength = value;
                        CommandManager.Log($"Set {parameters[0]} to {value}");
                        break;
                    }
                case "dex":
                    {
                        if (!Settings.Default.AllowUnsafe)
                        {
                            SendError(parameters[0]);
                            break;
                        }

                        _player.Dexterity = value;
                        CommandManager.Log($"Set {parameters[0]} to {value}");
                        break;
                    }
                case "int":
                    {
                        if (!Settings.Default.AllowUnsafe)
                        {
                            SendError(parameters[0]);
                            break;
                        }

                        _player.Intelligence = value;
                        CommandManager.Log($"Set {parameters[0]} to {value}");
                        break;
                    }
                case "faith":
                    {
                        if (!Settings.Default.AllowUnsafe)
                        {
                            SendError(parameters[0]);
                            break;
                        }

                        _player.Faith = value;
                        CommandManager.Log($"Set {parameters[0]} to {value}");
                        break;
                    }
                case "arc":
                    {
                        if (!Settings.Default.AllowUnsafe)
                        {
                            SendError(parameters[0]);
                            break;
                        }

                        _player.Arcane = value;
                        CommandManager.Log($"Set {parameters[0]} to {value}");
                        break;
                    }
                default:
                    {
                        throw new InvalidCommandException($"The inputed stat is either invalid or not supported.");
                    }
            }
        }

        private void SendError(string stat)
        {
            CommandManager.Log($"Cannot set '{stat}' because you do not allow unsafe options.");
            CommandManager.Log("To change this, go to misc and toggle 'Allow Unsafe Options`.");
        }
    }
}
