using Microsoft.AspNetCore.Mvc.Rendering;
using Patisserie.Models;
using System.Collections.Generic;
namespace Patisserie.ViewModels
{
    public class ProductsFlavoursEditViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<int>? SelectedFlavours { get; set; }
        public IEnumerable<SelectListItem>? FlavourList { get; set; }
    }
}