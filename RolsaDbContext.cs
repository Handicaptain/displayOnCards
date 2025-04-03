namespace displayOnCards
{
    using Microsoft.EntityFrameworkCore;

    public class RolsaDbContext : DbContext
    {
        public RolsaDbContext(DbContextOptions<RolsaDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> tblUsers { get; set; }  // This should match your database table name
    }
}
