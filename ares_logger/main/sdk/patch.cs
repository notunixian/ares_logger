using ares_logger.util;
using IL2CPP_Core.Objects;
using System;
using System.Runtime.InteropServices;

namespace ares_logger.sdk
{
    unsafe internal class patch : IL2Object
    {
        internal IL2Method target;
        internal IntPtr orig;

        internal patch(IL2Method method, Delegate new_method) : base(IntPtr.Zero)
        {
            Pointer = new_method.Method.MethodHandle.GetFunctionPointer();
            target = method;
            hook.create_hook(*(IntPtr*)method.Pointer, Pointer, out orig);
            Enabled = true;
        }

        public T create_delegate<T>() where T : Delegate
        {
            return Marshal.GetDelegateForFunctionPointer(orig, typeof(T)) as T;
        }

        public bool Enabled
        {
            get => is_enabled;
            set
            {
                if (is_enabled = value)
                    hook.enable_hook(*(IntPtr*)target.Pointer);
                else
                    hook.disable_hook(*(IntPtr*)target.Pointer);
            }
        }

        private bool is_enabled = true;
    }
}
