using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Console.Commands
{
    internal class SettingsCommand : CommandBase
    {

        public SettingsCommand()
        {
            Name = "Settings Command";
            Description = "Change any of you settings manually";
            CommandString = "/settings";
            HasParams = true;
            RequireParams = true;
            RequiresParamsString = new string[] { "settingName", "value" };
        }

        private string[] settingNames = { "AutoUpdate", "AllowUnsafe", "SpawnAnimation", "DebugLogs", "InvasionPhantomID", "ItemGibSingle", "enableachievements" };

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (parameters[0].ToLower() == "list")
            {
                CommandManager.Log("Listing all Settings");

                foreach(string name in settingNames)
                {
                    CommandManager.Log($"{name}: {Settings.Default.Properties[name].DefaultValue}");
                }
                return;
            }

            string setting = settingNames.FirstOrDefault(x => x.ToLower() == parameters[0]);
            if (string.IsNullOrEmpty(setting))
                throw new InvalidCommandException($"The setting '{parameters[0]}' does not exist.");

            if (parameters.Count < 2)
                throw new InvalidCommandException("Missing parameter: value");


            string value = parameters[1];
            switch (parameters[0].ToLower())
            {
                case "autoupdate":
                    {
                        bool state = value.ToLower() is "true" or "1" ? true : false;

                        Settings.Default.AutoUpdate = state;
                        Settings.Default.Save();

                        CommandManager.Log($"Saved AutoUpdate state to: {state}");
                        break;
                    }
                case "allowunsafe":
                    {
                        bool state = value.ToLower() is "true" or "1" ? true : false;

                        Settings.Default.AllowUnsafe = state;
                        Settings.Default.Save();

                        CommandManager.Log($"Saved AutoUnsafe state to: {state}");
                        break;
                    }
                case "spawnanimation":
                    {
                        if (int.TryParse(value, out int result))
                        {
                            Settings.Default.SpawnAnimation = result;
                            Settings.Default.Save();

                            CommandManager.Log($"Saved SpawnAnimation to: {result}");
                        }
                        else
                            throw new InvalidCastException($"Unable to parse '{value}' to int.");
                        break;
                    }
                case "debuglogs":
                    {
                        bool state = value.ToLower() is "true" or "1" ? true : false;

                        Settings.Default.DebugLogs = state;
                        Settings.Default.Save();

                        CommandManager.Log($"Saved DebugLogs state to: {state}");
                        break;
                    }
                case "invasionphantomid":
                    {
                        if (int.TryParse(value, out int result))
                        {
                            Settings.Default.InvasionPhantomID = result;
                            Settings.Default.Save();

                            CommandManager.Log($"Saved InvasionPhantomID to: {result}");
                        }
                        else
                            throw new InvalidCastException($"Unable to parse '{value}' to int.");
                        break;
                    }
                case "itemgibsingle":
                    {
                        bool state = value.ToLower() is "true" or "1" ? true : false;

                        Settings.Default.ItemGibSingle = state;
                        Settings.Default.Save();

                        CommandManager.Log($"Saved ItemGibSingle state to: {state}");
                        break;
                    }
                case "enableachievements":
                    {
                        bool state = value.ToLower() is "true" or "1" ? true : false;

                        Settings.Default.EnableAchievements = state;
                        Settings.Default.Save();

                        CommandManager.Log($"Saved EnableAchievements state to: {state}");
                        break;
                    }
            }
        }
    }
}
