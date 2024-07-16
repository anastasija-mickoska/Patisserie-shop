using System.ComponentModel.DataAnnotations;

namespace Patisserie.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string CategoryImage { get; set; }
        public ICollection<Product> Products { get; set;}
    }
}
