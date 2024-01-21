using Business.Interfaces;
using Business.Services;
using Core.Database.UnitOfWork;
using Core.DTO;
using Core.Entites;
using Core.Enums;
using CryptoHelper;

namespace Business.Managers
{
    public class ProfileManager : ManagerBase, IProfileManager
    {
        private readonly IEmailService emailService;

        public ProfileManager(IUnitOfWork unitOfWork, IEmailService emailService) : base(unitOfWork)
        {
            this.emailService = emailService;
        }

        public async Task<Result> ChangePassword(ChangePasswordDto password, CancellationToken cancellationToken = default)
        {
            var user = await UnitOfWork.Sql<User>().GetFirst(x => x.Id == password.UserId, x => x, cancellationToken);
            if (user == null || !Crypto.VerifyHashedPassword(user.Password, password.Password))
            {
                return Result.Fail;
            }
            user.Password = Crypto.HashPassword(password.NewPassword);
            await UnitOfWork.Sql<User>().Update(user);
            await UnitOfWork.SaveChanges(cancellationToken);
            return Result.Success;
        }

        public async Task<Result> Edit(User user, CancellationToken cancellationToken = default)
        {
            var entity = await UnitOfWork.Sql<User>().GetFirst(x => x.Id == user.Id, x => x, cancellationToken);
            if (entity == null || await UnitOfWork.Sql<User>().Exist(x => x.Id != user.Id && x.Email == user.Email, cancellationToken))
            {
                return Result.Fail;
            }
            entity.Email = user.Email;
            entity.Firstname = user.Firstname;
            entity.Lastname = user.Lastname;
            await UnitOfWork.Sql<User>().Update(entity);
            await UnitOfWork.SaveChanges(cancellationToken);
            return Result.Success;
        }
    }
}
