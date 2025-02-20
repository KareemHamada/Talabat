

using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Kareem Hamada",
                    Email = "kareemhamada219@gmail.com",
                    UserName = "kareemhamada219",
                    PhoneNumber = "01090802802"

                };
                await userManager.CreateAsync(user,"Pa$$w0rd");   
            }
        }
    }
}
