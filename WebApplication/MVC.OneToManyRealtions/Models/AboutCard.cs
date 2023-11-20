using System.ComponentModel.DataAnnotations;

namespace MVC.SliderFrontToBack.Models
{
    public class AboutCard
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Desc { get; set; }
    }
}
