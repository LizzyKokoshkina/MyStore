using Core.DTO;
using Core.Entites;
using Core.Enums;

namespace Business.Interfaces
{
    public interface IProfileManager
    {
        Task<Result> ChangePassword(ChangePasswordDto changePassword, CancellationToken cancellationToken = default);
        Task<Result> Edit(User user, CancellationToken cancellationToken = default);
    }
}
