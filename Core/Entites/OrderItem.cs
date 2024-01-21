namespace Core.Entites
{
    public class OrderItem : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
