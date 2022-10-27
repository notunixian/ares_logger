using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.util
{
    internal class hook
    {
        public delegate void _create_hook(IntPtr pTarget, IntPtr pDetour, out IntPtr ppOrig);
        public delegate void _remove_hook(IntPtr pTarget);
        public delegate void _enable_hook(IntPtr pTarget);
        public delegate void _disable_hook(IntPtr pTarget);

        public static _create_hook create_hook { get; private set; }
        public static _remove_hook remove_hook { get; private set; }
        public static _enable_hook enable_hook { get; private set; }
        public static _disable_hook disable_hook { get; private set; }

        private static T create_delegate<T>(IntPtr method) where T : Delegate
        {
            return Marshal.GetDelegateForFunctionPointer(method, typeof(T)) as T;
        }

        public static void setup_hook(IntPtr pcreate_hook, IntPtr premove_hook, IntPtr penable_hook, IntPtr pdisable_hook)
        {
            create_hook = create_delegate<_create_hook>(pcreate_hook);
            remove_hook = create_delegate<_remove_hook>(premove_hook);
            enable_hook = create_delegate<_enable_hook>(penable_hook);
            disable_hook = create_delegate<_disable_hook>(pdisable_hook);
        }

    }
}
