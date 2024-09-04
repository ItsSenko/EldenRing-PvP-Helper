using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Items;
using PvPHelper.Core;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using PvPHelper.Core.Extensions;
using static Erd_Tools.Models.Weapon;
using CommandBase = PvPHelper.Core.CommandBase;
using System.Threading;

namespace PvPHelper.MVVM.Commands.Items
{
    internal class MassGib : CommandBase
    {
        private ItemCategory _category;
        private ErdHook _hook;
        private bool IsMax { get; set; }
        private bool IsWeapon { get; set; }
        private ItemsViewModel ViewModel { get; set; }
        private int UpgradeLevel => ViewModel.MassGibUpgLevel;

        public MassGib(ErdHook hook, ItemCategory category, ItemsViewModel viewModel, bool max = true, bool isWeapon = false)
        {
            _hook = hook;
            _category = category;
            IsMax = max;
            IsWeapon = isWeapon;
            ViewModel = viewModel;
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
                    if (item is Weapon weapon && IsWeapon)
                    {
                        GiveItem(item);
                        continue;
                    }

                    int totalMax = GetMaxStorageAmount(item);
                    int totalHas = totalMax - item.GetQuantity();
                    int amount = IsMax ? (totalHas < 0 ? 0 : totalHas) : 1;

                    if (amount > 0)
                        GiveItem(item, amount);
                }
            }

            if (queue.Count > 0)
                _hook.GetItem(queue, CancellationToken.None);
            queue.Clear();
        }

        List<ItemSpawnInfo> queue = new();
        private void GiveItem(Item item, int amount = 1)
        {
            if (IsWeapon)
            {
                if (item is Weapon weapon)
                {
                    int level = weapon.IsSomber() ? Helpers.GetSomberLevel(UpgradeLevel) : UpgradeLevel;

                    ItemSpawnInfo info = new(item.ID, item.ItemCategory, 1, item.MaxQuantity, (int)Infusion.Standard, level);

                    if (Settings.Default.ItemGibSingle)
                        _hook.GetItem(info);
                    else
                        queue.Add(info);
                }
                return;
            }

            int stacks = (int)MathF.Floor(amount / item.MaxQuantity);
            int remainder = amount % item.MaxQuantity;

            List<ItemSpawnInfo> items = new();

            items.Add(new(item.ID, item.ItemCategory, amount, item.MaxQuantity, (int)Infusion.Standard, 0));

            if (items.Count == 1)
            {
                if (Settings.Default.ItemGibSingle)
                    _hook.GetItem(items[0]);
                else
                    queue.AddRange(items);
                return;
            }

            if (items.Count <= 0)
                return;
            if (Settings.Default.ItemGibSingle)
                _hook.GetItem(items, CancellationToken.None);
            else
                queue.AddRange(items);
        }

        public int GetMaxStorageAmount(Item item)
        {
            if (item is Weapon weapon)
            {
                if ((int)weapon.Type is 81 or 83 or 85 or 86)
                    return item.MaxQuantity + 600;
                return 1;
            }

            
            var maxRepositoryNumOffset = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "maxRepositoryNum").FieldOffset;
            var potGroupIdOffset = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "potGroupId").FieldOffset;

            var row = _hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == item.ID);

            if (row == null)
                return item.MaxQuantity;

            int maxInStorage = row.Param.Pointer.ReadInt16((int)row.DataOffset + maxRepositoryNumOffset);
            int potGroupId = row.Param.Pointer.ReadInt16((int)row.DataOffset + potGroupIdOffset);

            if (potGroupId < 255)
                return maxInStorage;

            return maxInStorage + item.MaxQuantity;
        }

        public bool HasItem(Item item)
        {
            List<InventoryEntry>? storageEntries = (List<InventoryEntry>)_hook.GetStorage();
            List<InventoryEntry>? inventoryEntries = (List<InventoryEntry>)_hook.GetInventory();

            var invEntry = inventoryEntries.FirstOrDefault(x => (x.Name == item.Name) || (x.ItemID == item.ID));
            var storEntry = storageEntries.FirstOrDefault(x => (x.Name == item.Name) || (x.ItemID == item.ID));

            return invEntry != null || storEntry != null;
        }

        public Param GetParam(Item.Category category)
        {
            switch (category)
            {
                case Item.Category.Weapons:
                    {
                        return _hook.EquipParamWeapon;
                    }
                case Item.Category.Goods:
                    {
                        return _hook.EquipParamGoods;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
