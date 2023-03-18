using EBW.Models;
using Microsoft.EntityFrameworkCore;

namespace EBW.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
