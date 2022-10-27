using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ares_logger.main;

namespace ares_logger.handler
{
    internal sealed class domain_handler : AppDomainManager, main_domain
    {
        public domain_handler() { }
        public override void InitializeNewDomain(AppDomainSetup appDomainInfo) { InitializationFlags = AppDomainManagerInitializationOptions.RegisterWithHost; }

        public void init() => core.init();
        public void setup_hook(IntPtr create_hook, IntPtr remove_hook, IntPtr enable_hook, IntPtr disable_hook) 
                    => util.hook.setup_hook(create_hook, remove_hook, enable_hook, disable_hook);
    }
}
