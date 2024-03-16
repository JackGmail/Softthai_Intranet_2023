using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ST_API.Interfaces
{
    public interface ILineService
    {
        void Webhook(dynamic objReq);
        string TestSend();
    }
}