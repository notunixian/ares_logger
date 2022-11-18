using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main.config
{
    public class config
    {
        public bool log_avatars {get; set;}

        public bool ignore_friends { get; set; }

        public bool log_worlds { get; set; }
    }
}
