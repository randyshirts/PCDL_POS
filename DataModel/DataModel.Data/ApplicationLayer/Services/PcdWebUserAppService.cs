using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Utils.Email;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataModel.Data.ApplicationLayer.Services
{
    public class PcdWebUserAppService : IPcdWebUserAppService
    {
        private readonly IPcdWebUserRepository _userRepository;
        private readonly IIdentityMessageService _emailService;
        private readonly UserManager<User> _userManager;
        //private readonly IFriendshipRepository _friendshipRepository;

        public PcdWebUserAppService(UserManager<User> userManager)
        {
            _userRepository = new PcdWebUserRepository();
            //_friendshipRepository = friendshipRepository;
            _emailService = new EmailService();
            _userManager = userManager;
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

        public void RegisterUser(RegisterUserInput registerUser)
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
                    FirstName = registerUser.Name,
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

                PhoneNumber = registerUser.Phone,
                Email = registerUser.EmailAddress,
                PasswordHash = registerUser.Password,
                //UserName = registerUser.UserName

            };

            //var userEntity = registerUser;
            userEntity.PasswordHash = new PasswordHasher().HashPassword(userEntity.PasswordHash);
            userEntity.EmailConfirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(userEntity.Id).Result;
            _userRepository.Insert(userEntity);
            SendConfirmationEmail(userEntity);
        }

        public async Task<IdentityResult> ConfirmEmail(ConfirmEmailInput input)
        {
            var user = _userRepository.Get(input.UserId.ToString());
            var result = await _userManager.ConfirmEmailAsync(input.UserId.ToString(), input.ConfirmationCode);
            return result;
        }

        public GetCurrentUserInfoOutput GetCurrentUserInfo(GetCurrentUserInfoInput input)
        {
            //TODO: Use GetUser?
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

            user.PasswordResetCode = _userManager.GeneratePasswordResetTokenAsync(input.UserId.ToString()).ToString();
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

        public GenerateUserIdentityAsyncOutput GenerateUserIdentityAsync(GenerateUserIdentityAsyncInput input)
        {
            return new GenerateUserIdentityAsyncOutput
            {
                Task = _userRepository.GenerateUserIdentityAsync(_userManager, input.User.ConvertToUser())
            };
        }


        public GenerateUserIdentityAsyncOutput GenerateUserOauthIdentityAsync(GenerateUserIdentityAsyncInput input)
        {
            return new GenerateUserIdentityAsyncOutput
            {
                Task = _userRepository.GenerateUserIdentityAsync(_userManager, input.User.ConvertToUser())
            };
        }

        #region Private methods

        private void SendConfirmationEmail(User user)
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
                    <p><a href=""http://www.playcreatediscover.com/Account/ConfirmEmail?UserId={USER_ID}&ConfirmationCode={CONFIRMATION_CODE}"">http://www.playcreatediscover.com/Account/ConfirmEmail?UserId={USER_ID}&amp;ConfirmationCode={CONFIRMATION_CODE}</a></p>
                    <p>&nbsp;</p>
                    <p><a href=""http://www.playcreatediscover.com"">http://www.playcreatediscover.com</a></p>
                </body>
                </html>");

            mailBuilder.Replace("{TEXT_HTML_TITLE}", "Email confirmation for PcdWeb");
            mailBuilder.Replace("{TEXT_WELCOME}", "Welcome to PcdWeb.com!");
            mailBuilder.Replace("{TEXT_DESCRIPTION}",
                "Click the link below to confirm your email address and login to the PcdWeb.com");
            mailBuilder.Replace("{USER_ID}", user.Id.ToString());
            mailBuilder.Replace("{CONFIRMATION_CODE}", user.EmailConfirmationCode);

            mail.Body = mailBuilder.ToString();
            mail.BodyEncoding = Encoding.UTF8;

            var message = new IdentityMessage
            {
                Body = mail.Body,
                Subject = "Email confirmation for PcdWeb",
                Destination = user.Email
            };

            _emailService.SendAsync(message);
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
            mailBuilder.Replace("{USER_ID}", user.Id.ToString());
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
