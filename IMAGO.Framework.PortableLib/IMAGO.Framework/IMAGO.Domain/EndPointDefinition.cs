using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IMAGO.Domain
{
    [DataContract]
    public class EndPointDefinition
    {
        [DataMember]
        public string Contract { get; set; }
        [DataMember]
        public string Service { get; set; }
    }
}
