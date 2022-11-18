using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ares_logger.main.util
{
    [DataContract]
    public class world
    {
        [DataMember]
        public string TimeDetected { get; set; }

        [DataMember]
        public string WorldID { get; set; }

        [DataMember]
        public string WorldName { get; set; }

        [DataMember]
        public string WorldDescription { get; set; }

        [DataMember]
        public string AuthorName { get; set; }

        [DataMember]
        public string AuthorID { get; set; }

        [DataMember]
        public string PCAssetURL { get; set; }

        [DataMember]
        public string ImageURL { get; set; }

        [DataMember]
        public string ThumbnailURL { get; set; }

        [DataMember]
        public string UnityVersion { get; set; }

        [DataMember]
        public string Releasestatus { get; set; }

        [DataMember]
        public string Tags { get; set; }

    }
}
