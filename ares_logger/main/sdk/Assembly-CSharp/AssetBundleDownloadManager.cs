using ares_logger.main.util;
using Assembly_CSharp.VRC.Core;
using IL2CPP_Core.Objects;
using System;
using System.Linq;
using UnityEngine;

namespace Assembly_CSharp
{
    internal class AssetBundleDownloadManager : MonoBehaviour
    {
        public AssetBundleDownloadManager(IntPtr ptr) : base(ptr) { }

        static AssetBundleDownloadManager()
        {
            // finds the unitask for loading world bundles
            (Instance_Class.GetMethod(x => x.GetParameters().Length > 1 && x.GetParameters()[0].ReturnType.Name == ApiWorld.Instance_Class.FullName)).Name = "DownloadWorldAssetBundle";
            (Instance_Class.GetMethod(x => x.GetParameters().Length > 1 && x.GetParameters()[0].ReturnType.Name == ApiAvatar.Instance_Class.FullName)).Name = "DownloadAvatarAssetBundle";
        }

        public static new IL2Class Instance_Class = IL2CPP.AssemblyList["Assembly-CSharp"].GetClasses().FirstOrDefault(x => x.BaseType == MonoBehaviour.Instance_Class && x.GetField(y => y.ReturnType.Name == "UnityEngine.Cache") != null);
    }
}
