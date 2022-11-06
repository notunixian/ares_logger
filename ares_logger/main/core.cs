using ares_logger.main.util;
using System;
using System.Collections.Generic;
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
        public static void init()
        {
            SetConsoleTitle("ARES Logger by Unixian");
            log_sys.log($"[init]: successfully started ARES Logger", ConsoleColor.Green);
            log_sys.log($"[init]: made with love by unixian", ConsoleColor.Green);
            log_sys.log($"[init]: join the ARES discord server @ discord.gg/vrc-ares", ConsoleColor.Green);

            if (Environment.CommandLine.Contains("--ares-debug"))
            {
                log_sys.log($"[init]: debug mode enabled", ConsoleColor.Green);
                ares_debug = true;
            }

            patches.on_event.init_patch();
            patches.network_mgr.init_patch();

            log_sys.log($"[init]: patches completed successfully", ConsoleColor.Green);
        }
    }
}
