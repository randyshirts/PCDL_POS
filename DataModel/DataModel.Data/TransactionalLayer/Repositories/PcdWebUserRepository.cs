using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using DataModel.Data.ApplicationLayer.Identity;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class PcdWebUserRepository : PcdRepositoryBase<User, string>, IPcdWebUserRepository
    {
        private PcdUserManager _userManager;
        public PcdUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<PcdUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, User user)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public bool UpdateConfirmationCode(User user)
        {
            var original = Context.Users.Find(user.Id);
            original.EmailConfirmationCode = user.EmailConfirmationCode;

            return Context.SaveChanges() > 0;
        }

        public bool UpdateResetCode(User user)
        {
            var original = Context.Users.Find(user.Id);
            
            original.PasswordResetCode = user.PasswordResetCode;

            return Context.SaveChanges() > 0;
        }

        public bool ResetPassword(string userId, string password)
        {
            var original = Context.Users.Find(userId);

            original.PasswordHash = new PasswordHasher().HashPassword(password);
            original.PasswordResetCode = null;

            return Context.SaveChanges() > 0;
        }

        public bool AddUserToPerson(Person person, User user)
        {
            //var thisPerson = Context.Persons.Find(person.Id);
            var thisPerson = (from p in Context.Persons
                         where (p.Id == person.Id)
                         select p).FirstOrDefault();

            if (thisPerson == null) return false;
            thisPerson.User = user;
            return Context.SaveChanges() > 0;
        }

        public bool ConfirmEmail(string userId , string code)
        {
            var query = (from u in Context.Users
                         where (u.Id == userId) && 
                            (u.EmailConfirmationCode == code)
                         select u).FirstOrDefault();

            if (query == null) return false;
            query.EmailConfirmed = true;
            return true;
        }

        public User GetUserByUsername(string username)
        {
            var query = (from u in Context.Users
                         where (u.UserName == username) 
                         select u).FirstOrDefault();
            return query;
        }

        
    }
}
