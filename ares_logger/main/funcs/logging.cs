using ares_logger.main.util;
using ares_logger.util;
using Assembly_CSharp;
using Assembly_CSharp.VRC.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main.funcs
{
    internal class logging
    {
        public static void execute_log(VRCPlayer player, bool aviChange = false)
        {
            if (player.Pointer == IntPtr.Zero) return;
            if (player.AvatarModel.Pointer == IntPtr.Zero) return;

            var apiAvatar = player.AvatarModel;
            var upload = new avatar
            {
                PCAssetURL = apiAvatar.assetUrl,
                ImageURL = apiAvatar.imageUrl,
                ThumbnailURL = apiAvatar.thumbnailImageUrl,
                AvatarID = apiAvatar.id,
                Tags = apiAvatar.tags,
                AuthorID = apiAvatar.authorId,
                AuthorName = apiAvatar.authorName,
                AvatarDescription = apiAvatar.description,
                AvatarName = apiAvatar.name,
                QUESTAssetURL = "None",
                Releasestatus = apiAvatar.releaseStatus,
                UnityVersion = apiAvatar.unityVersion,
                TimeDetected = $"{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()}",
                Pin = "false",
                PinCode = "None"
            };

            var avi_file = $"{core.ares_dir}\\log.txt";
            var avi_file_ids = $"{core.ares_dir}\\log_ids.txt";
            if (!File.Exists(avi_file))
                File.AppendAllText(avi_file, "main avatar file created by ares logger - by unixian\n");

            if (!File.Exists(avi_file_ids))
                File.AppendAllText(avi_file_ids, "main avatar id file created by ares logger - by unixian\n");

            if (!contains_avi_id(avi_file_ids, apiAvatar.id))
            {
                File.AppendAllText(avi_file_ids, apiAvatar.id + "\n");
                File.AppendAllLines(avi_file, new[]
                {
                    $"Time Detected: {((DateTimeOffset) DateTime.UtcNow).ToUnixTimeSeconds()}",
                    $"Avatar ID: {apiAvatar.id}",
                    $"Avatar Name: {apiAvatar.name}",
                    $"Avatar Description: {apiAvatar.description}",
                    $"Author ID: {apiAvatar.authorId}",
                    $"Author Name: {apiAvatar.authorName}"
                });

                File.AppendAllLines(avi_file, new[]
                {
                    $"PC Asset URL: {apiAvatar.assetUrl}",
                    "Quest Asset URL: None",
                    $"Image URL: {apiAvatar.imageUrl}",
                    $"Thumbnail URL: {apiAvatar.thumbnailImageUrl}",
                    $"Unity Version: {apiAvatar.unityVersion}",
                    $"Release Status: {apiAvatar.releaseStatus}"
                });


                if (apiAvatar.tags.Length > 0)
                {
                    var builder = new StringBuilder();
                    builder.Append("Tags: ");
                    foreach (var tag in apiAvatar.tags) builder.Append($"{tag},");
                    File.AppendAllText(avi_file, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
                }
                else
                {
                    File.AppendAllText(avi_file, "Tags: None");
                }

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.ares-mod.com/records/Avatars");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.UserAgent = "ARES";

                string text = json<avatar>.serialize(upload);
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(text);
                }

                try
                {
                    using (StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream()))
                    {
                        streamReader.ReadToEnd();
                    }
                    log_sys.log($"[log]: successfully logged {apiAvatar.name} - to file and API", ConsoleColor.Green);
                    File.AppendAllText(avi_file, "\n\n");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("409"))
                    {
                        log_sys.log($"[log]: {apiAvatar.name} already exists on ARES API - logged to file.", ConsoleColor.Blue);
                        File.AppendAllText(avi_file, "\n\n");
                    }
                    else
                    {
                        log_sys.log($"[log failure]: unknown exception, e: {ex.Message}", ConsoleColor.Red);
                    }
                }
                
            }
            else
            {
                if (core.ares_debug)
                {
                    log_sys.log($"[log]: did not log or upload {apiAvatar.name} due to it already being locally logged.", ConsoleColor.Blue);
                }
            }
            
        }

        public static bool contains_avi_id(string avatarFile, string avatarId)
        {
            var lines = File.ReadLines(avatarFile);
            foreach (var line in lines)
                if (line.Contains(avatarId))
                    return true;

            return false;
        }
    }
}
