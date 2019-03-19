using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Server.DTOs
{
    public class Queue
    {
        public User User { get; set; }

        public ReserveQueue ReserveQueue { get; set; }
    }
}
