using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDispatchInterface
    {
        bool InsertDispatch(InputDispatch model);

        List<OrdDispatchDto> GetDispatch();

        void DelDispatch(SendSmsDto model);
    }
}
