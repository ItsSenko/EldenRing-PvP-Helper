using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PvPHelper.Console
{
    public class ProgramLogs
    {
        private static string dir => Path.Combine(Directory.GetCurrentDirectory(), "Resources/Logs");
        private static string ErrorDir => Path.Combine(dir, "Errors");
        private static string LogsDir => Path.Combine(dir, "Logs");

        private static string CurrErrorDir => Path.Combine(ErrorDir, DateTime.Now.ToString("dd-MM-yyyy"));
        private static string CurrLogsDir => Path.Combine(LogsDir, DateTime.Now.ToString("dd-MM-yyyy"));

        private static string CurrLogPath;
        private static string CurrErrorPath;

        public static void Initialize()
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!Directory.Exists(ErrorDir))
            {
                Directory.CreateDirectory(ErrorDir);
            }
            if (!Directory.Exists(LogsDir))
            {
                Directory.CreateDirectory(LogsDir);
            }

            if (!Directory.Exists(CurrErrorDir))
            {
                Directory.CreateDirectory(CurrErrorDir);
            }
            if (!Directory.Exists(CurrLogsDir))
            {
                Directory.CreateDirectory(CurrLogsDir);
            }

            string time = DateTime.Now.ToString("hh_mm_ss");

            CurrLogPath = Path.Combine(CurrLogsDir, $"Log {time}.txt");
            CurrErrorPath = Path.Combine(CurrErrorDir, $"Error {time}.txt");

            Program.OnLogged += OnNewLog;
            Program.OnUnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(Exception e)
        {
            using (var sw = new StreamWriter(CurrErrorPath, true))
            {
                sw.WriteLine($"{e.Message} | {e.Source} | {e.TargetSite}" +
                    $"{e.StackTrace}");
            }
        }

        private static void OnNewLog(string message)
        {
            using (var sw = new StreamWriter(CurrLogPath, true))
            {
                sw.WriteLine(message);
            }

            if (message.StartsWith("Unhandled Exception:"))
            {
                using (var sw = new StreamWriter(CurrErrorPath, true))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}
