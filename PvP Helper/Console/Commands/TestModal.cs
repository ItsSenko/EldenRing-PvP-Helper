using Erd_Tools;
using PropertyHook;
using PvPHelper.Core;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using System;
using Erd_Tools.Models;
using System.Linq;
using static Erd_Tools.Models.Weapon;
using PvPHelper.MVVM.ViewModels;
using PvPHelper.MVVM.Models.Regions;
using System.Runtime.InteropServices;
using PvPHelper.MVVM.Views;
using Erd_Tools.Models.Entities;
using PvPHelper.Core.Extensions;
using SoulsFormats;
using System.IO;
using PvPHelper.MVVM.Models.Search;
using PvPHelper.MVVM.Models.Search.SortOrders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PvPHelper.MVVM.Models.Builds;
using Keystone;
using System.Text.RegularExpressions;
using Kernel32 = PropertyHook.Kernel32;

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private ErdHook hook;

        private MainWindowViewModel viewModel;

        private ISearchAlgorithm algorithm;

        private bool isInitialized = false;

        private const string GetQuantityAOB = "?? 8b f9 ?? 8d 44 ?? 48 ?? 89 44 ?? ?? 8b 01";

        private PHPointer GetQuantityFunc;
        public TestModal(ErdHook hook, MainWindowViewModel viewModel)
        {
            CommandString = "/test";
            RequireParams = false;
            HasParams = true;
            this.hook = hook;
            this.viewModel = viewModel;
            GetQuantityFunc = hook.RegisterAbsoluteAOB(GetQuantityAOB);
        }

        protected override void OnTriggerCommand()
        {
            ItemCategory cat = ItemCategory.All.FirstOrDefault(x => x.Name == "Consumables");
            Item item = cat.Items.FirstOrDefault(x => x.ID == 390);

            var maxRepositoryNumOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "maxRepositoryNum").FieldOffset;
            var potGroupIdOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "potGroupId").FieldOffset;

            var row = hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == item.ID);

            int maxInStorage = row.Param.Pointer.ReadInt16((int)row.DataOffset + maxRepositoryNumOffset);
            byte potGroupId = row.Param.Pointer.ReadByte((int)row.DataOffset + potGroupIdOffset);

            CommandManager.Log(potGroupId.ToString());
        }

        public int GetMaxStorageAmount(Item item)
        {
            var fieldOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "maxRepositoryNum").FieldOffset;
            var row = hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == item.ID);

            return row.Param.Pointer.ReadInt16((int)row.DataOffset + fieldOffset);
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded");
        }

        public int GetQuantity(Item item)
        {
            int rawItemID = item.ID + (int)item.ItemCategory;

            string asm = Helpers.GetEmbededResource("Resources.Assembly.GetQuantity.asm");

            IntPtr idPtr = hook.GetPrefferedIntPtr(sizeof(int));
            Kernel32.WriteBytes(hook.Handle, idPtr, BitConverter.GetBytes(rawItemID));

            IntPtr returnPtr = hook.GetPrefferedIntPtr(sizeof(int));

            string format = string.Format(asm, idPtr, GetQuantityFunc.Resolve() - 0x14, returnPtr);

            hook.AsmExecute(format);

            return Kernel32.ReadInt32(hook.Handle, returnPtr);
        }

    }
}
