using Microsoft.AspNetCore.Mvc.Rendering;
using Patisserie.Areas.Identity.Data;
using Patisserie.Models;

namespace Patisserie.ViewModels
{
    public class CreateReviewViewModel
    {
        public Review Review { get; set; }
        public PatisserieUser User { get; set; }
        public string ProductName { get; set; } 
        public int ProductId { get; set; } 
    }
}
