namespace Core.DTO
{
    public class TransactionDto
    {
        public DateTime? CreatedAt { get; set; }
        public decimal? Amount { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}
