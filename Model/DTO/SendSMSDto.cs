using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class SendSmsDto
    {
        public int ConstId { get; set; }

        public int RoleId { get; set; }

        public string Phone { get; set; }

        public string RolesName { get; set; }

        public string ConstName { get; set; }
}
}
