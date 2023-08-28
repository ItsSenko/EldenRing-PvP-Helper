using Erd_Tools;
using Erd_Tools.Models;
using PvPHelper.MVVM.Dialogs;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Drawing;
using System.Linq;
using static Erd_Tools.Models.Param;
using static Erd_Tools.Models.Weapon;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.PrefabCreator
{
    internal class LoadPrefab : CommandBase
    {
        private ErdHook hook;
        private PrefabCreatorViewModel prefabCreator;

        public LoadPrefab(ErdHook hook, PrefabCreatorViewModel prefabCreator)
        {
            this.hook = hook;
            this.prefabCreator = prefabCreator;
        }

        public override void Execute(object? parameter)
        {
            if (!hook.Loaded || !hook.Hooked)
                return;

            if (prefabCreator.weaponPrefabs.Count > 0)
            {
                foreach (WeaponPrefab wpn in prefabCreator.weaponPrefabs)
                {
                    ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Items.FirstOrDefault(x => x.ID == wpn.ID && x is Weapon) != null);
                    if (category != null)
                    {
                        Item item = category.Items.FirstOrDefault(x => x.ID == wpn.ID);
                        hook.GetItem(new(item.ID, item.ItemCategory, 1, item.MaxQuantity, wpn.Infusion, wpn.UpgradeLevel, wpn.SwordArtID, item.EventID));
                    }
                }
            }

            if (prefabCreator.armorPrefabs.Count > 0)
            {
                foreach (ArmorPrefab armor in prefabCreator.armorPrefabs)
                {
                    ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Armor");
                    if (category != null)
                    {
                        Item item = category.Items.FirstOrDefault(x => x.ID == armor.ID);
                        hook.GetItem(new(item.ID, item.ItemCategory, 1, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID));
                    }
                }
            }

            if (prefabCreator.talismanPrefabs.Count > 0)
            {
                foreach (TalismanPrefab talisman in prefabCreator.talismanPrefabs)
                {
                    ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");
                    if (category != null)
                    {
                        Item item = category.Items.FirstOrDefault(x => x.ID == talisman.ID);
                        hook.GetItem(new(item.ID, item.ItemCategory, 1, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID));
                    }
                }
            }
        }
    }
}
