using MVC.OneToManyRelations.Models;

namespace MVC.OneToManyRelations.ViewModels
{
    public class PortfolioViewModel
    {
        public List<Category> Categories {  get; set; }
        public List<Portfolio> Portfolios { get; set; }
        public List<Slider> Sliders { get; set; }
    }
}
