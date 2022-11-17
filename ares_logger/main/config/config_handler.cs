using ares_logger.main.util;
using ares_logger.util;
using System;
using System.Diagnostics;
using System.IO;

namespace ares_logger.main.config
{
    internal class config_handler
    {
        static string path = $"{core.ares_dir}\\config.json";
        static bool init = false;
        static config config;
        public static void init_config()
        {
            log_sys.debug_log("init config...");
            var timer = new Stopwatch();
            timer.Start();

            if (File.Exists($"{core.ares_dir}\\config.json"))
            {
                log_sys.debug_log("deserialize from existing config");
                config = json<config>.deserialize(File.ReadAllText(path));
            }
            else
            {
                log_sys.debug_log("creating config");
                File.Create($"{core.ares_dir}\\config.json");
                var conf = new config
                {
                    log_avatars = true,
                    ignore_friends = false
                };
                var text = json<config>.serialize(conf);
                File.WriteAllText(path, text);
            }

            log_sys.log($"[config]: successfully finished config init in {timer.Elapsed.ToString(@"m\:ss\.fff")}");
            init = true;
        }

        public static config get_config()
        {
            if (init == false || config == null) init_config();
            return config;
        }
    }
}
