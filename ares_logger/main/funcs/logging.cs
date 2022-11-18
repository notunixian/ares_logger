﻿using ares_logger.main.config;
using ares_logger.main.util;
using ares_logger.util;
using Assembly_CSharp;
using Assembly_CSharp.VRC.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
            if (config_handler.get_config().log_avatars == false) return;

            var apiAvatar = player.AvatarModel;

            var avi_file = $"{core.ares_dir}\\log.txt";
            var avi_file_ids = $"{core.ares_dir}\\log_ids.txt";
            if (!File.Exists(avi_file))
                File.AppendAllText(avi_file, "main avi file, made by ares logger\n");

            if (!File.Exists(avi_file_ids))
                File.AppendAllText(avi_file_ids, "main avi id file, made by ares logger\n");

            

            if (config_handler.get_config().ignore_friends == true && APIUser.IsFriendsWith(player.player.api_user.id))
            {
                log_sys.debug_log("ignoring friend due to ignore_friends being set to true.");
                return;
            }

            if (!contains_avi_id(avi_file_ids, apiAvatar.id))
            {
                var upload = new avatar
                {
                    PCAssetURL = apiAvatar.assetUrl,
                    ImageURL = apiAvatar.imageUrl,
                    ThumbnailURL = apiAvatar.thumbnailImageUrl,
                    AvatarID = apiAvatar.id,
                    Tags = (apiAvatar.tags.Length > 1) ? string.Join(", ", apiAvatar.tags) : "None",
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
                httpWebRequest.UserAgent = "ARES Logger";

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
                    log_sys.log($"[log]: successfully logged {apiAvatar.name} to file and API", ConsoleColor.Green);
                    File.AppendAllText(avi_file, "\n\n");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("409"))
                    {
                        log_sys.log($"[log]: {apiAvatar.name} already exists on ARES API, logged to file.", ConsoleColor.Blue);
                        File.AppendAllText(avi_file, "\n\n");
                    }
                    else
                    {
                        log_sys.log($"[log failure]: unknown exception in upload, e: {ex.Message}", ConsoleColor.Red);
                        File.AppendAllText(avi_file, "\n\n");
                    }
                }

            }
            else
            {
                if (core.ares_debug)
                {
                    log_sys.debug_log($"skipped {apiAvatar.name} -> already exists on file");
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
