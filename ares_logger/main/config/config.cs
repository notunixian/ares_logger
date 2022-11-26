using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main.config
{
    public class conf
    {
        public static config_handler<values> handler;
    }

    public class values
    {
        public bool log_avatars { get; set; } = true;
        public bool ignore_friends { get; set; } = false;
        public bool log_worlds { get; set; } = true;
        public bool enable_unsafe_features { get; set; } = false;
    }
}
