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

        protected override void OnTriggerCommand()
        {
            CommandManager.Log(CustomPointers.GetEquipParamGoodsFunc.Resolve().ToString("X"));
            CommandManager.Log(Helpers.GetEquipGoodsEntryParamPtr(910).ToString("X"));
        }
        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded");
        }
    }
}
