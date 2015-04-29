using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace DataModel.Data.ApplicationLayer.Identity
{
    public class PcdSignInManager : SignInManager<User, string>
    {
        public PcdSignInManager(PcdUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public Task<ClaimsIdentity> CreateUserIdentityAsync(PcdUserRepository userRepo, User user)
        {
            return userRepo.GenerateUserIdentityAsync((PcdUserManager)UserManager, user);
        }

        public static PcdSignInManager Create(IdentityFactoryOptions<PcdSignInManager> options, IOwinContext context)
        {
            return new PcdSignInManager(context.GetUserManager<PcdUserManager>(), context.Authentication);
        }
    }
}
