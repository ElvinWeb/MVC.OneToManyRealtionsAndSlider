using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.SliderFrontToBack.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Tax { get; set; }
        public string Code { get; set; }
        public bool IsAvailable { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public double DiscountPercent { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public int ManufacturerId { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
        [NotMapped]
        public List<int> TagIds { get; set; }
        [NotMapped]
        public List<int>? ProductImageIds { get; set; }
        [NotMapped]
        public IFormFile? ProductMainImage { get; set; }
        [NotMapped]
        public IFormFile? ProductHoverImage { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }

        public List<ProductImage>? ProductImages { get; set; }


    }
}
