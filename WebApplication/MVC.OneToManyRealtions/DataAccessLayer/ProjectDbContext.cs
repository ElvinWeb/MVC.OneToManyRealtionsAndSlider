using Microsoft.EntityFrameworkCore;
using MVC.OneToManyRealtions.Models;

namespace MVC.OneToManyRealtions.DataAccessLayer
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
    }
}
