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

namespace PvPHelper.Console.Commands
{
    internal class TestModal : CommandBase
    {
        private ErdHook hook;
        private DispatcherTimer timer;
        private PHPointer ArrayStartPtr;
        private PHPointer ArrayEndPtr;

        private static List<Infusion> infusions = new List<Infusion>() { Infusion.Blood, Infusion.Cold, Infusion.Fire, Infusion.FlameArt, Infusion.Heavy, Infusion.Keen, Infusion.Lightning, Infusion.Magic, Infusion.Occult, Infusion.Poison, Infusion.Quality, Infusion.Sacred, Infusion.Standard};

        private MainWindowViewModel viewModel;

        private static string resourcesPath => Path.Combine(Directory.GetCurrentDirectory(), "Resources/Items");
        public TestModal(ErdHook hook, MainWindowViewModel viewModel)
        {
            CommandString = "/test";
            RequireParams = false;
            HasParams = true;
            this.hook = hook;
            this.viewModel = viewModel;
            ArrayStartPtr = hook.CreateChildPointer(hook.GameDataMan, new int[] { 0x8, 0x918 });
            ArrayEndPtr = hook.CreateChildPointer(hook.GameDataMan, new int[] { 0x8, 0x920 });
        }

        protected override void OnTriggerCommand()
        {
            ItemCategory cat = ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Gems");
            foreach(var item in ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Gems").Items)
            {
                if (item.ID == 411000)
                {
                    CommandManager.Log(item.Name);
                    foreach(var type in (item as Gem).WeaponTypes)
                    {
                        CommandManager.Log(type.ToString());
                    }
                    
                }
                if (item.ID == 412000)
                {
                    CommandManager.Log(item.Name);
                    foreach (var type in (item as Gem).WeaponTypes)
                    {
                        CommandManager.Log(type.ToString());
                    }
                }
                
            }
            foreach (var item in ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Melee Weapons").Items)
            {
                if (item.ID == 64500000)
                {
                    CommandManager.Log(item.Name);
                    CommandManager.Log((item as Weapon).Type.ToString());

                }
                if (item.ID == 68500000)
                {
                    CommandManager.Log(item.Name);
                    CommandManager.Log((item as Weapon).Type.ToString());
                }
            }
            foreach (var item in ItemCategory.All.FirstOrDefault(x => x.Name == "DLC Shields").Items)
            {
                if (item.ID == 62500000)
                {
                    CommandManager.Log(item.Name);
                    CommandManager.Log((item as Weapon).Type.ToString());

                }
            }
        }

        static void LogFMGItemsInFolder(string folderPath, string fmgFileName)
        {
            //Get all files from path
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                try
                {
                    //Trun file into bytes and then use BND4.Read() to read them
                    byte[] fileBytes = File.ReadAllBytes(file);
                    foreach (var bnd in BND4.Read(fileBytes).Files)
                    {
                        if (bnd.Name.Contains(fmgFileName))
                        {
                            //Get FMG
                            var fmg = FMG.Read(bnd.Bytes);

                            //Log Current bnd
                            CommandManager.Log(bnd.Name);
                            foreach (var entry in fmg.Entries)
                            {
                                //This can be removed, I use this to check if we already have the item somewhere
                                if (isInFile("DLC", $"{entry.ID} {entry.Text}"))
                                    continue;
                                if (entry.ID == 0)
                                    continue;
                                if (string.IsNullOrEmpty(entry.Text) || entry.Text.Contains("[ERROR]"))
                                    continue;
                                //Log the item
                                CommandManager.Log($"{entry.ID} {entry.Text}", false);
                            }
                            
                        }

                    }
                }
                catch (Exception ex)
                {
                    CommandManager.Log($"Unhandled Exception: {ex.Message} {ex.StackTrace}");
                }
            }

            // Recursively get files from subdirectories
            string[] subdirectories = Directory.GetDirectories(folderPath);
            foreach (string subdirectory in subdirectories)
            {
                LogFMGItemsInFolder(subdirectory, fmgFileName);
            }
        }

        public static bool isInFile(string path, string text)
        {
            foreach (var file in Directory.GetFiles(Path.Combine(resourcesPath, path)))
            {
                string[] items = File.ReadAllLines(file);
                if (items.FirstOrDefault(x => x.Contains(text)) != null)
                    return true;
            }
            return false;
        }

        public static bool hasInfusion(string name)
        {
            if (name.Contains("Flame Art"))
                return true;
            if (name.Contains("Poison"))
                return true;
            foreach(var inf in infusions)
            {
                if (name.Contains(inf.ToString()))
                    return true;
            }
            return false;
        }
        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            /*var path = Path.Combine("C:/Users/eleme/Documents/PvP Helper Project/", "msg/engus/");
            CommandManager.Log("Getting Data...");
            LogFMGItemsInFolder(path, parameters[0]);
            CommandManager.Log("Got all data");*/
        }
    }
}
