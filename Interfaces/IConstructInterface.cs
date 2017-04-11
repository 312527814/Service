using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Interfaces
{
    public interface IConstructInterface
    {
        List<SMSEvalLogDTO> GetConstruct();

        int AddSMSLog(SMSEvalLog model);
    }
}
