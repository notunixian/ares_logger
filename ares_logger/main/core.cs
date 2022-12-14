using ares_logger.main.config;
using ares_logger.main.util;
using Assembly_CSharp.VRC.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ares_logger.main
{
    public class core
    {
        public static bool ares_debug = false;
        public static string ares_dir;
        public static void init()
        {
            Console.Title = "ARES Logger by Unixian | ver 1.01";
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

            conf.handler = new(ares_dir + "\\ares_config.json");
            patches.on_event.init_patch();
            patches.network_mgr.init_patch();
            patches.download_mgr.init_patch();
            Thread cmd_thread = new Thread(() => { while (true) { handle_cmd(); } });

            try { cmd_thread.Start(); }
            catch (Exception e) { log_sys.log($"[console error]: unable to start command handler thread, commands will not work. e: {e.Message} | in: {e.InnerException.Message}"); }

            }


        public static string[] args = new string[0];
        public static void handle_cmd()
        {
            string c_read = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(c_read)) return;
            try
            {
                args = console_util.split_cmd(c_read).ToArray();
            }
            catch { return; }

            switch (args[0])
            {
                case "clear":
                    {
                        Console.Clear();
                        log_sys.log("[console]: successfully cleared console", ConsoleColor.Green);
                        break;
                    }
                case "download":
                    {
                        if (args.Length > 1)
                        {
                            if (conf.handler.Config.enable_unsafe_features == false)
                            {
                                log_sys.log("[console error]: you do not have unsafe features enabled!", ConsoleColor.Red);
                                break;
                            }

                            if (args[1].StartsWith("http") && args[1].Contains("api.vrchat.cloud/api/1/file/")) downloader.download(args[1]);
                            else log_sys.log("[console error]: you did not provide a valid url.", ConsoleColor.Red);
                        }
                        else 
                        {
                            var text = clipboard.GetText();
                            log_sys.debug_log(text);
                            if (text.StartsWith("http") && text.Contains("api.vrchat.cloud"))
                            {
                                downloader.download(text);
                                break;
                            }
                            log_sys.debug_log("displaying info because clipboard did not contain valid a valid asset url.");
                            log_sys.log("[console info]: download ([url]) OR ([input from clipboard]) | downloads an avatar based off it's asset url, requires the enable_unsafe_feature flag enabled in your config.", ConsoleColor.Blue); 
                        }
                        break;
                    }
                default: return;
            }
            args = new string[0];
        }
    }

}
