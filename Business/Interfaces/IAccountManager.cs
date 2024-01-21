using Core.DTO;
using Core.Enums;

namespace Business.Interfaces
{
    public interface IAccountManager
    {
        Task<LoginResultDto> Login(LoginDto login, CancellationToken cancellationToken = default);
        Task<Result> Register(RegisterDto register, CancellationToken cancellationToken = default);
    }
}
