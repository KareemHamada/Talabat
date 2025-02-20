using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Identity;

namespace Talabat.Apis.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentiyServies(this IServiceCollection Services)
        {
            Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();
            Services.AddAuthentication(); // usermanger  // singinmanager // rolemanager

            return Services;
        }
    }
}
