using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Server.Business
{
    public enum ReserveQueueFlag
    {
        //it is not login client in reserve queue.
        Normal = 0,
        //cancel reserve lab from server
        ServerCancel = 1,
        //Admin cancel reserve 
        ClientCancel = 2,
        //it is out of access time, so it will auto cancel
        ClientAutoCancel = 3,
        //Normal login and logout
        Success = 4
    }
}
