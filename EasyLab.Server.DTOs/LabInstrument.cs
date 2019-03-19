using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Server.DTOs
{
    public class LabInstrument
    {
        public Guid Id { get; set; }

        public string RecordId { get; set; }

        public string ParentId { get; set; }

        public string RecordName { get; set; }

        public int cancelAfterTime { get; set; }

        public int cancelPreTime { get; set; }

        public int cancelWithoutActionTime { get; set; }

        public int loginPreTime { get; set; }

        public string userId { get; set; }
    }
}
