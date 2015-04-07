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
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;


namespace DataModel.Data.ApplicationLayer.Services
{
    public class PcdWebUserAppService : IPcdWebUserAppService
    {
        private readonly IPcdWebUserRepository _userRepository;
        private readonly IIdentityMessageService _emailService;

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
        }

        private PcdRoleManager _roleManager;
        public PcdRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.Current.GetOwinContext().Get<PcdRoleManager>();
            }
        }

        private PcdSignInManager _signInManager;
        public PcdSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<PcdSignInManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            return _userRepository.GetAllList().Select(u => new UserDto(u));
        }

        public GetUserByUsernameOutput GetUserByUsername(GetUserByUsernameInput input)
        {
            var user = _userRepository.GetUserByUsername(input.Email);
            if(user != null)
                return new GetUserByUsernameOutput() {User = new UserDto(user)};

            return new GetUserByUsernameOutput() { User = null };
        }

        public UserDto GetActiveUserOrNull(string emailAddress, string password) //TODO: Make this GetUserOrNullInput and GetUserOrNullOutput
        {
            var userEntity = _userRepository.FirstOrDefault(user => user.Email == emailAddress && user.PasswordHash == password && user.EmailConfirmed);
            return new UserDto(userEntity);
        }

        public GetUserOutput GetUser(GetUserInput input)
        {
            var user = _userRepository.Get(input.UserId);
            
            return new GetUserOutput() { User = new UserDto(user) };
        }

        public UpdateUserOutput UpdateUser(UpdateUserInput input)
        {
            var original = UserManager.FindByNameAsync(input.CurrentEmail).Result;

            //Only update phonenumber and email for now
            original.PhoneNumber = input.PhoneNumber;

            if (input.NewEmail != null)
            {
                original.Email = input.NewEmail;
                original.UserName = input.NewEmail;
            }
            var result = UserManager.Update(original);

            return new UpdateUserOutput() {Result = result.Succeeded};
        }

        public async Task<RegisterUserOutput> RegisterUser(RegisterUserInput registerUser)
        {
            User existingUser = null;

            IdentityResult result = null;
            try
            {
                existingUser = _userRepository.FirstOrDefault(u => u.Email == registerUser.EmailAddress);
            }
            catch (Exception ex)
            {
                
                result =
                    new IdentityResult("The server is down, wait an hour or two and if the problem persists call us");
                
            }

            if (result != null)
            {
                await Task.FromResult(0);
                return new RegisterUserOutput() { Result = result };
            }

            if (existingUser != null)
            {
                try
                {
                    if (!existingUser.EmailConfirmed)
                    {
                        //var result = SendConfirmationEmail(existingUser);
                        throw new UserFriendlyException("You registered with this email address before (" +
                                                        registerUser.EmailAddress +
                                                        ")! We re-sent an activation code to your email!");
                    }

                    throw new UserFriendlyException("There is already a user with this email address (" +
                                                    registerUser.EmailAddress + ")! Select another email address!");
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    
                    result = new IdentityResult(ex.Message);
                    
                }

                if (result != null)
                {
                    await Task.FromResult(0);
                    return new RegisterUserOutput() { Result = result };
                }
            }

            List<Person> persons;
            using (var repo = new EmailRepository())
            {
                var app = new EmailAppService(repo);
                persons = app.GetPersonsByEmail(new GetPersonsByEmailInput() { EmailAddress = registerUser.EmailAddress }).EmailPersons
                    .Select(p => p.ConvertToPerson()).ToList();
            }
            
            if (persons.Any())
            {
                await Task.FromResult(0);
                return new RegisterUserOutput() { Result = new IdentityResult("There is already a record for this email, try selecting 'Merge Account' on the home page") };
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
                    },
                    Consignor = new Consignor()
                    {
                        DateAdded = DateTime.Now
                    }
                },

                UserName = registerUser.EmailAddress,
                PhoneNumber = registerUser.Phone,
                Email = registerUser.EmailAddress,
                PasswordHash = registerUser.Password,
            };
            
            userEntity.PasswordHash = new PasswordHasher().HashPassword(userEntity.PasswordHash);
            try
            {
                var createResult = await UserManager.CreateAsync(userEntity);

                //if (result.Succeeded)
                //{
                //    var user = _userRepository.FirstOrDefault(u => u.Email == registerUser.EmailAddress);
                //    await SendConfirmationEmail(user);
                //}

                return new RegisterUserOutput() { Result = createResult };
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<UpdateRegisterUserOutput> UpdateRegisterUser(UpdateRegisterUserInput input)
        {

            var userEntity = new User
            {
                UserName = input.EmailAddress,
                Email = input.EmailAddress,
                PasswordHash = input.Password,
                PhoneNumber = input.Person.PhoneNumbers.CellPhoneNumber ?? input.Person.PhoneNumbers.HomePhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString() 
            };

            input.Person.User = null;          

            userEntity.PasswordHash = new PasswordHasher().HashPassword(userEntity.PasswordHash);
            try
            {
                bool result;
                
                result = _userRepository.AddUserToPerson(input.Person.ConvertToPerson(), userEntity);

                if (result)
                {
                    var user = _userRepository.FirstOrDefault(u => u.Email == input.EmailAddress);
                    await SendConfirmationEmail(user);
                    return new UpdateRegisterUserOutput() { Result = true};
                }

                return new UpdateRegisterUserOutput() { Result = false };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new UpdateRegisterUserOutput() { Result = false };
            }
        }

        public async Task<LoginWithFormOutput> LoginWithForm(LoginWithFormInput input)
        {
            var user = await UserManager.FindByNameAsync(input.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    return new LoginWithFormOutput() { Result = SignInStatus.RequiresVerification };
                }
            }
            
            var result = await SignInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, input.ShouldLockout);
            return new LoginWithFormOutput() {Result = result};
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

        public VerifyResetCodeOutput VerifyResetCode(VerifyResetCodeInput input)
        {
            var user = UserManager.FindByIdAsync(input.UserId);
            if (user.Result != null)
            {

                var resetCode = HttpUtility.UrlDecode(input.ResetCode);
                if (user.Result.PasswordResetCode == resetCode)
                {
                    user.Result.PasswordResetCode = "resetConfirmed";
                    var updateResult = _userRepository.UpdateResetCode(user.Result);
                    return new VerifyResetCodeOutput() {Result = updateResult};
                }
                return new VerifyResetCodeOutput() { Result = false };
            }
            return new VerifyResetCodeOutput() { Result = false };
        }

        public async Task<ChangeForgotPasswordOutput> ChangeForgotPassword(ChangeForgotPasswordInput input)
        {
            var user = await UserManager.FindByNameAsync(input.Email);
            if (user == null)
            {
                return new ChangeForgotPasswordOutput() {Result = false};
            }

            if (user.PasswordResetCode == "resetConfirmed")
            {
                var result = _userRepository.ResetPassword(user.Id, input.Password);
                return new ChangeForgotPasswordOutput() {Result = result};
            }

            return new ChangeForgotPasswordOutput() { Result = false };
        }


        public GetCurrentUserInfoOutput GetCurrentUserInfo(GetCurrentUserInfoInput input)
        {
            return new GetCurrentUserInfoOutput { User = new UserDto(_userRepository.Get(input.UserId.ToString())) };
        }

        public async Task<ChangePasswordOutput> ChangePassword(ChangePasswordInput input)
        {
            var currentUser = UserManager.FindByNameAsync(input.EmailAddress).Result;
            
            var result = await UserManager.ChangePasswordAsync(currentUser.Id, input.CurrentPassword, input.NewPassword);
           
            return new ChangePasswordOutput() {Result = result.Succeeded};
        }

        public async Task<SendPasswordResetLinkOutput> SendPasswordResetLink(SendPasswordResetLinkInput input)
        {
            try
            {
                var user = _userRepository.FirstOrDefault(u => u.Email == input.EmailAddress);
                if (user == null)
                {
                    throw new UserFriendlyException("Can not find this email address in the system.");
                }

                var result = UserManager.GeneratePasswordResetTokenAsync(user.Id);
                user.PasswordResetCode = result.Result;
                var updateSuccessful = _userRepository.Update(user);
                await SendPasswordResetLinkEmail(user);
                return new SendPasswordResetLinkOutput() {Result = result.IsFaulted};
            }
            catch (Exception ex)
            {
                return new SendPasswordResetLinkOutput() {Result = true}; //true == isFaulted
            }
        }

        public async Task<SendConfirmationOutput> SendConfirmation(SendConfirmationInput input)
        {
            var existingUser = _userRepository.FirstOrDefault(u => u.Email == input.EmailAddress);

            //try
            //{
            //    //Send confirmation
            //    await SendConfirmationEmail(existingUser);
            //    return new SendConfirmationOutput{Result = true};
            //}
            //catch (Exception ex)
            //{
            //    return new SendConfirmationOutput{Result = false};
            //}

            //Send confirmation
            await SendConfirmationEmail(existingUser);
            return new SendConfirmationOutput { Result = true };

        }       

        #region Private methods

        private async Task SendConfirmationEmail(User user)
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
                    <p>Click this <a href=""http://playcreatediscover.com/api/Account/ConfirmEmail?userId={USER_ID}&confirmationCode={CONFIRMATION_CODE}"">link</a> to activate your account</p>
                    <p>&nbsp;</p>
                    <p><a href=""http://www.playcreatediscover.com"">http://www.playcreatediscover.com</a></p>
                </body>
                </html>");

            mailBuilder.Replace("{TEXT_HTML_TITLE}", "Email confirmation for Play Create Discover");
            mailBuilder.Replace("{TEXT_WELCOME}", "Welcome to PlayCreateDiscover.com!");
            mailBuilder.Replace("{TEXT_DESCRIPTION}",
                "Click the link below to confirm your email address and login to the PlayCreateDiscover.com");
            mailBuilder.Replace("{USER_ID}", user.Id);
            mailBuilder.Replace("{CONFIRMATION_CODE}", encoded);

            mail.Body = mailBuilder.ToString();
            mail.BodyEncoding = Encoding.UTF8;

            var message = new IdentityMessage
            {
                Body = mail.Body,
                Subject = "Email confirmation for Play Create Discover",
                Destination = user.Email
            };

            await _emailService.SendAsync(message);
        }

        private async Task SendPasswordResetLinkEmail(User user)
        {
            var encoded = user.PasswordResetCode.Replace("+", "%2B");
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
                    <p><a href=""http://www.playcreatediscover.com/api/Account/ResetPassword?UserId={USER_ID}&ResetCode={RESET_CODE}"">http://localhost:61754/api/Account/ResetPassword?UserId={USER_ID}&amp;ResetCode={RESET_CODE}</a></p>
                    <p>&nbsp;</p>
                    <p><a href=""http://www.playcreatediscover.com"">http://www.playcreatediscover.com</a></p>
                </body>
                </html>");

            mailBuilder.Replace("{TEXT_HTML_TITLE}", "Password reset for Play Create Discover");
            mailBuilder.Replace("{TEXT_WELCOME}", "Reset your password on Play Create Discover!");
            mailBuilder.Replace("{TEXT_DESCRIPTION}", "Click the link below to reset your password on the PlayCreateDiscover.com");
            mailBuilder.Replace("{USER_ID}", user.Id);
            mailBuilder.Replace("{RESET_CODE}", encoded);

            mail.Body = mailBuilder.ToString();
            mail.BodyEncoding = Encoding.UTF8;

            var message = new IdentityMessage
            {
                Body = mail.Body,
                Subject = "Password reset for Play Create Discover",
                Destination = user.Email
            };

            await _emailService.SendAsync(message);
        }

        #endregion
    }
    
}
