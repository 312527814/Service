using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comm
{
    public class RoleConvert
    {
        public static string GetRoleName(int roleId)
        {
            switch (roleId)
            {
                case 9:
                    return "水工";
                case 10:
                    return "电工";
                case 11:
                    return "木工";
                case 12:
                    return "瓦工";
                case 13:
                    return "油工";
                default:
                    return "未知";
            }
        }
    }
}
