namespace MVC.OneToManyRelations.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Portfolio> Portfolios { get; set; }

    }
}
