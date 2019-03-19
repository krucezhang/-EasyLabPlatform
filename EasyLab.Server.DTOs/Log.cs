using System;

namespace EasyLab.Server.DTOs
{
    public class Log
    {
        public Guid Id { get; set; }
        public string InstrumentId { get; set; }
        public string ResourceType { get; set; }
        public string ResourceValue { get; set; }
        public string ResourceAction { get; set; }
        public string ResourceType2 { get; set; }
        public string ResourceValue2 { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
