using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zza.Entities
{
    [DataContract]
    public class Client
    {
        [Key]
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string HostName { get; set; }
        [DataMember]
        public string IpAddress { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }

    }
}
