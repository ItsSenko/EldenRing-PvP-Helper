using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Items;
using PvPHelper.Core.Extensions;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Erd_Tools.Models.Weapon;

namespace PvPHelper.Console.Commands
{
    internal class MassGibConsoleCommand : CommandBase
    {
        private ErdHook hook;
        Dictionary<int, int> SmithyToSomber = new();
        public MassGibConsoleCommand(ErdHook hook)
        {
            CommandString = "/massgib";
            RequireParams = true;
            HasParams = true;
            this.hook = hook;
            RequiresParamsString = new string[] { "categoryName", "amount", "itemLevel"};

            SmithyToSomber.Add(0, 0);
            SmithyToSomber.Add(1, 0);
            SmithyToSomber.Add(2, 1);
            SmithyToSomber.Add(3, 1);
            SmithyToSomber.Add(4, 1);
            SmithyToSomber.Add(5, 2);
            SmithyToSomber.Add(6, 2);
            SmithyToSomber.Add(7, 3);
            SmithyToSomber.Add(8, 3);
            SmithyToSomber.Add(9, 3);
            SmithyToSomber.Add(10, 4);
            SmithyToSomber.Add(11, 4);
            SmithyToSomber.Add(12, 5);
            SmithyToSomber.Add(13, 5);
            SmithyToSomber.Add(14, 5);
            SmithyToSomber.Add(15, 6);
            SmithyToSomber.Add(16, 6);
            SmithyToSomber.Add(17, 7);
            SmithyToSomber.Add(18, 7);
            SmithyToSomber.Add(19, 7);
            SmithyToSomber.Add(20, 8);
            SmithyToSomber.Add(21, 8);
            SmithyToSomber.Add(22, 9);
            SmithyToSomber.Add(23, 9);
            SmithyToSomber.Add(24, 9);
            SmithyToSomber.Add(25, 10);
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not attached or loaded.");

            ItemCategory cat = ItemCategory.All.FirstOrDefault(x => x.Name.RemoveSpaces().ToLower() == parameters[0].ToLower());

            if (cat == null)
                throw new InvalidCommandException($"The category '{parameters[0]}' does not exist.");

            List<ItemSpawnInfo> items = new();
            foreach(var item in cat.Items)
            {
                if (!Blacklist.blacklistedItems.Any(x => x.ItemID == item.ID && x.CatName == cat.Name))
                {
                    if (!int.TryParse(parameters[1], out int amount))
                        throw new InvalidCommandException("Invalid amount level");
                    amount = amount > item.MaxQuantity ? item.MaxQuantity : amount;
                    if ((item as Weapon) != null)
                    {
                        var wep = item as Weapon;
                        var gem = wep.DefaultGem?.ID ?? -1;

                        if (!int.TryParse(parameters[2], out int level))
                            throw new InvalidCommandException("Invalid weapon level");

                        int upgradeLevel = level;

                        if (!wep.Infusible && level > 10)
                        {
                            upgradeLevel = SmithyToSomber[level];
                        }

                        if (upgradeLevel > wep.MaxUpgrade)
                            upgradeLevel = wep.MaxUpgrade;

                        items.Add(new(wep.ID, wep.ItemCategory, amount, wep.MaxQuantity, (int)Infusion.Standard, upgradeLevel, gem, wep.EventID));
                    }
                    else
                        items.Add(new(item.ID, item.ItemCategory, amount, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID));
                }
                else
                    CommandManager.Log("Item was blocked because it was blacklisted.");
            }
            hook.GetItem(items, CancellationToken.None);
            CommandManager.Log($"Gave the player {items.Count} items.");
            CommandManager.Log($"Category: {cat.Name}");
        }
    }
}
