using System;
using DataModel.Data.ApplicationLayer.Utils.Email;
using DataModel.Data.DataLayer;
using DataModel.Data.DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace DataModel.Data.ApplicationLayer.Identity
{
    public class PcdUserManager : UserManager<User>
    {

        public PcdUserManager(IUserStore<User> store)
            : base(store)
        {
        }

        public static PcdUserManager Create(IdentityFactoryOptions<PcdUserManager> options, IOwinContext context)
        {
            
            var manager = new PcdUserManager(new UserStore<User>(context.Get<DataContext>())) 
           {
               // Configure validation logic for passwords 
               PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                },

                // Configure user lockout defaults
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5),
                MaxFailedAccessAttemptsBeforeLockout = 5

            };

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            //manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User>
            //{
            //    MessageFormat = "Your security code is: {0}"
            //});

            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();

            return manager;
        }



    }

}
