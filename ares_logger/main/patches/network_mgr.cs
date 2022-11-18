using ares_logger.main.util;
using ares_logger.sdk;
using Assembly_CSharp;
using Assembly_CSharp.VRC;
using IL2CPP_Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main.patches
{
    internal class network_mgr
    {
        public delegate void _(IntPtr _instance, IntPtr _player);
        public static patch player_join;
        public static _ __OnPlayerJoined;

        public static patch player_left;
        public static _ __OnPlayerLeft;

        public static Dictionary<int, Assembly_CSharp.VRC.Player> player_list = new Dictionary<int, Assembly_CSharp.VRC.Player>();
        public static Dictionary<IntPtr, int> actor_list = new Dictionary<IntPtr, int>();

        public static void init_patch()
        {
            try
            {
                IL2Method method = NetworkManager.Instance_Class.GetMethod("OnPlayerJoined");
                player_join = new patch(method, (_)on_join);
                __OnPlayerJoined = player_join.create_delegate<_>();
            }
            catch (Exception e) { log_sys.log($"[patch fail]: networkmgr (onplayerjoin) patch exception | e: {e.Message}", ConsoleColor.Red); }

            try
            {
                IL2Method method = NetworkManager.Instance_Class.GetMethod("OnPlayerLeft");
                player_left = new patch(method, (_)on_left);
                __OnPlayerLeft = player_join.create_delegate<_>();
            }
            catch (Exception e) { log_sys.log($"[patch fail]: networkmgr (onplayerleft) patch exception | e: {e.Message}", ConsoleColor.Red); }


        }

        private static void on_join(IntPtr _instance, IntPtr _player)
        {
            if (_player == IntPtr.Zero) return;
            var player = new Assembly_CSharp.VRC.Player(_player);


            try
            {
                if (player.vrc_player.actor_id == VRCPlayer.Instance.actor_id) patches.on_event.log_avatar();
                player_list.Add(player.vrc_player.actor_id, player);
                actor_list.Add(player.Pointer, player.vrc_player.actor_id);
            }
            catch (Exception e)
            {
                log_sys.log($"[on_join]: failed at on_join code, e: {e.Message}", ConsoleColor.Red);
            }
            

            __OnPlayerJoined(_instance, _player);
        }

        private static void on_left(IntPtr _instance, IntPtr _player)
        {
            if (_player == IntPtr.Zero) return;

            var player = new Player(_player);


            // some shit code to fix vrcplayer being destroyed before i can access it
            try
            {
                var list = actor_list.TryGetValue(player.Pointer, out int actor);
                if (list == true)
                {
                    player_list.Remove(actor);
                    actor_list.Remove(player.Pointer);
                }
            }
            catch (Exception e)
            {
                log_sys.log($"[on_join]: failed at on_left code, e: {e.Message}", ConsoleColor.Red);
            }


            __OnPlayerLeft(_instance, _player);
        }

    }
}
