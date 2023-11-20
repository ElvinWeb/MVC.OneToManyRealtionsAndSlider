using Microsoft.EntityFrameworkCore;
using MVC.OneToManyRealtions.Models;
using MVC.SliderFrontToBack.Models;

namespace MVC.OneToManyRealtions.DataAccessLayer
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AboutCard> AboutCards { get; set; }
    }
}
