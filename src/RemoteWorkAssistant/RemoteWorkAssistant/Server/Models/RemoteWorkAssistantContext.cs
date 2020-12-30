using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace RemoteWorkAssistant.Server.Models
{
    public class RemoteWorkAssistantContext : DbContext
    {
        public RemoteWorkAssistantContext(DbContextOptions<RemoteWorkAssistantContext> options)
          : base(options)
        {
        }

        public DbSet<UserRecord> UserTable { get; set; }
        public DbSet<PcRecord> PcTable { get; set; }

        public bool ExistsUserRecord(string mailAddress)
        {
            return this.UserTable.Any(e => e.MailAddress.Equals(mailAddress));
        }

        public bool ExistsPcRecord(string id)
        {
            return this.PcTable.Any(e => e.Id.Equals(id));
        }

        public static string GeneratePcInfoId(string mailAddress, string pcName)
        {
            string delimiter = "-";
            return new StringBuilder()
              .Append(mailAddress).Append(delimiter).Append(pcName).ToString();
        }
    }
}
