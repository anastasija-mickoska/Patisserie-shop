using Patisserie.Areas.Identity.Data;

namespace Patisserie.Models
{
    public class UserProduct
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public PatisserieUser? User { get; set; }
        public Product? Product { get; set; }
    }
}
