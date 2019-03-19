using System;

namespace EasyLab.Server.DTOs
{
    public class ReserveQueue
    {
        public System.Guid Id { get; set; }

        public System.Guid userId { get; set; }

        public string reserveId { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        public DateTime? loginDate { get; set; }

        public DateTime? logoutDate { get; set; }

        public int cancelReserve { get; set; }

        public string reserveUserId { get; set; }

        public int autoCancelReserve { get; set; }

        public int Flag { get; set; }

        public int? Sequence { get; set; }

        public bool IsTemporary { get; set; }

        public string comment { get; set; }
    }
}
