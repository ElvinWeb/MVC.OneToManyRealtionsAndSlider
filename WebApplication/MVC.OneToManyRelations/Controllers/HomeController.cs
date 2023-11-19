using Microsoft.AspNetCore.Mvc;
using MVC.OneToManyRelations.DataAccessLayer;
using MVC.OneToManyRelations.ViewModels;

namespace MVC.OneToManyRelations.Controllers
{
    public class HomeController : Controller
    {
        private PortfolioViewModel _ViewModel = new PortfolioViewModel();
        private readonly EternaDbContext _DbContext;

        public HomeController(EternaDbContext context)
        {
            _DbContext = context;
        }
        public IActionResult Index()
        {
            _ViewModel.Sliders = _DbContext.Sliders.ToList();
            return View(_ViewModel);
        }
    }
}
