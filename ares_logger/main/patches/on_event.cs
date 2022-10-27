using ares_logger.main.funcs;
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
            Console.WriteLine("[patches] onevent patch start");
            try
            {
                IL2Method method = VRCNetworkingClient.Instance_Class.GetMethod("OnEvent");
                patch = new sdk.patch(method, (_onevent)event_patch);
                __onevent = patch.create_delegate<_onevent>();
                Console.WriteLine("[patches] onevent patch success");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[patches] onevent patch exception | e: {e.Message}");
            }
            
        }

        private static void event_patch(IntPtr instance, IntPtr data)
        {
            if (data == IntPtr.Zero) return;
            __onevent(instance, data);

            var event_data = new EventData(data);
            switch (event_data.Code)
            {
                // event 223 would of been here, but in my testing both of these get sent no matter what.
                case 42:
                    log_avatar(event_data.Sender);
                    break;
                default:
                    break;
            }
        }

        private static void log_avatar(int actor_id)
        {
            try
            {
                var list = network_mgr.player_list.TryGetValue(actor_id, out var player);
                if (list == true)
                {
                    funcs.logging.execute_log(player.vrc_player, true);
                }
            }
            catch
            {
                Console.WriteLine("[log failure] unable to call log");
            }
        }

        // override that takes 0 params for when you just join a world.
        public static void log_avatar()
        {
            try
            {
                foreach (VRCPlayer player in UnityEngine.Object.FindObjectsOfType<VRCPlayer>())
                {
                    funcs.logging.execute_log(player, true);
                }
            }
            catch
            {
                Console.WriteLine("[log failure] unable to foreach");
            }
        }
    }
}
