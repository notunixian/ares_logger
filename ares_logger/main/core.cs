using ares_logger.main.config;
using ares_logger.main.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main
{
    public class core
    {
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleTitle(string lpConsoleTitle);

        public static bool ares_debug = false;
        public static string ares_dir;
        public static void init()
        {
            Console.Title = "ARES Logger by Unixian";
            log_sys.log($"[init]: successfully started ARES Logger", ConsoleColor.Green);
            log_sys.log($"[init]: made with love by unixian", ConsoleColor.Green);
            log_sys.log($"[init]: join the ARES discord server @ discord.gg/vrc-ares", ConsoleColor.Green);

            if (!Directory.Exists($"{Environment.CurrentDirectory}\\ares_logger"))
            {
                ares_dir = $"{Environment.CurrentDirectory}\\ares_logger";
                Directory.CreateDirectory(ares_dir);
                log_sys.log($"[init]: created ares_logger directory at {ares_dir}", ConsoleColor.Blue);
            }
            else { ares_dir = $"{Environment.CurrentDirectory}\\ares_logger"; }

            if (Environment.CommandLine.Contains("--ares-debug"))
            {
                log_sys.log($"[init]: debug mode enabled", ConsoleColor.Green);
                ares_debug = true;
            }

            config_handler.init_config();
            patches.on_event.init_patch();
            patches.network_mgr.init_patch();
        }
    }
}
