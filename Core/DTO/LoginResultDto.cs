using Core.Entites;
using Core.Enums;

namespace Core.DTO
{
    public class LoginResultDto
    {
        public User User { get; set; }
        public Result Result { get; set; }
        public string Token { get; set; }
    }
}
