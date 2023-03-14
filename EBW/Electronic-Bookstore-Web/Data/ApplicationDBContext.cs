using Electronic_Bookstore_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Electronic_Bookstore_Web.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<CoverType> CoverTypes { get; set; }

    }
}
