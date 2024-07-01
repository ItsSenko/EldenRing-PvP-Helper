﻿using Erd_Tools.Models;
using PvPHelper.Console;
using PvPHelper.Core.Extensions;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Erd_Tools.Models.Weapon;

namespace PvPHelper.Core
{
    public class Helpers
    {
        public static Dictionary<string, ImageSource> AllImages = new();

        public static void LogFMGItemsInFolder(string folderPath, string fmgFileName)
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
            foreach (var file in Directory.GetFiles(path))
            {
                string[] items = File.ReadAllLines(file);
                if (items.FirstOrDefault(x => x.Contains(text)) != null)
                    return true;
            }
            return false;
        }

        public static bool hasInfusion(string name, List<Infusion> infusions)
        {
            if (name.Contains("Flame Art"))
                return true;
            if (name.Contains("Poison"))
                return true;
            foreach (var inf in infusions)
            {
                if (name.Contains(inf.ToString()))
                    return true;
            }
            return false;
        }

        public static ItemCategory GetCategoryByName(string name)
        {
            return ItemCategory.All.FirstOrDefault(x => x.Name.RemoveSpaces().ToLower() == name.ToLower().RemoveSpaces());
        }
        public static byte SetBit(byte b, int bitIndex, bool value)
        {
            if ((bitIndex < 0) || (bitIndex > 7))
            {
                throw new ArgumentOutOfRangeException("bitIndex", bitIndex, "bitNumber must be 0..7");
            }

            if (value)
                b |= (byte)(1 << bitIndex);
            else
                b &= (byte)~(1 << bitIndex);

            return b;
        }
        public static bool IsBitSet(byte value, int bitNumber)
        {
            if ((bitNumber < 0) || (bitNumber > 7))
            {
                throw new ArgumentOutOfRangeException("bitNumber", bitNumber, "bitNumber must be 0..7");
            }

            return ((value & (1 << bitNumber)) != 0);
        }
        public static string GetEmbededResource(string item)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string resourceName = $"PvPHelper.{item}";

            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new NullReferenceException($"Could not find embedded resource: {item} in the {Assembly.GetCallingAssembly().GetName()} assembly");

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        public static void SaveEmbededFileToPath(string file, string pathToSave)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string resourceName = $"PvPHelper.{file}";

            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new NullReferenceException($"Could not find embedded resource: {file} in the {Assembly.GetCallingAssembly().GetName()} assembly");

