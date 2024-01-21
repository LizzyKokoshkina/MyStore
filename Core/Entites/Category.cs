namespace Core.Entites
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public ICollection<ProductCategory> Products { get; set; }
    }
}
