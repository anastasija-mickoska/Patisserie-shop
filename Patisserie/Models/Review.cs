using Patisserie.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Patisserie.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public Product? Product { get; set; }
        public PatisserieUser? User { get; set; }
        [StringLength(500)]
        public string Comment { get; set; }
        public int? Rating { get; set; }
    }
}
