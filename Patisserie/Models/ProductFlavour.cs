namespace Patisserie.Models
{
    public class ProductFlavour
    {
        public int Id { get; set; }
        public int FlavourId { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public Flavour? Flavour { get; set; }

    }
}
