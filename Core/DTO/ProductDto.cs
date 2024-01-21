using Core.Entites;

namespace Core.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Color { get; set; }
        public string? Sizes { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsOnSale { get; set; }
        public IEnumerable<Category> Categories { get; set;}
    }
}
