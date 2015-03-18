using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Abp.UI;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Identity;
using DataModel.Data.ApplicationLayer.Utils.Email;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class PcdWebUserAppService : IPcdWebUserAppService
    {
        private readonly IPcdWebUserRepository _userRepository;
        private readonly IIdentityMessageService _emailService;
        //private readonly UserManager<User> _userManager;
        //private readonly IFriendshipRepository _friendshipRepository;

        public PcdWebUserAppService()
        {
            _userRepository = new PcdWebUserRepository();
            //_friendshipRepository = friendshipRepository;
            _emailService = new EmailService();
            //_userManager = new PcdUserManager();
        }

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

        private PcdRoleManager _roleManager;
        public PcdRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().Get<PcdRoleManager>();
            }
            private set { _roleManager = value; }
        }

        private PcdSignInManager _signInManager;
        public PcdSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<PcdSignInManager>();
            }
            private set { _signInManager = value; }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        //public GetUserProfileOutput GetUserProfile(GetUserProfileInput input)
        //{
        //    var currentUser = _userRepository.Load(IdentityUser.CurrentUserId.Value);

        //    var profileUser = _userRepository.Get(input.UserId);
        //    if (profileUser == null)
        //    {
        //        throw new UserFriendlyException("Can not find the user!");
        //    }

        //    if (profileUser.Id != currentUser.Id)
        //    {
        //        var friendship = _friendshipRepository.GetOrNull(currentUser.Id, input.UserId);
        //        if (friendship == null)
        //        {
        //            return new GetUserProfileOutput { CanNotSeeTheProfile = true, User = profileUser.MapTo<UserDto>() }; //TODO: Is it true to send user informations?
        //        }

        //        if (friendship.Status != FriendshipStatus.Accepted)
        //        {
        //            return new GetUserProfileOutput { CanNotSeeTheProfile = true, SentFriendshipRequest = true, User = profileUser.MapTo<UserDto>() };
        //        }
        //    }

        //    return new GetUserProfileOutput { User = profileUser.MapTo<UserDto>() };
        //}

        //public ChangeProfileImageOutput ChangeProfileImage(ChangeProfileImageInput input)
        //{
        //    var currentUser = _userRepository.Get(IdentityUser.CurrentUserId.Value); //TODO: test Load method
        //    var oldFileName = currentUser.ProfileImage;

        //    currentUser.ProfileImage = input.FileName;

        //    return new ChangeProfileImageOutput() { OldFileName = oldFileName };
        //}


        public IEnumerable<UserDto> GetAllUsers()
        {
            return _userRepository.GetAllList().Select(u => new UserDto(u));
        }

        public UserDto GetActiveUserOrNull(string emailAddress, string password) //TODO: Make this GetUserOrNullInput and GetUserOrNullOutput
        {
            var userEntity = _userRepository.FirstOrDefault(user => user.Email == emailAddress && user.PasswordHash == password && user.EmailConfirmed);
            return new UserDto(userEntity);
        }

        public GetUserOutput GetUser(GetUserInput input)
        {
            var user = _userRepository.Get(input.UserId.ToString());
            return new GetUserOutput(new UserDto(user));
        }

        //Todo - make mergeUser - where it creates the user and merges an existing person
        public RegisterUserOutput RegisterUser(RegisterUserInput registerUser)
        {

            var existingUser = _userRepository.FirstOrDefault(u => u.Email == registerUser.EmailAddress);

            if (existingUser != null)
            {
                if (!existingUser.EmailConfirmed)
                {
                    SendConfirmationEmail(existingUser);
                    throw new UserFriendlyException("You registered with this email address before (" + registerUser.EmailAddress + ")! We re-sent an activation code to your email!");
                }

                throw new UserFriendlyException("There is already a user with this email address (" + registerUser.EmailAddress + ")! Select another email address!");
            }

            var userEntity = new User
            {

                PersonUser = new Person
                {
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    EmailAddresses = new List<Email>
                    {
                        new Email
                        {
                            EmailAddress = registerUser.EmailAddress
                        }
                    },
                    PhoneNumbers = new PhoneNumber
                    {
                        HomePhoneNumber = registerUser.Phone
                    }
                },

                UserName = registerUser.EmailAddress,
                PhoneNumber = registerUser.Phone,
                Email = registerUser.EmailAddress,
                PasswordHash = registerUser.Password,
                //UserName = registerUser.UserName

            };

            //var userEntity = registerUser;
            
            userEntity.PasswordHash = new PasswordHasher().HashPassword(userEntity.PasswordHash);
            try
            {
                var result = UserManager.CreateAsync(userEntity);

                if (result.Result.Succeeded)
                {
                    
                    SendConfirmationEmail(userEntity);
                }

                return new RegisterUserOutput() {Result = result.Result};
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public UpdateRegisterUserOutput UpdateRegisterUser(UpdateRegisterUserInput input)
        {
            //var userEntity = new User
            //{
            //    //PersonUser = input.Person.ConvertToPerson(),
            //    PersonUser = input.Person.ConvertToPerson(),
            //    UserName = input.EmailAddress,
            //    Email = input.EmailAddress,
            //    PasswordHash = input.Password,
            //};

            var userEntity = new User
            {
                //PersonUser = input.Person.ConvertToPerson(),
                //PersonUser = input.Person.ConvertToPerson(),
                UserName = input.EmailAddress,
                Email = input.EmailAddress,
                PasswordHash = input.Password,
                SecurityStamp = Guid.NewGuid().ToString() //Added since we aren't using createAsync
            };

            input.Person.User = null;
            //userEntity.PersonUser.Consignor.Consignor_Person.User = null;
            

            userEntity.PasswordHash = new PasswordHasher().HashPassword(userEntity.PasswordHash);
            try
            {
                bool result;
                try
                {                 
                    result = _userRepository.AddUserToPerson(input.Person.ConvertToPerson(), userEntity);
                }
                catch (Exception ex)
                {
                    result = false;
                }

                if (result)
                {
                    var user = _userRepository.FirstOrDefault(u => u.Email == input.EmailAddress);
                    SendConfirmationEmail(user);
                }

                return new UpdateRegisterUserOutput() { Result = result };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public RegisterExternalUserOutput RegisterExternalUser(RegisterExternalUserInput input)
        {

            return new RegisterExternalUserOutput() {Result = _userManager.CreateAsync(input.User).Result};

        }

        public ConfirmEmailOutput ConfirmEmail(ConfirmEmailInput input)
        {
            //var user = _userRepository.Get(input.UserId.ToString());
            var code = HttpUtility.UrlDecode(input.ConfirmationCode);
            if (code != null)
            {
                //code = code.Replace("%99", "+");
                var result = UserManager.ConfirmEmailAsync(input.UserId, code);
                //var result = _userRepository.ConfirmEmail(input.UserId, code);
                return new ConfirmEmailOutput() { Result = result.Result.Succeeded };
            }
            return new ConfirmEmailOutput(){Result = false};
        }

        public GetCurrentUserInfoOutput GetCurrentUserInfo(GetCurrentUserInfoInput input)
        {
            return new GetCurrentUserInfoOutput { User = new UserDto(_userRepository.Get(input.UserId.ToString())) };
        }

        public void ChangePassword(ChangePasswordInput input)
        {
            var currentUser = _userRepository.Get(input.CurrentUserId.ToString());
            if (currentUser.PasswordHash != input.CurrentPassword)
            {
                throw new UserFriendlyException("Current password is invalid!");
            }

            currentUser.PasswordHash = input.NewPassword;
            currentUser.PasswordHash = new PasswordHasher().HashPassword(currentUser.PasswordHash);
        }

        public void SendPasswordResetLink(SendPasswordResetLinkInput input)
        {
            var user = _userRepository.FirstOrDefault(u => u.Email == input.EmailAddress);
            if (user == null)
            {
                throw new UserFriendlyException("Can not find this email address in the system.");
            }

            user.PasswordResetCode = UserManager.GeneratePasswordResetTokenAsync(input.UserId.ToString()).ToString();
            SendPasswordResetLinkEmail(user);
        }

        public void ResetPassword(ResetPasswordInput input)
        {
            var user = _userRepository.Get(input.UserId.ToString());
            if (string.IsNullOrWhiteSpace(user.PasswordResetCode))
            {
                throw new UserFriendlyException("You can not reset your password. You must first send a reset password link to your email.");
            }

            if (input.PasswordResetCode != user.PasswordResetCode)
            {
                throw new UserFriendlyException("Password reset code is invalid!");
            }

            user.PasswordHash = input.Password;
            user.PasswordResetCode = null;
        }

        public SendConfirmationOutput SendConfirmation(SendConfirmationInput input)
        {
            var existingUser = _userRepository.FirstOrDefault(u => u.Email == input.EmailAddress);

            try
            {
                //Send confirmation
                SendConfirmationEmail(existingUser);
                return new SendConfirmationOutput{Result = true};
            }
            catch (Exception ex)
            {
                return new SendConfirmationOutput{Result = false};
            }
        
        }

        

        #region Private methods

        private void SendConfirmationEmail(User user)
        {
            var code = UserManager.GenerateEmailConfirmationTokenAsync(user.Id);           
            user.EmailConfirmationCode = code.Result;
            _userRepository.UpdateConfirmationCode(user);

            var encoded = code.Result.Replace("+", "%2B");
            encoded = HttpUtility.UrlEncode(encoded);

            var mail = new MailMessage {IsBodyHtml = true, SubjectEncoding = Encoding.UTF8};

            var mailBuilder = new StringBuilder();
            mailBuilder.Append(
                @"<!DOCTYPE html>
                <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
                <head>
                    <meta charset=""utf-8"" />
                    <title>{TEXT_HTML_TITLE}</title>
                    <style>
                        body {
                            font-family: Verdana, Geneva, 'DejaVu Sans', sans-serif;
                            font-size: 12px;
                        }
                    </style>
                </head>
                <body>
                    <h3>{TEXT_WELCOME}</h3>
                    <p>{TEXT_DESCRIPTION}</p>
                    <p>&nbsp;</p>
                    <p><a href=""http://localhost:61754/api/Account/ConfirmEmail?userId={USER_ID}&confirmationCode={CONFIRMATION_CODE}"">http://localhost:61754/api/Account/ConfirmEmail?userId={USER_ID}&amp;confirmationCode={CONFIRMATION_CODE}</a></p>
                    <p>&nbsp;</p>
                    <p><a href=""http://www.playcreatediscover.com"">http://www.playcreatediscover.com</a></p>
                </body>
                </html>");

            mailBuilder.Replace("{TEXT_HTML_TITLE}", "Email confirmation for PcdWeb");
            mailBuilder.Replace("{TEXT_WELCOME}", "Welcome to PcdWeb.com!");
            mailBuilder.Replace("{TEXT_DESCRIPTION}",
                "Click the link below to confirm your email address and login to the PcdWeb.com");
            mailBuilder.Replace("{USER_ID}", user.Id);
            mailBuilder.Replace("{CONFIRMATION_CODE}", encoded);

            mail.Body = mailBuilder.ToString();
            mail.BodyEncoding = Encoding.UTF8;

            var message = new IdentityMessage
            {
                Body = mail.Body,
                Subject = "Email confirmation for PcdWeb",
                Destination = user.Email
            };

            var result = _emailService.SendAsync(message);
        }

        private void SendPasswordResetLinkEmail(User user)
        {
            var mail = new MailMessage {IsBodyHtml = true, SubjectEncoding = Encoding.UTF8};

            var mailBuilder = new StringBuilder();
            mailBuilder.Append(
                @"<!DOCTYPE html>
                <html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
                <head>
                    <meta charset=""utf-8"" />
                    <title>{TEXT_HTML_TITLE}</title>
                    <style>
                        body {
                            font-family: Verdana, Geneva, 'DejaVu Sans', sans-serif;
                            font-size: 12px;
                        }
                    </style>
                </head>
                <body>
                    <h3>{TEXT_WELCOME}</h3>
                    <p>{TEXT_DESCRIPTION}</p>
                    <p>&nbsp;</p>
                    <p><a href=""http://www.taskever.com/Account/ResetPassword?UserId={USER_ID}&ResetCode={RESET_CODE}"">http://www.taskever.com/Account/ResetPassword?UserId={USER_ID}&amp;ResetCode={RESET_CODE}</a></p>
                    <p>&nbsp;</p>
                    <p><a href=""http://www.taskever.com"">http://www.taskever.com</a></p>
                </body>
                </html>");

            mailBuilder.Replace("{TEXT_HTML_TITLE}", "Password reset for PcdWeb");
            mailBuilder.Replace("{TEXT_WELCOME}", "Reset your password on PcdWeb!");
            mailBuilder.Replace("{TEXT_DESCRIPTION}", "Click the link below to reset your password on the PcdWeb.com");
            mailBuilder.Replace("{USER_ID}", user.Id);
            mailBuilder.Replace("{RESET_CODE}", user.PasswordResetCode);

            mail.Body = mailBuilder.ToString();
            mail.BodyEncoding = Encoding.UTF8;

            var message = new IdentityMessage
            {
                Body = mail.Body,
                Subject = "Password reset for PcdWeb",
                Destination = user.Email
            };

            _emailService.SendAsync(message);
        }

        #endregion
    }
    
}
