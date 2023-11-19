using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.OneToManyRelations.DataAccessLayer;
using MVC.OneToManyRelations.Models;
using MVC.OneToManyRelations.ViewModels;

namespace MVC.OneToManyRelations.Controllers
{

    public class PortfolioController : Controller
    {
        private readonly EternaDbContext _DbContext;
        private PortfolioViewModel _ViewModel = new PortfolioViewModel();

        public PortfolioController(EternaDbContext context)
        {
            _DbContext = context;
        }
        public IActionResult PortfolioIndex()
        {
            _ViewModel.Categories = _DbContext.Categories.ToList();
            _ViewModel.Portfolios = _DbContext.Portfolios.Include(x => x.Images).ToList();
            return View(_ViewModel);
        }
        public IActionResult PortfolioDetails(int id)
        {
            Portfolio portfolio = _DbContext.Portfolios.Include(x => x.Images).Include(x => x.Category).FirstOrDefault(portfolio => portfolio.Id == id);

            if (portfolio == null)
            {
                return NotFound();
            }
            return View(portfolio);
        }
    }
}
