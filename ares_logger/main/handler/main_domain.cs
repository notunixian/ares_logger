using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.handler
{
    [ComImport, Guid("A3FA5B56-22BA-4550-8016-915BF8A395A6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface main_domain
    {
        void init();
        void setup_hook(IntPtr create_hook, IntPtr remove_hook, IntPtr enable_hook, IntPtr disable_hook);
    }
}
