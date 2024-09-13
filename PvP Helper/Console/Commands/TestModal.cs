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
            var driedFinger = hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == 8380012);
            var consumeOffset = driedFinger.Param.Fields[28].FieldOffset;
            CommandManager.Log(driedFinger.Param.Pointer.ReadByte(driedFinger.DataOffset + consumeOffset).ToString());
            CommandManager.Log((CustomPointers.GetEquipParamGoodsFunc.Resolve().ToInt64() - hook.Process.MainModule.BaseAddress.ToInt64()).ToString("X"));
        }

        public enum SeamlessItems
        {
            HostingItem = 8380001,
            JoiningItem = 8380002,
            BreakInItem = 8380003,
            LeavingItem = 8380004,
            GameRuleChangeItem = 8380005,
            RuneArcItem = 8380006,
            RotItem_01 = 8380007,
            RotItem_02 = 8380008,
            RotItem_03 = 8380009,
            RotItem_04 = 8380010,
            RotItem_05 = 8380011,
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded");
        }

        public IntPtr GetEquipGoodsEntryParamPtr(int id)
        {
            var hook = ExtensionsCore.GetMainHook();

            PHPointer pointer = CustomPointers.GetEquipParamGoodsFunc;
            IntPtr entryPtr = hook.GetPrefferedIntPtr(16);
            PHPointer ptr = hook.CreateBasePointer(entryPtr);

            string asmStr = Helpers.GetEmbededResource("Resources.Assembly.GetEquipParamGoodsEntry.asm");
            string formattedStr = string.Format(asmStr, entryPtr, id, pointer.Resolve());

            hook.AsmExecute(formattedStr);
            string s = "";
            foreach(byte b in ptr.ReadBytes(0x0, 16))
            {
                s = s + ", " + b.ToString("X");
            }
            CommandManager.Log(s);
            return ptr.ReadIntPtr(0x8);
        }
    }
}
