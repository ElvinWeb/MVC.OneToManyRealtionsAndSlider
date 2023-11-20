using Microsoft.AspNetCore.Mvc;
using MVC.OneToManyRealtions.DataAccessLayer;
using MVC.SliderFrontToBack.Models;
using MVC.SliderFrontToBack.ViewModel;

namespace MVC.SliderFrontToBack.Areas.Manage.Controllers
{
    [Area("manage")]
    public class DashBoardController : Controller
    {
        private readonly ProjectDbContext _DbContext;
        private HomeViewModel HomeViewModel = new HomeViewModel();

        public DashBoardController(ProjectDbContext context)
        {
            _DbContext = context;
        }
        public IActionResult Index()
        {
            HomeViewModel.AboutCards = _DbContext.AboutCards.ToList();

            return View(HomeViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create(AboutCard card)
        {
            return View();
        }
    }
}
