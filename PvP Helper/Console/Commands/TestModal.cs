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
using Architecture = Keystone.Architecture;

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private ErdHook hook;

        private MainWindowViewModel viewModel;
        
        public TestModal(ErdHook hook, MainWindowViewModel viewModel)
        {
            CommandString = "/test";
            RequireParams = false;
            HasParams = true;
            this.hook = hook;
            this.viewModel = viewModel;
            
        }
        private bool state = false;
        protected override void OnTriggerCommand()
        {
            /*state = !state;
            var idOffset = hook.EquipParamWeapon.Fields.FirstOrDefault(x => x.InternalName == "spEffectBehaviorId1").FieldOffset;
            var weaponTypeOffset = hook.EquipParamWeapon.Fields.FirstOrDefault(x => x.InternalName == "wepType").FieldOffset;
            foreach (var row in hook.EquipParamWeapon.Rows)
            {
                if ((WeaponType)row.Param.Pointer.ReadInt16((int)row.DataOffset + weaponTypeOffset) == WeaponType.Dagger)
                {
                    int id = row.Param.Pointer.ReadInt32((int)row.DataOffset + idOffset);
                    row.Param.Pointer.WriteInt32((int)row.DataOffset + idOffset, state ? 70 : -1);
                }
            }
            CommandManager.Log(state.ToString());*/
        }
        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded");
        }

        public IntPtr GetEquipGoodsEntryParamPtr(int id)
        {
            PHPointer pointer = CustomPointers.GetEquipParamGoodsFunc;
            IntPtr entryPtr = hook.GetPrefferedIntPtr(16);
            PHPointer ptr = hook.CreateBasePointer(entryPtr);

            string asmStr = Helpers.GetEmbededResource("Resources.Assembly.GetEquipParamGoodsEntry.asm");
            string formattedStr = string.Format(asmStr, entryPtr, id, pointer.Resolve());

            hook.AsmExecute(formattedStr);

            return ptr.ReadIntPtr(0x8);
        }
    }
}
