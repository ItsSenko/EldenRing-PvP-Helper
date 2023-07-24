using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Console
{
    public class CommandManager
    {
        private List<CommandBase> _commands = new List<CommandBase>();
        public void RegisterCommand(CommandBase newCommand)
        {
            _commands.Add(newCommand);
        }

        public void HandleInput(string input)
        {
            foreach(var command in _commands)
            {
                if (input.StartsWith(command.CommandString))
                {
                    string paramString = input.Substring(command.CommandString.Length);
                    
                    if (!string.IsNullOrEmpty(paramString))
                    {
                        var parameters = ParseParameters(paramString);
                        if (command.HasParams)
                        {
                            command.TriggerWithParameters(parameters);
                            return;
                        }
                        throw new InvalidCommandException($"The command '{command.Name}' does not use any parameters.");
                    }

                    if (command.RequireParams)
                        throw new InvalidCommandException($"The command '{command.Name}' requires the parameters: {string.Join(",",command.RequiresParamsString)}.");

                    command.Trigger();
                    return;
                }
            }

            throw new InvalidCommandException($"No command found with the input: {input}");
        }

        private List<string> ParseParameters(string paramString)
        {
            string[] paramTokens = paramString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return paramTokens.ToList();
        }
    }
}
