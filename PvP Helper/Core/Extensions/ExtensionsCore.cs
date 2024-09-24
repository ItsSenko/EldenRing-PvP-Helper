using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Game;
using PropertyHook;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PvPHelper.Core.Extensions
{
    public static class ExtensionsCore
    {
        public static void Initialize()
        {
            AddSoul_Call = GetMainHook().RegisterAbsoluteAOB(AddSoul_CallAoB);
            KickOutFunction = GetMainHook().RegisterRelativeAOB(KickOutFunctionAOB, 1, 5);
            GetQuantityFunc = GetMainHook().RegisterAbsoluteAOB(GetQuantityAOB);
            
        }
        public static ErdHook GetMainHook() => MainWindowViewModel.GetMainHook();
        public const string AddSoul_CallAoB = "44 8B 49 ? 45 33 DB 44 89 5C 24";
        public static PHPointer AddSoul_Call = null;

        private static string KickOutFunctionAOB = "e8 ?? ?? ?? ?? 41 ff c6 48 81 c3 00 01 00 00 48 3b df 0f 85 39 ff ff ff";
        public static PHPointer KickOutFunction = null;

        private const string GetQuantityAOB = "?? 8b f9 ?? 8d 44 ?? 48 ?? 89 44 ?? ?? 8b 01";

        private static PHPointer GetQuantityFunc;

        public static string RemoveSpaces(this string str)
        {
            return str.Replace(" ", string.Empty);
        }

        public static Task DownloadAsync(this BitmapImage bitmapImage)
        {
            var tcs = new TaskCompletionSource<bool>();
            EventHandler downloadCompleted = null;
            downloadCompleted = (s, e) =>
            {
                bitmapImage.DownloadCompleted -= downloadCompleted;
                tcs.SetResult(true);
            };

            bitmapImage.DownloadCompleted += downloadCompleted;

            if (bitmapImage.IsDownloading)
            {
                return tcs.Task;
            }
            else
            {
                // The image is already downloaded
                tcs.SetResult(true);
                return tcs.Task;
            }
        }

        public static int GetQuantity(this Item item)
        {
            ErdHook hook = GetMainHook();

            if (!hook.Hooked || !hook.Setup)
                return 0;

            if (item.ItemCategory == Item.Category.Protector || item.ItemCategory == Item.Category.Accessory)
                return 0;

            List<InventoryEntry> invEntries = (List<InventoryEntry>)hook.PlayerGameData.Inventory.GetNormalInventory();
            invEntries.AddRange(hook.PlayerGameData.Inventory.GetKeyInventory());
            List<InventoryEntry> storEntries = (List<InventoryEntry>)hook.PlayerGameData.Storage.GetNormalInventory();
            storEntries.AddRange(hook.PlayerGameData.Storage.GetKeyInventory());

            int quantity = 0;

            InventoryEntry invEntry = invEntries.FirstOrDefault(x => x.RawItemId == (int)item.ItemCategory + item.ID);
            InventoryEntry storEntry = storEntries.FirstOrDefault(x => x.RawItemId == (int)item.ItemCategory + item.ID);

            if (invEntry != null)
                quantity += invEntry.Quantity;

            if (storEntry != null)
                quantity += storEntry.Quantity;

            return quantity;
        }

        public static string BytesToString(this byte[] bytes)
        {
            string str = string.Empty;
            foreach (byte b in bytes)
                str = str + b.ToString("X") + ", ";

            return str;
        }

        public static bool IsSomber(this Weapon weapon)
        {
            return weapon.MaxUpgrade == 10;
        }

        public static int GetMaxStorageAmount(this Item item, ErdHook hook)
        {
            if (item is Weapon weapon)
            {
                if ((int)weapon.Type is 81 or 83 or 85 or 86)
                    return item.MaxQuantity + 600;
                return 1;
            }


            var maxRepositoryNumOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "maxRepositoryNum").FieldOffset;
            var potGroupIdOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "potGroupId").FieldOffset;

            var row = hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == item.ID);

            if (row == null)
                return item.MaxQuantity;

            int maxInStorage = row.Param.Pointer.ReadInt16((int)row.DataOffset + maxRepositoryNumOffset);
            int potGroupId = row.Param.Pointer.ReadInt16((int)row.DataOffset + potGroupIdOffset);

            if (potGroupId < 255)
                return maxInStorage;

            return maxInStorage + item.MaxQuantity;
        }

        public static bool HasItem(this Item item, ErdHook hook)
        {
            List<InventoryEntry>? storageEntries = (List<InventoryEntry>)hook.PlayerGameData.Inventory.NormalItems.GetInventoryList();
            List<InventoryEntry>? inventoryEntries = (List<InventoryEntry>)hook.PlayerGameData.Inventory.KeyItems.GetInventoryList();

            var invEntry = inventoryEntries.FirstOrDefault(x => (x.Name == item.Name) || (x.ItemID == item.ID));
            var storEntry = storageEntries.FirstOrDefault(x => (x.Name == item.Name) || (x.ItemID == item.ID));

            return invEntry != null || storEntry != null;
        }
    }
}
