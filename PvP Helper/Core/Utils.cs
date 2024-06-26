using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Core
{
    public static class Utils
    {
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
    }
}
