﻿using Erd_Tools;
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

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private ErdHook hook;

        private MainWindowViewModel viewModel;

        private ISearchAlgorithm algorithm;

        private bool isInitialized = false;
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
            foreach(InventoryEntry entry in hook.GetInventory())
            {
                CommandManager.Log($"Name: {entry.Name}");
                CommandManager.Log($"ID: {entry.ItemID}");
                CommandManager.Log($"RawID: {entry.RawItemId}");
                CommandManager.Log($"DisplayID: {entry.DisplayID}");
            }

            /*int biggestGemAmount = 0;
            string biggestItemName = string.Empty;
            foreach(ItemCategory cat in ItemCategory.All)
            {
                foreach(Item item in cat.Items)
                {
                    if (item is Weapon weapon)
                    {
                        if (weapon.Infusible)
                        {
                            int gemCount = Gem.All.Where(x => x.WeaponTypes.Contains(weapon.Type)).Count();
                            if (gemCount > biggestGemAmount)
                            {
                                biggestGemAmount = gemCount;
                                biggestItemName = weapon.Name;
                            }
                        }
                    }
                }
            }

            CommandManager.Log("Item with the biggest amount of gems.");
            CommandManager.Log($"Name: {biggestItemName}");
            CommandManager.Log($"Amount: {biggestGemAmount}");*/
        }
        
        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded");
        }

        
    }
}
