using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERPvPHelper
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
            string resourceName = $"ERPvPHelper.{item}";

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
            string resourceName = $"ERPvPHelper.{file}";

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
    }
}
