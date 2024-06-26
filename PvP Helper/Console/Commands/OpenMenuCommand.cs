using Erd_Tools;
using PvPHelper.Core;
using PvPHelper.MVVM.Models;
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
        private List<MenuItem> Menus = new();
        public OpenMenuCommand(ErdHook hook)
        {
            Hook = hook;

            Name = "Open Menu Command";
            Description = "Open a specified menu in game";
            CommandString = "/menu";
            HasParams = true;
            RequireParams = true;
            RequiresParamsString = new string[] { "menuName/address", "startId", "endId" };

            Hook.OnSetup += Hook_OnSetup;
        }

        private void Hook_OnSetup(object? sender, PropertyHook.PHEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                Menus.Add(new("Memorize Spells", 0x80f600));
                Menus.Add(new("Sort Chest", 0x8104c0));
                Menus.Add(new("Level", 0x810160));
                Menus.Add(new("Ashes of War", 0x80eee0));
                Menus.Add(new("Flask Allocation", 0x80e080));
                Menus.Add(new("Wondrous Physick Mix", 0x80de30));
                Menus.Add(new("Great Runes", 0x80d940));
                Menus.Add(new("Rebirth", 0x810bc0));
                Menus.Add(new("Cosmetics (Mirror)", 0x80dcb0));
                Menus.Add(new("Spirit Tuning", 0x80daf0));
                Menus.Add(new("Blacksmith", 0x80ebb0));
                Menus.Add(new("Smithing Table", 0x80f4f0));
                Menus.Add(new("Sell Item", 0x80edd0));
            });
        }

        private IntPtr GetAddress(int offset)
        {
            return Hook.Process.MainModule.BaseAddress + offset;
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (parameters.Count < 1 || parameters.Count > 3)
                throw new InvalidCommandException("Invalid parameter count");

            if (!Hook.Setup || !Hook.Loaded)
                throw new InvalidCommandException("Not attached or loaded into a character");

            if (!Settings.Default.AllowUnsafe)
                throw new InvalidCommandException("Unsafe not enabled.");

            foreach (var menu in Menus)
            {
                if (menu.Name.ToLower().StartsWith(parameters[0]))
                {
                    OpenMenu(menu);
                    return;
                }
            }

            if (parameters.Count > 2)
            {
                
                int.TryParse(parameters[1], out int startId);
                int.TryParse(parameters[2], out int endId);
                OpenShop(parameters[0], startId, endId);
            }
            else
            {
                if (parameters[0].ToLower() == "shop")
                {
                    OpenShop("0x80e770", 0, 9999999);
                    return;
                }
            }
        }

        private void OpenMenu(MenuItem menu)
        {
            string asmStr = Helpers.GetEmbededResource("Resources.Assembly.OpenMenu.asm");
            string asm = string.Format(asmStr, menu.Offset.ToString("X"));
            Hook.AsmExecute(asm);
            CommandManager.Log($"Opened {menu.Name} menu.");
        }

        private void OpenShop(string address, int startId, int endId)
        {
            string asmStr = Helpers.GetEmbededResource("Resources.Assembly.OpenShopMenu.asm");
            string asm = string.Format(asmStr, startId.ToString(), endId.ToString(), address);
            Hook.AsmExecute(asm);
            CommandManager.Log($"Opened menu at offset: {address}");
            CommandManager.Log($"StartID: {startId}");
            CommandManager.Log($"EndID: {endId}");
        }
    }
}
