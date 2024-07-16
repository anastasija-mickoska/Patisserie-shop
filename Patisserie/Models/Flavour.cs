using System.ComponentModel.DataAnnotations;

namespace Patisserie.Models
{
    public class Flavour
    {
        public int FlavourId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string FlavourImage { get; set; }
        public ICollection<ProductFlavour>? ProductFlavours { get; set; }
    }
}
