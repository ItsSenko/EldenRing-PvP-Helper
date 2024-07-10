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
            CustomPointers.idleAnimation.WriteInt32(0x18, 63020);
        }

        
        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Setup || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded");
            switch (parameters[0].ToLower())
            {
                case "init":
                    {
                        var cat = Helpers.GetCategoryByName(parameters[1]);
                        if (cat == null)
                            throw new InvalidCommandException("Category does not exist");

                        List<SearchItem<Item>> searchItems = new();
                        foreach(var item in cat.Items)
                        {
                            searchItems.Add(new(item, item.Name));
                        }
                        algorithm = new SearchAlgorithm<Item>(searchItems, new AlphabeticalSort<Item>());
                        
                        isInitialized = true;
                        CommandManager.Log("Initialized");
                        break;
                    }
                case "setorder":
                    {
                        if (!isInitialized)
                            throw new InvalidCommandException("Not initialized");

                        if (parameters[1] == "alpha")
                            (algorithm as SearchAlgorithm<Item>).Order = new AlphabeticalSort<Item>();
                        else if (parameters[1] == "close")
                            (algorithm as SearchAlgorithm<Item>).Order = new ClosestMatchSort<Item>();

                        CommandManager.Log("Changed Order");
                        break;
                    }
                case "setsearch":
                    {
                        if (!isInitialized)
                            throw new InvalidCommandException("Not initialized");

                        (algorithm as SearchAlgorithm<Item>).SearchString = parameters[1];
                        CommandManager.Log("Set Search String");
                        break;
                    }
                case "show":
                    {
                        if (!isInitialized)
                            throw new InvalidCommandException("Not initialized");

                        CommandManager.Log("Shown Items: ");
                        foreach(var item in (algorithm as SearchAlgorithm<Item>).ShownItems)
                        {
                            CommandManager.Log(item.Item.Name);
                        }
                        break;
                    }
            }
        }
    }
}
