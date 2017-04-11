using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Test
    {
        public int ProcessId { get; set; }

        public int Days { get; set; }

        public int Sort { get; set; }
    }

    public class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? State { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
