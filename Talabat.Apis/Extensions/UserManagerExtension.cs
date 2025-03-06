using System.Security.Claims;

namespace Talabat.Apis.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(a=>a.Address).FirstOrDefaultAsync(x=>x.Email == email);
            return user;
        }
    }
}
