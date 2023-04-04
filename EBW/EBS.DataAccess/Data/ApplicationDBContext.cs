using EBW.Models;
using Microsoft.EntityFrameworkCore;

namespace EBW.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Product> Products { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelbulder)
        //{
        //    modelbulder.Entity<CoverType>().HasData(new CoverType {Id = 1, Name = "Paper"});
        //}
    }
}
