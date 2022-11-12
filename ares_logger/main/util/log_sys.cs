using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ares_logger.main.util
{
    internal class log_sys
    {
        // credits: stackoverflow
        public static void log(string message, ConsoleColor color)
        {
            var pieces = Regex.Split(message, @"(\[[^\]]*\])");
            Console.Write($"[{DateTime.Now.ToString("hh:mm:ss")}] ");

            for (int i = 0; i < pieces.Length; i++)
            {
                string piece = pieces[i];

                if (piece.StartsWith("[") && piece.EndsWith("]"))
                {
                    Console.ForegroundColor = color;
                    piece = piece.Substring(1, piece.Length - 2);
                }

                Console.Write(piece);
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        public static void log(string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss")}] {message}");
        }

        public static void debug_log(string message)
        {
            if (!core.ares_debug) return;

            if (core.ares_debug)
            {
                log($"[debug log]: {message}", ConsoleColor.Cyan);
            }
        }
    }
}
