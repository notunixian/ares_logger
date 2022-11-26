using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main.util
{
    public static class console_util
    {
        public static IEnumerable<string> split_cmd(string commandLine)
        {
            bool inQuotes = false;

            return commandLine.split(c =>
            {
                if (c == '\"')
                    inQuotes = !inQuotes;

                return !inQuotes && c == ' ';
            })
                              .Select(arg => arg.Trim().trim_quotes('\"'))
                              .Where(arg => !string.IsNullOrEmpty(arg));
        }

        public static IEnumerable<string> split(this string str,
                                        Func<char, bool> controller)
        {
            int nextPiece = 0;

            for (int c = 0; c < str.Length; c++)
            {
                if (controller(str[c]))
                {
                    yield return str.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return str.Substring(nextPiece);
        }

        public static string trim_quotes(this string input, char quote)
        {
            if ((input.Length >= 2) &&
                (input[0] == quote) && (input[input.Length - 1] == quote))
                return input.Substring(1, input.Length - 2);

            return input;
        }
    }
}
