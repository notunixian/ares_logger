using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main.config
{
    [DataContract]
    public class config
    {
        // future proofing, i'll add checks to add new values if the version mismatches
        [DataMember]
        public int version = 1;

        [DataMember]
        public bool log_avatars {get; set;}

        [DataMember]
        public bool ignore_friends { get; set; }

        [DataMember]
        public bool log_worlds { get; set; }
    }
}
