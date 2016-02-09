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
    public class Schedule
    {
        [Key]
        [DataMember]
        public int sID { get; set; }
        [DataMember]
        public int taskID { get; set; }
        [DataMember]
        public int intHours { get; set; }
        [DataMember]
        public int intMin { get; set; }
        [DataMember]
        public int intSec { get; set; }
        [DataMember]
        public DateTime? startAt { get; set; }
        [DataMember]
        public DateTime? endAt { get; set; }
        [DataMember]
        public int repeatforever { get; set; }
	
    }
}
