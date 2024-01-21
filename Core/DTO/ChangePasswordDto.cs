namespace Core.DTO
{
    public class ChangePasswordDto
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
