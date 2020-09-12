using Microsoft.EntityFrameworkCore;

namespace UFISApp
{
    public class LoginContext : DbContext
    {
        // add our Employee database
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=ufis.database.windows.net; Database=UserLogin; User Id=serveradmin; Password=password1!; Trusted_Connection=False; MultipleActiveResultSets=true");
        }
    }
}
