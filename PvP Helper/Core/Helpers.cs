using Erd_Tools.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PvPHelper.Core
{
    public class Helpers
    {
        public static Dictionary<string, ImageSource> AllImages = new();
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
        public static void LoadAllImages()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();

            string itempicfolder = "PvPHelper.Resources.Images.Items";
            string infusionpicfolder = "PvPHelper.Resources.Images.Infusions";

            foreach (string name in resourceNames)
            {
                if (name.EndsWith(".png") || name.EndsWith(".jpg"))
                {
                    if (name.StartsWith(itempicfolder))
                    {
                        string fileName = name.Substring(itempicfolder.Length + 1);
                        AllImages.Add(fileName, LoadImageFromResource(name));
                        continue;
                    }

                    if (name.StartsWith(infusionpicfolder))
                    {
                        string fileName = name.Substring(infusionpicfolder.Length + 1);
                        AllImages.Add(fileName, LoadImageFromResource(name));
                        continue;
                    }
                    AllImages.Add(name, LoadImageFromResource(name));
                }
            }
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
                foreach(var fileName in Directory.GetFiles(iconsDirectory))
                {
                    if (Path.GetFileName(fileName).ToLower().StartsWith(searchStr.ToLower()))
                    {
                        var image = LoadImageFromPath(fileName);
                        AllImages.Add(Path.GetFileName(fileName), image);
                        return image;
                    }
                }
            }
            return null;
        }
        public static ImageSource GetImageSource(string searchStr)
        {
            if (searchStr.Contains("'"))
            {
                searchStr = searchStr.Replace('\'', '_');
            }
            ImageSource image = AllImages.FirstOrDefault(x => x.Key.StartsWith(searchStr, StringComparison.OrdinalIgnoreCase)).Value;

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
