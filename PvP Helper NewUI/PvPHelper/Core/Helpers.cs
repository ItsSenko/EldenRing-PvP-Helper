using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PvPHelper.Core
{
    internal class Helpers
    {
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
        public static ImageSource GetImageSource(string searchStr, string folder, bool replace, bool png)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();
            string itemName = searchStr;
            if (searchStr.Contains("'") && replace)
            {
                itemName = searchStr.Replace('\'', '_');
            }
            foreach (string resourceName in resourceNames)
            {
                bool startsWith = resourceName.StartsWith(folder);
                bool endsWith = resourceName.EndsWith(png ? ".png" : ".jpg");
                if (resourceName.StartsWith(folder) && resourceName.EndsWith(png?".png":".jpg"))
                {
                    string fileName = resourceName.Substring(folder.Length + 1); // +1 to remove the dot
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                    if (fileNameWithoutExtension.StartsWith(itemName, StringComparison.OrdinalIgnoreCase))
                    {
                        return LoadImageFromResource(resourceName);
                    }
                }
            }
            return LoadImageFromResource("PvPHelper.Resources.Images.null.png");
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
    }
}
