using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Construct
    {
        public int Id;
        public string Name;
        public string Demand;
        public string HouseTypeDescribe;
        public string Address;
        public int UserId;
        public int? FinishState;
        public DateTime? PlanstartTime;
        public DateTime? PlanendTime;
        public DateTime CreateTime;
        public int? ConstProtection;
        public int? ConstSafety;
    }

}
