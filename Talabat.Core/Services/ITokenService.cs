
namespace Talabat.Core.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser User,UserManager<AppUser> userManager);
    }
}
