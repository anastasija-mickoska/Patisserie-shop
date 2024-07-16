using Microsoft.AspNetCore.Mvc.Rendering;
using Patisserie.Models;

namespace Patisserie.ViewModels
{
    public class ProductFilterViewModel
    {
        public IList<Product>? Products { get; set; }
        public SelectList? Filters { get; set; }
        public string searchString { get; set; }

    }
}
