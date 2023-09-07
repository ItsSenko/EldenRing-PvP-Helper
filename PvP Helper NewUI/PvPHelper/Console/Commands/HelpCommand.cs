using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Console.Commands
{
    public class HelpCommand : CommandBase
    {
        private CommandManager Manager;
        public HelpCommand(CommandManager manager)
        {
            Name = "Help";
            Description = "Will show all commands";
            CommandString = "/help";
            RequireParams = false;
            HasParams = false;
            
            Manager = manager;
        }

        protected override void OnTriggerCommand()
        {
            List<CommandBase> commands = Manager.GetAllRegisteredCommands();

            CommandManager.Log("All Commands");
            foreach (CommandBase command in commands)
            {
                CommandManager.Log(command.CommandString);
            }
            CommandManager.Log("For more information, click the question mark!");
        }
    }
}
