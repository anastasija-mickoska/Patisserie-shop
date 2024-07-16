using Microsoft.AspNetCore.Mvc.Rendering;
using Patisserie.Models;

namespace Patisserie.ViewModels
{
    public class CategoryFilterViewModel
    {
        public IList<Category>? Categories { get; set; }
        public SelectList? Filters { get; set; }
        public string searchString { get; set; }
    }
}
