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
            try
            {
                log_sys.debug_log("init config...");
                var timer = new Stopwatch();
                timer.Start();

                if (File.Exists(path))
                {
                    if (File.ReadAllText(path) == string.Empty) create_config();
                    log_sys.debug_log("deserialize from existing config");
                    config = json<config>.deserialize(File.ReadAllText(path));

                    timer.Stop();
                    log_sys.log($"[config]: successfully read from existing config in {timer.Elapsed.ToString(@"m\:ss\.fff")}", ConsoleColor.Green);
                }
                else
                {
                    create_config();

                    timer.Stop();
                    log_sys.log($"[config]: successfully created a new config in {timer.Elapsed.ToString(@"m\:ss\.fff")}", ConsoleColor.Green);
                }
                
                init = true;
            }
            catch (Exception e)
            {
                log_sys.log($"[config failure]: unknown exception in config init, e: {e.Message}", ConsoleColor.Red);
            }
        }

        public static config get_config()
        {
            if (init == false || config == null) init_config();
            return config;
        }

        public static void create_config()
        {
            log_sys.debug_log("creating config");
            var stream = File.Create(path);
            stream.Close();
            var conf = new config
            {
                log_avatars = true,
                ignore_friends = false,
                log_worlds = true,
            };
            var text = json<config>.serialize(conf);
            File.WriteAllText(path, text);
            conf = config;
        }
    }
}
