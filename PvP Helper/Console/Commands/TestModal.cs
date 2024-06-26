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

        private static List<Infusion> infusions = new List<Infusion>() { Infusion.Blood, Infusion.Cold, Infusion.Fire, Infusion.FlameArt, Infusion.Heavy, Infusion.Keen, Infusion.Lightning, Infusion.Magic, Infusion.Occult, Infusion.Posion, Infusion.Quality, Infusion.Sacred, Infusion.Standard};

        private MainWindowViewModel viewModel;

        private static string resourcesPath => Path.Combine(Directory.GetCurrentDirectory(), "Resources/Items");
        public TestModal(ErdHook hook, MainWindowViewModel viewModel)
        {
            CommandString = "/test";
            //RequireParams = true;
            HasParams = true;
            this.hook = hook;
            this.viewModel = viewModel;
            ArrayStartPtr = hook.CreateChildPointer(hook.GameDataMan, new int[] { 0x8, 0x918 });
            ArrayEndPtr = hook.CreateChildPointer(hook.GameDataMan, new int[] { 0x8, 0x920 });
        }

        protected override void OnTriggerCommand()
        {
            var path = Path.Combine("C:/Users/eleme/Documents/PvP Helper Project/EldenRing-PvP-Helper - DLC Branch/", "msg/engus/");
            CommandManager.Log("Getting Data...");
            GetFilesBytes(path);
            CommandManager.Log("Got all data");
        }

        static void GetFilesBytes(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                try
                {
                    byte[] fileBytes = File.ReadAllBytes(file);
                    foreach (var bnd in BND4.Read(fileBytes).Files)
                    {
                        /*CommandManager.Log($"From file: {Path.GetFileName(file)}");
                        CommandManager.Log($"BND: {bnd.ID} - {bnd.Name}");*/

                        if (bnd.Name.Contains("WeaponName_dlc01"))
                        {
                            var fmg = FMG.Read(bnd.Bytes);
                            CommandManager.Log(bnd.Name);
                            foreach (var entry in fmg.Entries)
                            {
                                if (isInFile("Weapons", $"{entry.ID} {entry.Text}"))
                                    continue;
                                if (entry.ID == 0)
                                    continue;
                                if (string.IsNullOrEmpty(entry.Text) || entry.Text.Contains("[ERROR]"))
                                    continue;
                                if (hasInfusion(entry.Text))
                                    continue;

                                CommandManager.Log($"{entry.ID} - {entry.Text}", false);
                            }
                            
                        }
                        /*var fmg = FMG.Read(bnd.Bytes);
                        foreach(var entry in fmg.Entries)
                        {
                            CommandManager.Log($"FMG: {entry.ID} - {entry.Text}");
                        }*/

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
                GetFilesBytes(subdirectory);
            }
        }

        public static bool isInFile(string path, string text)
        {
            foreach (var file in Directory.GetFiles(Path.Combine(resourcesPath, path)))
            {
                string[] items = File.ReadAllLines(file);
                if (items.FirstOrDefault(x => x.Contains(text)) != null)
                    return true;
                if (items.FirstOrDefault(x => x.StartsWith(text)) != null)
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
            switch(parameters[1])
            {
                case "r":
                    {
                        int.TryParse(parameters[0], out int result);
                        CommandManager.Log(hook.IsEventFlag(result).ToString());
                        break;
                    }
                case "s":
                    {
                        int.TryParse(parameters[0], out int result);
                        bool.TryParse(parameters[2], out bool state);
                        hook.SetEventFlag(result, state);
                        break;
                    }
            }
        }
    }
}
