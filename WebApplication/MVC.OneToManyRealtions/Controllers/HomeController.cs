using Microsoft.AspNetCore.Mvc;
using MVC.OneToManyRealtions.DataAccessLayer;
using MVC.OneToManyRealtions.Models;

namespace MVC.OneToManyRealtions.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectDbContext _DbContext;

        public HomeController(ProjectDbContext context)
        {
            _DbContext = context;
        }
       
        public IActionResult Index()
        {
            List<Slider> sliderList = _DbContext.Sliders.ToList(); 

            return View(sliderList);
        }
        public IActionResult ProductDetails()
        {
            return View();
        }
    }
}
