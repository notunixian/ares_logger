using ares_logger.main.config;
using Assembly_CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ares_logger.main.util
{
    internal class downloader
    {
        public static string gen_mac()
        {
            var random = new Random(Environment.TickCount);
            byte[] bytes = new byte[20];
            random.NextBytes(bytes);
            string HWID = string.Join("", bytes.Select(it => it.ToString("x2")));
            return HWID;
        }

        public static void download(string url)
        {
            if (conf.handler.Config.enable_unsafe_features == false) return;

            // as close as to 1:1 i can get, this should be 100% safe but i'm still going to put this behind unsafe.
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var handler = new HttpClientHandler
            {
                UseCookies = false
            };
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Host = "api.vrchat.cloud";
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "deflate, gzip");
            client.DefaultRequestHeaders.Add("User-Agent", "VRC.Core.BestHTTP");
            client.DefaultRequestHeaders.Add("X-Client-Version", Application.version);
            client.DefaultRequestHeaders.Add("X-MacAddress", gen_mac());
            client.DefaultRequestHeaders.Add("X-Platform", "standalonewindows");
            client.DefaultRequestHeaders.Add("X-UnityVersion", Application.unityVersion);
            client.DefaultRequestHeaders.Add("Cookie", $"auth={VRCPlayer.Instance.player.api_user.authToken}; twoFactorAuth=");
            

            try
            {
                string downloadPath = $"{core.ares_dir}\\downloads";
                if (!Directory.Exists(downloadPath))
                {
                    Directory.CreateDirectory(downloadPath);
                }
                HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();
                string path = downloadPath + "\\" + Guid.NewGuid().ToString() + ".vrca";
                using FileStream fileStream = new FileStream(path, FileMode.CreateNew);
                response.Content.CopyToAsync(fileStream).GetAwaiter().GetResult();
                log_sys.log("[download]: successfully downloaded avatar, saved to downloads folder at " + path, ConsoleColor.Green);
            }
            catch (HttpRequestException ex)
            {
                log_sys.log($"[download failure]: unable to download, e: {ex.Message} | in: {ex.InnerException.Message}", ConsoleColor.Red);
            }
        }
    }
}
