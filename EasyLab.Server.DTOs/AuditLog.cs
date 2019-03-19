
namespace EasyLab.Server.DTOs
{
    public class AuditLog
    {
        public Log Log { get; set; }
        public User User { get; set; }
        public Application Application { get; set; }
    }
}
