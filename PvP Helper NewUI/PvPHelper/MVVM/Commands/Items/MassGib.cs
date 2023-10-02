using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Items;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.ViewModels;
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
        private int newMax;
        private ItemsViewModel viewModel;

        public MassGib(ErdHook hook, ItemCategory category, ItemsViewModel viewModel, string[] excludedItems = null, bool single = false, int newMax = 0)
        {
            _hook = hook;
            _category = category;
            if (excludedItems != null)
                exclude = excludedItems;
            this.single = single;

            this.viewModel = viewModel;
            this.newMax = newMax < 1 ? 1 : newMax;
            
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
                    ItemSpawnInfo info = new(item.ID, item.ItemCategory, viewModel.IsOverrideChecked ? newMax : item.MaxQuantity, viewModel.IsOverrideChecked ? newMax : item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID);
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
