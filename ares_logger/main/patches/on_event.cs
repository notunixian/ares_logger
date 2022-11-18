using ares_logger.main.funcs;
using ares_logger.main.util;
using Assembly_CSharp;
using Assembly_CSharp.VRC.Core;
using IL2CPP_Core.Objects;
using Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main.patches
{
    internal class on_event
    {
        public static sdk.patch patch;
        public delegate void _onevent(IntPtr instance, IntPtr data);
        public static _onevent __onevent;

        public static void init_patch()
        {
            try
            {
                IL2Method method = VRCNetworkingClient.Instance_Class.GetMethod("OnEvent");
                if (method == null)
                    throw new NullReferenceException();

                patch = new sdk.patch(method, (_onevent)event_patch);
                __onevent = patch.create_delegate<_onevent>();
            }
            catch (Exception e)
            {
                log_sys.log($"[patch fail]: onevent patch exception | e: {e.Source}", ConsoleColor.Red);
            }

        }

        private static void event_patch(IntPtr instance, IntPtr data)
        {
            var event_data = new EventData(data);
            switch (event_data.Code)
            {
                // event 223 does not contain a valid sender at all in my testing, so it's not going to be used here.
                case 42:
                    log_avatar(event_data.Sender);
                    break;
                default:
                    break;
            }

            __onevent(instance, data);
        }

        private static void log_avatar(int actor_id)
        {
            log_sys.debug_log($"log_avatar (int actor_id) enter");
            try
            {
                var list = network_mgr.player_list.TryGetValue(actor_id, out var player);
                if (list == true)
                {
                    funcs.logging.execute_log(player.vrc_player, true);
                }
            }
            catch (Exception e)
            {
                log_sys.log($"[log failure]: unknown exception: {e.Message}", ConsoleColor.Red);
            }
            log_sys.debug_log($"log_avatar (int actor_id) exit");
        }

        // override that takes 0 params for when you just join a world.
        public static void log_avatar()
        {
            log_sys.debug_log($"log_avatar (void) enter");
            try
            {
                foreach (VRCPlayer player in UnityEngine.Object.FindObjectsOfType<VRCPlayer>())
                {
                    funcs.logging.execute_log(player, true);
                }
            }
            catch (Exception e)
            {
                log_sys.log($"[log failure]: unknown exception: {e.Source}", ConsoleColor.Red);
            }
            log_sys.debug_log($"log_avatar (void) exit");
        }
    }
}
