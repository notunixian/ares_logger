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
            var apiAvatar = player.AvatarModel;
            var upload = new avatar
            {
                PCAssetURL = apiAvatar.assetUrl,
                ImageURL = apiAvatar.imageUrl,
                ThumbnailURL = apiAvatar.thumbnailImageUrl,
                AvatarID = apiAvatar.id,
                Tags = "None",
                AuthorID = apiAvatar.authorId,
                AuthorName = apiAvatar.authorName,
                AvatarDescription = apiAvatar.description,
                AvatarName = apiAvatar.name,
                QUESTAssetURL = "None",
                Releasestatus = apiAvatar.releaseStatus,
                UnityVersion = apiAvatar.unityVersion,
                TimeDetected = "1649875469",
                Pin = "false",
                PinCode = "None"
            };

            if (core.ares_debug == true)
            {
                Console.WriteLine("[log] begginning debug log");
                Console.WriteLine($"pc asset url: {upload.PCAssetURL}\n" +
                                  $"image url: {upload.ImageURL}\n" +
                                  $"thumbnail url: {upload.ThumbnailURL}\n" +
                                  $"avatar id: {upload.AvatarID}\n" +
                                  $"author id: {upload.AuthorID}\n" +
                                  $"author name: {upload.AuthorName}\n" +
                                  $"avatar description: {upload.AvatarDescription}\n" +
                                  $"avatar name: {upload.AvatarName}\n" +
                                  $"releasestatus {upload.Releasestatus}\n" +
                                  $"unityversion: {upload.UnityVersion}\n");
                Console.WriteLine("[log] ending debug log\n");
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
                Console.WriteLine($"[log] successfully uploaded {apiAvatar.name} to API");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("409"))
                {
                    Console.WriteLine($"[log] {apiAvatar.name} already exists on api.");
                }
                else
                {
                    Console.WriteLine($"[log failure] unknown exception, e: {ex.Message}");
                }
            }
        }
    }
}
