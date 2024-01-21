namespace Core.Entites
{
    public class Image : EntityBase
    {
        public string Src { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
