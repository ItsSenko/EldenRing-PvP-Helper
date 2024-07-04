using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Items;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.Models.Builds;
using PvPHelper.MVVM.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static Erd_Tools.Models.Weapon;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.PrefabCreator
{
    internal class LoadPrefab : CommandBase
    {
        private ErdHook hook;
        private PrefabCreatorViewModel vm;

        public LoadPrefab(ErdHook hook, PrefabCreatorViewModel vm)
        {
            this.hook = hook;
            this.vm = vm;
        }

        public override void Execute(object? parameter)
        {
            /*if (!hook.Loaded || !hook.Hooked)
                return;

            List<ItemSpawnInfo> info = new();
            foreach(var inventory in vm.CurrentBuild.Inventories)
            {
                foreach(var item in inventory.Items)
                {
                    if (item is WeaponItem weaponItem)
                    {
                        Weapon weapon = GetWeaponFromID(weaponItem.ID);
                        if (weapon == null)
                            throw new System.Exception($"Couldnt find weapon at ID: {weaponItem.ID}");
                        info.Add(new(weapon.ID, weapon.ItemCategory, 1, weapon.MaxQuantity, weaponItem.Infusion, weaponItem.UpgradeLevel, weaponItem.SwordArtID, weapon.EventID));
                    }
                    else
                    {
                        Item item1 = GetItemFromID(item.ID, item.Category);

                        if (item1 == null)
                            throw new System.Exception($"Couldnt find item at ID: {item.ID} With name: {item.Name}");

                        info.Add(new(item1.ID, item1.ItemCategory, 1, item1.MaxQuantity, (int)Infusion.Standard, 0, -1, item1.EventID));
                    }
                }
            }
            hook.GetItem(info, CancellationToken.None);*/
        }

        
    }
}
