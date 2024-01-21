using Business.Interfaces;
using Business.Services;
using Core.Database.UnitOfWork;
using Core.DTO;
using Core.Entites;
using Core.Enums;
using CryptoHelper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace Business.Managers
{
    public class AccountManager : ManagerBase, IAccountManager
    {
        private readonly IEmailService emailService;

        public AccountManager(IUnitOfWork unitOfWork, IEmailService emailService) : base(unitOfWork)
        {
            this.emailService = emailService;
        }

        public async Task<LoginResultDto> Login(LoginDto login, CancellationToken cancellationToken = default)
        {
            var user = await UnitOfWork.Sql<User>().GetFirst(x => x.Email == login.Email, x => x, cancellationToken);
            if (user == null)
            {
                return new LoginResultDto
                {
                    Result = Result.Fail
                };
            }
            if (!Crypto.VerifyHashedPassword(user.Password, login.Password))
            {
                return new LoginResultDto
                {
                    Result = Result.Fail
                };
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, $"{user.Id}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Firstname),
                new Claim("Lastname", user.Lastname),
                new Claim(ClaimTypes.Role, $"{user.Role}"),
            };
            return new LoginResultDto { Result = Result.Success, User = user, Token = new JwtSecurityTokenHandler().WriteToken(CreateToken(claims)) };
        }

        public async Task<Result> Register(RegisterDto register, CancellationToken cancellationToken = default)
        {
            if (await UnitOfWork.Sql<User>().Exist(x => x.Email == register.Email, cancellationToken))
            {
                return Result.Fail;
            }
            await UnitOfWork.Sql<User>().Create(new User
            {
                Firstname = register.Firstname,
                Lastname = register.Lastname,
                Email = register.Email,
                Password = Crypto.HashPassword(register.Password),
                Role = UserRole.User/*UserRole.Admin*/
            }, cancellationToken);
            await UnitOfWork.SaveChanges(cancellationToken);
            return Result.Success;
        }

        public async Task<Result> ForgotPassword(string email, CancellationToken cancellationToken = default)
        {
            if (await UnitOfWork.Sql<User>().Exist(x => x.Email == email, cancellationToken))
            {
                return Result.Fail;
            }
            await emailService.SendEmail(email, "Password recovery", "");
            return Result.Success;

        }

        private JwtSecurityToken CreateToken(IEnumerable<Claim> claims)
        {
            return new JwtSecurityToken(
                issuer: "my-project",
                audience: "my-project",
                notBefore: DateTime.UtcNow,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("asqwezxnbansdqweqwhZadaqwWeqeqSajhdhahqwerEYWEWURbBBASDH12DASthruterasdq123afkjzxczczas")), SecurityAlgorithms.HmacSha512));
        }
    }
}
