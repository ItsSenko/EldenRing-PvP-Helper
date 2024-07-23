using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Items;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static Erd_Tools.Models.Weapon;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Items
{
    internal class MassGib : CommandBase
    {
        private string[] exclude = new string[] { };
        private ItemCategory _category;
        private ErdHook _hook;
        private bool single;
        private int Amount { get; set; }

        public MassGib(ErdHook hook, ItemCategory category, int amount, string[] excludedItems = null, bool single = false)
        {
            _hook = hook;
            _category = category;
            if (excludedItems != null)
                exclude = excludedItems;
            this.single = single;
            Amount = amount;
            
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Loaded || !_hook.Hooked)
                return;

            List<ItemSpawnInfo> items = new();
            foreach (Item item in _category.Items)
            {
                if (!Blacklist.blacklistedItems.Any(x => x.ItemID == item.ID && x.CatName == _category.Name))
                {
                    if (exclude != null && exclude.Any(x => x == item.Name))
                        continue;

                    if (!single)
                    {
                        if (Amount > item.MaxQuantity)
                        {
                            int stacks = (int)MathF.Floor(Amount / item.MaxQuantity);
                            int remainder = Amount % item.MaxQuantity;

                            for (int i = 0; i < stacks; i++)
                            {
                                ItemSpawnInfo info2 = new(item.ID, item.ItemCategory, item.MaxQuantity, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID);
                                items.Add(info2);
                            }

                            if (remainder > 0)
                            {
                                ItemSpawnInfo info2 = new(item.ID, item.ItemCategory, remainder, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID);
                                items.Add(info2);
                            }

                            continue;
                        }
                    }

                    ItemSpawnInfo info = new(item.ID, item.ItemCategory, Amount, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID);
                    if (single)
                        _hook.GetItem(info);
                    else
                        items.Add(info);
                }
            }

            if (!single)
                _hook.GetItem(items, CancellationToken.None);
        }
    }
}
