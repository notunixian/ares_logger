using ares_logger.main.funcs;
using ares_logger.main.util;
using Assembly_CSharp;
using Assembly_CSharp.VRC.Core;
using IL2CPP_Core.Objects;
using System;

namespace ares_logger.main.patches
{
    internal class download_mgr
    {
        public static sdk.patch patch;
        public delegate IntPtr _bundledownload(IntPtr hiddenStructReturn, IntPtr thisPtr, IntPtr pApiWorld, IntPtr pMulticastDelegate, bool param_3);
        public static _bundledownload __bundledownload;

        public static void init_patch()
        {
            try
            {
                var methodsBundle = AssetBundleDownloadManager.Instance_Class.GetMethods(x => x.GetParameters().Length >= 2);
                foreach (var method1 in methodsBundle)
                {
                    log_sys.debug_log($"{method1.Name} | {method1.GetParameters()[0].ReturnType.Name} / {method1.GetParameters()[1].ReturnType.Name} / {method1.GetParameters()[2].ReturnType.Name}");
                }

                IL2Method method = AssetBundleDownloadManager.Instance_Class.GetMethod("DownloadWorldAssetBundle");
                if (method == null)
                    throw new NullReferenceException();

                patch = new sdk.patch(method, (_bundledownload)bundle_download);
                __bundledownload = patch.create_delegate<_bundledownload>();
            }
            catch (Exception e)
            {
                log_sys.log($"[patch fail]: downloadmgr patch exception | e: {e.Message}", ConsoleColor.Red);
            }
        }

        public static IntPtr bundle_download(IntPtr hiddenStructReturn, IntPtr thisPtr, IntPtr pApiWorld, IntPtr pMulticastDelegate, bool param_3)
        {
            try
            {
                var apiWorld = new ApiWorld(pApiWorld);
                logging.execute_log(null, false, true, apiWorld);
            }
            catch (Exception e) { log_sys.debug_log($"fail at bundle code, e: {e.Message}"); }

            return __bundledownload(hiddenStructReturn, thisPtr, pApiWorld, pMulticastDelegate, param_3);
        }
    }
}