                using FileStream fs = new(pathToSave, FileMode.Create);
                stream.CopyTo(fs);
                fs.Close();
                stream.Close();
            }
        }

        public static IEnumerable<string> EnumerateLines(TextReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
        private static bool imagesLoaded = false;
        public static void LoadAllImages()
        {
            if (imagesLoaded)
                return;
            string[] resourceNames = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Icons"));

            foreach (string name in resourceNames)
            {
                if (name.EndsWith(".png") || name.EndsWith(".jpg"))
                {
                    AllImages.Add(name, LoadImageFromPath(name));
                }
            }
            imagesLoaded = true;
        }
        public static string GetNewPhantomName(int id)
        {
            switch (id)
            {
                case -1:
                    {
                        return $"{id} - Default";
                    }
                case 51:
                    {
                        return $"{id} - White Coop";
                    }
                case 60:
                    {
                        return $"{id} - Duelist";
                    }
                case 61:
                    {
                        return $"{id} - Yellow Coop";
                    }
                case 1:
                    {
                        return $"{id} - Big White Glow";
                    }
                case 230:
                    {
                        return $"{id} - White Ghost, White Eyes";
                    }
                case 901:
                    {
                        return $"{id} - White Ghost Normal";
                    }
                case 902:
                    {
                        return $"{id} - Blood Stain Ghost";
                    }
                case 810:
                    {
                        return $"{id} - Invisible Cloak";
                    }
                case 211:
                    {
                        return $"{id} - Black Phantom";
                    }
                case 270:
                    {
                        return $"{id} - Piss Ghost";
                    }
                case 290:
                    {
                        return $"{id} - VOID";
                    }
                case 811:
                    {
                        return $"{id} - Flashbang";
                    }
                case 70:
                    {
                        return $"{id} - Hunter";
                    }
                default:
                    {
                        return $"{id} -";
                    }
            }
        }
        public static ImageSource LoadImage(string searchStr, string path = "")
        {
            var iconsDirectory = string.IsNullOrEmpty(path) ? Path.Combine(Directory.GetCurrentDirectory(), "Icons") : Path.Combine(Directory.GetCurrentDirectory(), "Resources/Images");

            if (Directory.Exists(iconsDirectory))
            {
                foreach(var file in Directory.GetFiles(iconsDirectory))
                {
                    string fileName = Path.GetFileName(file);
                    string str = fileName.Substring(0, (fileName.Length - 4) );
                    if (str.ToLower() == searchStr.ToLower())
                    {
                        var image = LoadImageFromPath(file);
                        if (AllImages.Keys.Contains(Path.GetFileName(file)))
                            return image;
                        AllImages.Add(Path.GetFileName(file), image);
                        return image;
                    }
                }
            }
            return null;
        }

        public static string GetFullIconID(short id)
        {
            string str = id.ToString().PadLeft(5, '0');
            return str;
        }
        public static ImageSource GetImageSource(string searchStr, bool exact = false)
        {
            if (searchStr.Contains("'"))
            {
                searchStr = searchStr.Replace('\'', '_');
            }
            ImageSource image = AllImages.FirstOrDefault(x => Path.GetFileName(x.Key.ToLower()).Contains(searchStr.ToLower() + ".png")).Value;
            if (searchStr == "0")
            {
                if (AllImages.FirstOrDefault(x => x.Key == "null.png").Value == null)
                {
                    image = LoadImage("null.png");
                }
                else
                    image = AllImages.FirstOrDefault(x => x.Key == "null.png").Value;
            }

            if (image == null)
            {
                image = LoadImage(searchStr);

                if (image == null)
                {
                    if (AllImages.FirstOrDefault(x => x.Key == "null.png").Value == null)
                    {
                        image = LoadImage("null.png");
                    }
                    else
                        image = AllImages.FirstOrDefault(x => x.Key == "null.png").Value;
                }
            }

            return image;
        }
        public static ImageSource LoadImageFromResource(string resourcePath)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    return image;
                }
                else
                {
                    return null;
                }
            }
        }

        public static void RenameFilesInDirectory(string directory)
        {
            // Check if the provided directory exists
            if (!Directory.Exists(directory))
            {
                CommandManager.Log($"The directory {directory} does not exist.");
                return;
            }

            // Iterate over each file in the directory
            foreach (string filePath in Directory.GetFiles(directory))
            {
                // Extract the file name
                string fileName = Path.GetFileName(filePath);
                fileName = fileName.Substring(0, fileName.Length - 4);

                // Use regular expression to find the last 5-digit number
                Match match = Regex.Match(fileName, @"\d{5}$");

                if (match.Success && fileName.Length > 5)
                {
                    // New file name is the matched 5-digit number
                    string newFileName = match.Value + Path.GetExtension(filePath);
                    string newFilePath = Path.Combine(directory, newFileName);

                    // Rename the file
                    if (!File.Exists(newFilePath))
                        File.Move(filePath, newFilePath);
                    else
                        File.Delete(filePath);
                }
            }

            CommandManager.Log("Successfully renamed all files.");
        }
        public static ImageSource LoadImageFromPath(string path)
        {
            BitmapImage img = new();
            img.BeginInit();
            img.UriSource = new(path, UriKind.RelativeOrAbsolute);
            img.EndInit();

            return img;
        }
        public static string GetPathToPic(string searchStr, string folder, bool replace, bool png)
        {
            try
            {
                string[] pictureFiles = Directory.GetFiles(folder, png ? "*.png" : "*.jpg"); // You can adjust the search pattern as needed

                foreach (string pictureFile in pictureFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(pictureFile);


                    string itemName = searchStr;
                    if (searchStr.Contains("'") && replace)
                    {
                        itemName = searchStr.Replace('\'', '_');
                    }

                    if (fileName.StartsWith(itemName, StringComparison.OrdinalIgnoreCase))
                    {
                        return Path.GetFullPath(pictureFile);
                    }
                }
            }
            catch (Exception ex)
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Resources/Images/null.png");
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Resources/Images/null.png");
        }

        public static bool IsValidFileName(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                if (fileName.Contains(invalidChar))
                {
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            return true;
        }

        public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    return typedChild;
                }

                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }

            return null;
        }

        public static bool InventoryContainsItem(List<InventoryEntry> entries, string item)
        {
            foreach(InventoryEntry entry in entries)
            {
                if (entry.Name.Contains(item))
                    return true;
            }
            return false;
        }

        public static Item GetItemFromID(ItemCategory category, int ID)
        {
            return category.Items.FirstOrDefault(x => x.ID == ID);
        }
        public static Item GetItemFromID(ItemCategory[] categories, int ID)
        {
            foreach (ItemCategory category in categories)
            {
                if (category.Items.FirstOrDefault(x => x.ID == ID) != null)
                    return category.Items.FirstOrDefault(x => x.ID == ID);
            }
            return null;
        }

        public static int GetProperID(int ID)
        {
            if (!ID.ToString().EndsWith("0000") && ID != 0)
            {
                string intStr = ID.ToString().Substring(0, ID.ToString().Length - 4) + "0000";
                int.TryParse(intStr, out ID);
                return ID;
            }
            return ID;
        }
    }
}
