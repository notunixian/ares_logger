using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main
{
    public class core
    {
        public static bool ares_debug = false;
        public static void init()
        {
            Console.WriteLine("[init] ares logger begin");
            if (Environment.CommandLine.Contains("--ares-debug"))
            {
                Console.WriteLine("[init] debug mode enabled");
                ares_debug = true;
            }

            patches.on_event.init_patch();
            patches.network_mgr.init_patch();
        }
    }
}
