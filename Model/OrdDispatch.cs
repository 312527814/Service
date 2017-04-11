using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OrdDispatch
    {
        public int Id { get; set; }
        public int ConstId { get; set; }
        public int RoleId { get; set; }
        public int Day { get; set; }
        public decimal Price { get; set; }
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
