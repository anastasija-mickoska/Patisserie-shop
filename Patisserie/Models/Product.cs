using System.ComponentModel.DataAnnotations;

namespace Patisserie.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductFlavour>? ProductFlavours { get; set;}
        public ICollection<Review>? Reviews { get; set;}
        public ICollection<UserProduct>? UserProducts { get; set; }
        public double AverageRating()
        {
            if (Reviews == null || Reviews.Count == 0)
            {
                return 0;
            }
            int sumOfRatings = 0;
            foreach (var item in Reviews)
            {
                if (item.Rating != null)
                {
                    sumOfRatings += (int)item.Rating;
                }
            }
            double averageRating = (double)sumOfRatings / Reviews.Count;
            return averageRating;
        }

    }
}
