using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Items;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using static Erd_Tools.Models.Weapon;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Items
{
    internal class MassWeaponGib : CommandBase
    {
        private ErdHook hook;
        private ItemCategory category;
        public MassWeaponGib(ErdHook hook, ItemCategory category)
        {
            this.hook = hook;
            this.category = category;
        }
        public override void Execute(object? parameter)
        {
            if (!hook.Loaded || !hook.Hooked)
                return;

            List<ItemSpawnInfo> items = new();
            foreach (Item item in category.Items)
            {
                if (item is Weapon weapon)
                {
                    var gem = weapon.DefaultGem?.ID ?? -1;
                    int roundedLevel = (int)Math.Round((double)ItemsViewModel.MassUpgradeLevel / 2);
                    int upgradeLevel = weapon.Infusible ? ItemsViewModel.MassUpgradeLevel : roundedLevel > 10 ? 10 : roundedLevel;

                    items.Add(new(weapon.ID, weapon.ItemCategory, 1, weapon.MaxQuantity, (int)Infusion.Standard, upgradeLevel, gem, weapon.EventID));
                }
            }

            hook.GetItem(items, CancellationToken.None);
        }
    }
}
