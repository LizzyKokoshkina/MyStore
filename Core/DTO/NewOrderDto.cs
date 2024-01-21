namespace Core.DTO
{
    public class NewOrderDto
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Nonce { get; set; }
        public IEnumerable<ProductDto> Items { get; set; }
    }
}
