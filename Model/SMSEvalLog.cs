using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SMSEvalLog
    {
        public int Id { get; set; }
        public int? ConstId { get; set; }
        public string Phone { get; set; }
        public string ConstName { get; set; }
        public int? Phase { get; set; }
        public int Role { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
