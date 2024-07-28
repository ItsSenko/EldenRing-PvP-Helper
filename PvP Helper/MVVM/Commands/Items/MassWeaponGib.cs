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
        Dictionary<int, int> SmithyToSomber = new();
        Dictionary<int, int> SomberToSmithy = new();
        public MassWeaponGib(ErdHook hook, ItemCategory category)
        {
            this.hook = hook;
            this.category = category;

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

            SomberToSmithy.Add(0, 0);
            SomberToSmithy.Add(1, 4);
            SomberToSmithy.Add(2, 6);
            SomberToSmithy.Add(3, 9);
            SomberToSmithy.Add(4, 11);
            SomberToSmithy.Add(5, 13);
            SomberToSmithy.Add(6, 16);
            SomberToSmithy.Add(7, 19);
            SomberToSmithy.Add(8, 21);
            SomberToSmithy.Add(9, 24);
            SomberToSmithy.Add(10, 25);
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
                    int somberLevel = SmithyToSomber[ItemsViewModel.MassUpgradeLevel];
                    int upgradeLevel = !weapon.Unique ? (ItemsViewModel.MassUpgradeLevel > weapon.MaxUpgrade ? weapon.MaxUpgrade : ItemsViewModel.MassUpgradeLevel) : 
                        (somberLevel > weapon.MaxUpgrade ? weapon.MaxUpgrade : somberLevel);

                    items.Add(new(weapon.ID, weapon.ItemCategory, 1, weapon.MaxQuantity, (int)Infusion.Standard, upgradeLevel, gem, weapon.EventID));
                }
            }

            hook.GetItem(items, CancellationToken.None);
        }
    }
}
