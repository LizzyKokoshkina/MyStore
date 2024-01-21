namespace Core.Entites
{
    public class Product : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double NewPrice { get; set; }
        public bool IsOnSale { get; set; }
        public string Color { get; set; }
        public string Sizes { get; set; }
        public int Quantity { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<ProductCategory> Categories { get; set; }
    }
}
