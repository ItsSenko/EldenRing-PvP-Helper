using System;
using System.Collections.Generic;
using System.Linq;

namespace PvPHelper.Console
{
    public class CommandManager
    {
        private List<CommandBase> _commands = new List<CommandBase>();
        private static CommandManager instance;
        public CommandManager()
        {
            instance = this;
            Program.OnUnhandledException += OnError;
        }

        private void OnError(Exception args)
        {
            Log($"Unhandled Exception: {args.Message}");
            Log($"Stack Trace: {args.StackTrace}");
        }

        public void RegisterCommand(CommandBase newCommand)
        {
            _commands.Add(newCommand);
        }

        public List<CommandBase> GetAllRegisteredCommands() {return _commands; }

        public static event Action LogLoaded;

        
        private static Action<string> logAction;
        public static CommandManager RegisterConsole(Action<string> action)
        {
            logAction = action;
            //LogLoaded.Invoke();
            return instance;
        }
        public static void Log(string text, bool withTime = true)
        {
            string SystemTime = DateTime.Now.ToString("[" + "hh:mm:ss" + "]");

            string final = withTime ? $"{SystemTime} {text}" : text;
            logAction?.Invoke(final);
            Program.InvokeLog(final);
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
