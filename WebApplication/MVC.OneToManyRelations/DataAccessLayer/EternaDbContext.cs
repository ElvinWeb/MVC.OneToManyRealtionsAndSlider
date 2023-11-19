using Microsoft.EntityFrameworkCore;
using MVC.OneToManyRelations.Models;

namespace MVC.OneToManyRelations.DataAccessLayer
{
    public class EternaDbContext : DbContext
    {
        public EternaDbContext(DbContextOptions<EternaDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Slider> Sliders { get; set; }
    }
}
