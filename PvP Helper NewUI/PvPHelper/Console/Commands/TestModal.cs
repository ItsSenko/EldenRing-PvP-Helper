using Erd_Tools;
using PropertyHook;
using PvPHelper.Core;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using System;
using Erd_Tools.Models;
using System.Linq;
using static Erd_Tools.Models.Weapon;
using PvPHelper.MVVM.ViewModels;

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private PHPointer PhantomParamID;
        private ErdHook hook;
        private DispatcherTimer timer;
        public TestModal(ErdHook hook)
        {
            CommandString = "/test";
            //RequireParams = true;
            //HasParams = true;
            this.hook = hook;

            PhantomParamID = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x1E508});

            
        }

        protected override void OnTriggerCommand()
        {
            CommandManager.Log(CustomPointers.ChrDbgFlags.Resolve().ToString("X") + " ChrDbgFlags address");
            CommandManager.Log(CustomPointers.ChrDbgFlags.ReadByte(0x6).ToString());

            /*foreach(var netp in LobbyManagerViewModel.PlayerList)
            {
                if (netp.SteamData != null)
                {
                    CommandManager.Log((netp.SteamData.Resolve().ToInt64() + 0x10).ToString("X"));
                    CommandManager.Log(netp.SteamID.ToString());
                }
            }*/
            /*ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Melee Weapons");

            if (category.Items.FirstOrDefault(x => x.Name == "Black Knife") is Weapon weapon)
            {
                hook.GetItem(new(weapon.ID, weapon.ItemCategory, 1, weapon.MaxQuantity, (int)Infusion.Standard, 25, -1, weapon.EventID));
            }*/
        }
        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            
        }
    }
}
