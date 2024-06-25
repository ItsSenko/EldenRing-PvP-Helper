using Erd_Tools;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PvPHelper.Console.Commands
{
    internal class OpenMenuCommand : CommandBase
    {
        private ErdHook Hook;
        private Dictionary<string, IntPtr> Menus = new();
        public OpenMenuCommand(ErdHook hook)
        {
            Hook = hook;

            Name = "Open Menu Command";
            Description = "Open a specified menu in game";
            CommandString = "/menu";
            HasParams = true;
            RequireParams = true;
            RequiresParamsString = new string[] { "menuName" };

            Hook.OnSetup += Hook_OnSetup;
        }

        private void Hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                Menus.Add("respec", GetAddress(0x7FF180));
                Menus.Add("level", GetAddress(0x7FE720));
                Menus.Add("cosmetic", GetAddress(0x7FC270));
                Menus.Add("shop", GetAddress(0x7FCD30));
            });
        }

        private IntPtr GetAddress(int offset)
        {
            return Hook.Process.MainModule.BaseAddress + offset;
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (parameters.Count < 1 || parameters.Count > 2)
                throw new InvalidCommandException("Invalid parameter count");

            if (!Hook.Setup || !Hook.Loaded)
                throw new InvalidCommandException("Not attached or loaded into a character");

            if (!Menus.ContainsKey(parameters[0]))
                throw new InvalidCommandException("Invalid Menu");

            OpenMenu(parameters[0], Menus[parameters[0]]);
        }

        private void OpenMenu(string name, IntPtr address)
        {
            string asmStr = Helpers.GetEmbededResource(name == "shop" ? "Resources.Assembly.OpenShopMenu.asm" : "Resources.Assembly.OpenMenu.asm");
            string asm = string.Format(asmStr, address.ToString("X"));
            Hook.AsmExecute(asm);
            CommandManager.Log($"Opened {name} menu.");
        }
    }
}
