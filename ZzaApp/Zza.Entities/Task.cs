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
    public class Task
    {
        [Key]
        [DataMember]
        public int taskID { get; set; }
        [DataMember]
        public string TaskName { get; set; }
        [DataMember]
        public string TaskType { get; set; }
        [DataMember]
        public string Command { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string OutputLog { get; set; }
    }
}
