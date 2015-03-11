using System.Security.Claims;
using System.Threading.Tasks;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class PcdWebUserRepository : PcdRepositoryBase<User, string>, IPcdWebUserRepository
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, User user)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserOauthIdentityAsync(UserManager<User> manager, User user)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, OAuthDefaults.AuthenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        
    }
}
