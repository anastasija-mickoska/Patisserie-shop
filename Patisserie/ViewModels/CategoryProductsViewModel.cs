using Patisserie.Models;

namespace Patisserie.ViewModels
{
    public class CategoryProductsViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
