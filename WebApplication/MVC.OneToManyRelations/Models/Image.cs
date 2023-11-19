namespace MVC.OneToManyRelations.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}
