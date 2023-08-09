using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Entities;
using Erd_Tools.Models.Items;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.Core;
using PvPHelper.MVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using static Erd_Tools.Models.Weapon;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Items
{
    internal class MassGib : CommandBase
    {
        private string[] exclude = new string[] { };
        private ItemCategory _category;
        private ErdHook _hook;

        public MassGib(ErdHook hook, ItemCategory category, string[] excludedItems = null)
        {
            _hook = hook;
            _category = category;
            if (excludedItems != null)
                exclude = excludedItems;
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
                    ItemSpawnInfo info = new(item.ID, item.ItemCategory, item.MaxQuantity, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID);
                    items.Add(info);
                }
            }

            _hook.GetItem(items, CancellationToken.None);
        }
    }
}
