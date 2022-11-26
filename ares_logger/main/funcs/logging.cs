using ares_logger.main.config;
using ares_logger.main.util;
using ares_logger.util;
using Assembly_CSharp;
using Assembly_CSharp.VRC;
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
        public static void execute_log(VRCPlayer player, bool aviChange = false, bool worldChange = false, ApiWorld apiWorld = null)
        {
            if (worldChange == false && aviChange == true) process_avatar(player.AvatarModel, player.player);
            if (worldChange == true && aviChange == false) process_world(apiWorld);
        }

        public static bool contains_avi_id(string avatarFile, string avatarId)
        {
            var lines = File.ReadLines(avatarFile);
            foreach (var line in lines)
                if (line.Contains(avatarId))
                    return true;

            return false;
        }

        public static void process_avatar(ApiAvatar apiAvatar, Player player)
        {
            if (player.Pointer == IntPtr.Zero) return;
            if (player.vrc_player.AvatarModel.Pointer == IntPtr.Zero) return;

            if (conf.handler.Config.log_avatars == false) return;

            var avi_file = $"{core.ares_dir}\\avi_log.txt";
            var avi_file_ids = $"{core.ares_dir}\\avi_log_ids.txt";
            if (!File.Exists(avi_file))
                File.AppendAllText(avi_file, "main avi file, made by ares logger\n");

            if (!File.Exists(avi_file_ids))
                File.AppendAllText(avi_file_ids, "main avi id file, made by ares logger\n");



            if (conf.handler.Config.ignore_friends == true && APIUser.IsFriendsWith(player.api_user.id))
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
                    log_sys.log($"[avatar log]: successfully logged {apiAvatar.name} to file and API", ConsoleColor.Green);
                    File.AppendAllText(avi_file, "\n\n");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("409"))
                    {
                        log_sys.log($"[avatar log]: {apiAvatar.name} already exists on ARES API, logged to file.", ConsoleColor.Blue);
                        File.AppendAllText(avi_file, "\n\n");
                    }
                    else
                    {
                        log_sys.log($"[avatar log failure]: unknown exception in upload, e: {ex.Message}", ConsoleColor.Red);
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

        public static bool contains_wrld_id(string wrldFile, string wrldId)
        {
            var lines = File.ReadLines(wrldFile);
            foreach (var line in lines)
                if (line.Contains(wrldId))
                    return true;

            return false;
        }

        public static void process_world(ApiWorld apiWorld)
        {
            if (conf.handler.Config.log_worlds == false) return;
            if (apiWorld.Pointer == IntPtr.Zero) return;

            var wrld_file = $"{core.ares_dir}\\wrld_log.txt";
            var wrld_file_ids = $"{core.ares_dir}\\wrld_log_ids.txt";
            if (!File.Exists(wrld_file))
                File.AppendAllText(wrld_file, "main world file, made by ares logger\n");

            if (!File.Exists(wrld_file_ids))
                File.AppendAllText(wrld_file_ids, "main world id file, made by ares logger\n");

            if (!contains_wrld_id(wrld_file, apiWorld.id))
            {
                var upload = new world
                {
                    PCAssetURL = apiWorld.assetUrl,
                    ImageURL = apiWorld.imageUrl,
                    ThumbnailURL = apiWorld.thumbnailImageUrl,
                    WorldID = apiWorld.id,
                    Tags = (apiWorld.tags.Length > 1) ? string.Join(", ", apiWorld.tags) : "None",
                    AuthorID = apiWorld.authorId,
                    AuthorName = apiWorld.authorName,
                    WorldDescription = apiWorld.description,
                    WorldName = apiWorld.name,
                    Releasestatus = apiWorld.releaseStatus,
                    UnityVersion = apiWorld.unityVersion,
                    TimeDetected = $"{((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds()}",
                };

                File.AppendAllText(wrld_file_ids, apiWorld.id + "\n");
                File.AppendAllLines(wrld_file, new[]
                {
                    $"Time Detected: {((DateTimeOffset) DateTime.UtcNow).ToUnixTimeSeconds()}",
                    $"World ID: {apiWorld.id}",
                    $"World Name: {apiWorld.name}",
                    $"World Description: {apiWorld.description}",
                    $"Author ID: {apiWorld.authorId}",
                    $"Author Name: {apiWorld.authorName}"
                });

                File.AppendAllLines(wrld_file, new[]
                {
                    $"PC Asset URL: {apiWorld.assetUrl}",
                    $"Image URL: {apiWorld.imageUrl}",
                    $"Thumbnail URL: {apiWorld.thumbnailImageUrl}",
                    $"Unity Version: {apiWorld.unityVersion}",
                    $"Release Status: {apiWorld.releaseStatus}"
                });


                if (apiWorld.tags.Length > 0)
                {
                    var builder = new StringBuilder();
                    builder.Append("Tags: ");
                    foreach (var tag in apiWorld.tags) builder.Append($"{tag},");
                    File.AppendAllText(wrld_file, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
                }
                else
                {
                    File.AppendAllText(wrld_file, "Tags: None");
                }

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.ares-mod.com/records/Worlds");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.UserAgent = "ARES Logger";

                string text = json<world>.serialize(upload);
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
                    log_sys.log($"[world log]: successfully logged {apiWorld.name} to file and API", ConsoleColor.Green);
                    File.AppendAllText(wrld_file, "\n\n");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("409"))
                    {
                        log_sys.log($"[world log]: {apiWorld.name} already exists on ARES API, logged to file.", ConsoleColor.Blue);
                        File.AppendAllText(wrld_file, "\n\n");
                    }
                    else
                    {
                        log_sys.log($"[world log failure]: unknown exception in upload, e: {ex.Message}", ConsoleColor.Red);
                        File.AppendAllText(wrld_file, "\n\n");
                    }
                }

            }
            else
            {
                log_sys.debug_log($"skipped {apiWorld.name} -> already exists on file");
            }
        }
    }
}
