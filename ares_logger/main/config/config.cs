using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main.config
{
    [DataContract]
    internal class config
    {
        [DataMember]
        public bool log_avatars {get; set;}

        [DataMember]
        public bool ignore_friends { get; set; }
    }
}
