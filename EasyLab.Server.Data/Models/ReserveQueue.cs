using System;

namespace EasyLab.Server.Data.Models
{
    public partial class ReserveQueue
    {
        public System.Guid queueId { get; set; }

        public System.Guid userId { get; set; }

        public string reserveId { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        public DateTime? loginDate { get; set; }

        public DateTime? logoutDate { get; set; }

        public int cancelReserve { get; set; }

        public int autoCancelReserve { get; set; }

        public int Flag { get; set; }

        public int? Sequence { get; set; }

        public bool IsTemporary { get; set; }

        public string comment { get; set; }
    }
}
