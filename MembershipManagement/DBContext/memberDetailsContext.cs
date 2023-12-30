using MembershipManagement.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace MembershipManagement.DBContext
{
    public class memberDetailsContext : DbContext
    {
        public memberDetailsContext(DbContextOptions<memberDetailsContext> options) : base(options) { }

        public DbSet<memberDetails> MemberDetails { get;set; }
    }
}
