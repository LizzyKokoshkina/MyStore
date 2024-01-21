namespace Core.Entites
{
    public class Order : EntityBase
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string? Nonce { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }
}
