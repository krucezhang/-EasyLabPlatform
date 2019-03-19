
namespace EasyLab.Server.Data.Models
{
    public partial class LabInstrument
    {
        public System.Guid LabInstrumentId { get; set; }

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
