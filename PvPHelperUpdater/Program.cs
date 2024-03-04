using System.Diagnostics;
using System.IO.Compression;

class Updater
{
    static void Main(string[] args)
    {
        if (args[0] != "pweaseupdate" || !args[1].StartsWith("v"))
            return;

        var basePath = Directory.GetCurrentDirectory();
        var updatePath = Path.Combine(basePath, "update.zip");

        Console.WriteLine("Updating PvP Helper");

        Console.WriteLine("Extracing update.zip...");
        using (var update = ZipFile.OpenRead(updatePath))
        {
            foreach(var entry in update.Entries)
            {
                var fullPath = Path.Combine(basePath, entry.FullName);

                if (entry.FullName != "PvPHelperUpdater.exe")
                {
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }

                    string directoryPath = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    try
                    {
                        entry.ExtractToFile(Path.Combine(basePath, entry.FullName));
                    }
                    catch 
                    {
                        Console.WriteLine($"Failed to extract: {entry.FullName}. Skipping...");
                        continue; 
                    }
                }
            }
        }

        Console.WriteLine("Deleting update.zip...");
        File.Delete(updatePath);

        Console.WriteLine("Restarting PvPHelper.exe...");
        Process.Start(Path.Combine(basePath, "PvPHelper.exe"));
    }
}
