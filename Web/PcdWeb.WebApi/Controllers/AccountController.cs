using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Abp.Modules.Core.Mvc.Models;
using Abp.UI;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using PcdWeb.Logging;
using PcdWeb.Models;
using PcdWeb.Models.AccountModels;
using PcdWeb.Models.ItemModels;


namespace PcdWeb.Controllers
{
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {

        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        //private AuthRepository _repo = null;
        private readonly IPcdUserAppService _userAppService;

        public AccountController()
        {
            _userAppService = new PcdUserAppService();
        }

        // POST api/Account/Register
        [System.Web.Http.AllowAnonymous]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterUserInput input)
        {
            //ApiLog.Instance.Trace("Register account controller started");
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Your form is invalid!");
            }

            if (!ModelState.IsValid)
            {
                return Ok("Invalid input");
            }

            if (input.FirstName.Contains(" ")) { input.FirstName.Replace(" ", String.Empty); }
            if (input.LastName.Contains(" ")) { input.LastName.Replace(" ", String.Empty); }
            //ApiLog.Instance.Trace("Register account controller valid");

            //define output
            var output = new RegisterApiOutputModel();


            ////var recaptchaHelper = this.GetRecaptchaVerificationHelper(this.ToString(), recaptchaPrivateKey);
            //var recaptchaHelper = this.GetRecaptchaVerificationHelper();
            //if (String.IsNullOrEmpty(recaptchaHelper.Response))
            //{
            //    throw new UserFriendlyException("Captcha answer cannot be empty.");
            //}

            //var recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
            //if (recaptchaResult != RecaptchaVerificationResult.Success)
            //{
            //    throw new UserFriendlyException("Incorrect captcha answer.");
            //} 

            //Todo: var cts = new CancellationTokenSource(); //send cancellation token to RegisterUser instead of "await Task.FromResult(0);" use cancel token 
            //http://jeremybytes.blogspot.com/2015/01/task-and-await-basic-cancellation.html
            var result = await _userAppService.RegisterUser(input);
            //if (result == null) return BadRequest("Registration Failed - Try Again");

            if (result.Result.Succeeded)
            {
                await _userAppService.SendConfirmation(new SendConfirmationInput() { EmailAddress = input.EmailAddress });
                output.message = "success";
                return Ok(output);

            }

            if (result.Result.Errors.Any())
            {

                var alreadyRegisteredError =
                    result.Result.Errors.FirstOrDefault(e => e.StartsWith("You registered with"));
                if (alreadyRegisteredError != null)
                {
                    await _userAppService.SendConfirmation(new SendConfirmationInput() { EmailAddress = input.EmailAddress });
                    output.message = alreadyRegisteredError;
                }
                else
                    output.message = result.Result.Errors.FirstOrDefault();
            }

            return Ok(output);
        }

        // POST api/Account/Register
        [System.Web.Http.AllowAnonymous]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("UpdateRegistration")]
        public async Task<IHttpActionResult> UpdateRegistration(UpdateRegisterUserInput input)
        {

            //define output
            var output = new RegisterApiOutputModel();

            //Get matching person
            var persons = new List<Person>();
            try
            {
                persons = GetPersonsFromEmail(new GetPersonsByEmailInput() { EmailAddress = input.EmailAddress });
            }
            catch (Exception ex)
            {
                output.message = "The server is down, wait an hour or two and if the problem persists call us";
                //output.message = ex.Message + ex.InnerException.Message;
            }


            input.Person = new PersonDto(persons.FirstOrDefault(p => (p.FirstName == input.FirstName) &&
                                                        (p.LastName == input.LastName)));

            var result = await _userAppService.UpdateRegisterUser(input);

            if (result.Result)
            {
                output.message = "success";
                return Ok(output);
            }

            output.message = "Registration Failed";
            return Ok(output);
        }

        //[System.Web.Http.AllowAnonymous]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("ConfirmEmail")]
        public HttpResponseMessage ConfirmEmail(string userId, string confirmationCode)
        {
            var input = new ConfirmEmailInput { ConfirmationCode = confirmationCode, UserId = userId };

            if (input.UserId == null || input.ConfirmationCode == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var result = _userAppService.ConfirmEmail(input);

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri("http://test.playcreatediscover.com/#/emailConfirmed");
            return response;

        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("SendConfirmation")]
        public bool SendConfirmation(SendConfirmationInput input)
        {

            if (input.EmailAddress == null)
            {
                return false;
            }

            var result = _userAppService.SendConfirmation(input);

            return result.Result.Result;

        }

        // POST api/Account/getUsers
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [ValidateAntiForgeryToken]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("GetUsers")]
        public IHttpActionResult GetUsers(GetPersonsByEmailInput input)
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Your form is invalid!");
            }

            if (!ModelState.IsValid)
            {
                return Ok("Invalid input");
            }

            var output = new GetUsersApiOutput();

            //Check for existing user
            var user = new GetUserByUsernameOutput();
            try
            {
                user = _userAppService.GetUserByUsername(new GetUserByUsernameInput() { Email = input.EmailAddress });
            }
            catch (Exception ex)
            {
                output.message = "The server is down, wait an hour or two and if the problem persists call us";
                //output.message = ex.Message + ex.InnerException.Message;
            }

            if (user.User != null)
                output.message = "Email already registered";

            if (output.message != null)
                return Ok(output);

            //Find persons if no users exist with current email
            List<Person> existingUsers = GetPersonsFromEmail(input);

            if (existingUsers.Any())
            {
                output.users = existingUsers.Select(u => new ExistingUserInfo(u));
                output.message = "success";
                return Ok(output);
            }

            output.message = "No Matches Found";
            return Ok(output);
        }

        // POST api/Account/getUserInfo
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.Route("GetUserInfo")]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Authorize]
        public IHttpActionResult GetUserInfo(GetPersonsByEmailInput input)
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Your form is invalid!");
            }

            if (!ModelState.IsValid)
            {
                return Ok("Invalid input");
            }

            var person = new Person();
            var output = new UserInfoOutput();
            try
            {
                //Find persons person with current email
                List<Person> persons = GetPersonsFromEmail(input);
                person = persons.FirstOrDefault();

                output = new UserInfoOutput(person);
            }
            catch (Exception ex)
            {
                output.Message = "The server is down, wait an hour or two and if the problem persists call us";
                //output.Message = ex.Message + ex.InnerException.Message;
                return Ok(output.Message);
            }

            if (person != null)
            {
                output.Message = "success";
                return Ok(output);
            }

            output.Message = "No Matches Found";
            return Ok(output);
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("Login")]
        public virtual async Task<IHttpActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Your form is invalid!");
            }

            if (!ModelState.IsValid)
            {
                return Ok("Invalid input");
            }

            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var input = new LoginWithFormInput()
            {
                Email = loginModel.UserName,
                Password = loginModel.Password,
                RememberMe = loginModel.UseRefreshToken,
                ShouldLockout = false
            };
            var result = await _userAppService.LoginWithForm(input);
            switch (result.Result)
            {
                case SignInStatus.Success:
                    return Ok("success");
                case SignInStatus.LockedOut:
                    return BadRequest("lockout");
                case SignInStatus.RequiresVerification:
                    {
                        await _userAppService.SendConfirmation(new SendConfirmationInput() { EmailAddress = loginModel.UserName });
                        return BadRequest("Email not confirmed. We sent a new code to your email address");
                    }
                case SignInStatus.Failure:
                    return BadRequest("Invalid email address or password");
                default:
                    return BadRequest("Invalid login attempt");
            }

        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("LoginTimeout")]
        public IHttpActionResult LoginTimeout(string ReturnUrl)
        {
            var output = new AddItemApiOutput() { Message = "Session Timed Out" };
            return Ok(output);
        }


        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("RecoverPassword")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IHttpActionResult> RecoverPassword(SendPasswordResetLinkInput input)
        {
            var result = await _userAppService.SendPasswordResetLink(input);
            if (!result.Result)
                return Ok("success");
            else
                return
                    BadRequest("Could not send new password to the specified email. Please contact Play Create Discover");

        }

        // GET: /Account/ResetPassword
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.Route("ResetPassword")]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage ResetPassword(string userId, string resetCode)
        {
            //Check code and if successful set reset code to resetConfirmed        
            var result =
                _userAppService.VerifyResetCode(new VerifyResetCodeInput() { UserId = userId, ResetCode = resetCode });

            if (!result.Result) return Request.CreateResponse(HttpStatusCode.BadRequest);

            //Navigate to reset password page
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri("http://test.playcreatediscover.com/#/resetPassword");
            return resetCode == null ? Request.CreateResponse(HttpStatusCode.BadRequest) : response;
        }

        // POST: /Account/ResetPassword
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordModel input)
        {
            if (!ModelState.IsValid)
            {
                return Ok("Invalid input");
            }

            var result =
                await _userAppService.ChangeForgotPassword(new ChangeForgotPasswordInput()
                {
                    Password = input.Password,
                    Email = input.EmailAddress
                });

            if (result.Result)
            {
                return Ok("success");
            }
            return Ok("Password reset failed");
        }

        // GET: /Account/ChangePassword
        [System.Web.Http.Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordInput input)
        {
            try
            {
                await _userAppService.ChangePassword(input);
                return Ok("success");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // GET: /Account/ChangeEmail
        [System.Web.Http.Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.Route("ChangeEmail")]
        [ValidateAntiForgeryToken]
        public IHttpActionResult ChangeEmail(ChangeEmailModel input)
        {
            //Check if new email already has a person or user associated with it
            //Get matching person
            var persons = new List<Person>();
            var user = new User();
            try
            {
                persons = GetPersonsFromEmail(new GetPersonsByEmailInput() { EmailAddress = input.NewEmail });
                var userDto = _userAppService.GetUserByUsername(new GetUserByUsernameInput() { Email = input.NewEmail }).User;
                user = userDto != null ? userDto.ConvertToUser() : null;
            }
            catch (Exception ex)
            {
                return Ok("The server is down, wait an hour or two and if the problem persists call us");
                //return Ok(ex.Message + ex.InnerException.Message);
            }

            if (persons.Any() || (user != null))
            {
                return Ok("Email already associated with an account in the system. Contact the store for help");
            }

            //Get record for current person
            var person = GetPersonsFromEmail(new GetPersonsByEmailInput() { EmailAddress = input.CurrentEmail }).FirstOrDefault();
            if (person == null)
                return Ok("No record exists for the current email");

            //Update person 
            using (var repo = new PersonRepository())
            {
                var app = new PersonAppService(repo);

                //Change email address and update person
                person.EmailAddresses.FirstOrDefault().EmailAddress = input.NewEmail;
                try
                {
                    app.UpdatePerson(new UpdatePersonInput() { PersonDto = new PersonDto(person) });
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            }


            //Change Email and Username for user
            try
            {
                var updateInput = new UpdateUserInput()
                {
                    CurrentEmail = input.CurrentEmail,
                    NewEmail = input.NewEmail,
                    PhoneNumber = null
                };
                _userAppService.UpdateUser(updateInput);

                return Ok("success");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("UpdatePerson")]
        public IHttpActionResult UpdatePerson(UserInfoOutput input)
        {
            if (!ModelState.IsValid)
            {
                return Ok("Invalid input");
            }

            if (input.FirstName.Contains(" ")) { input.FirstName.Replace(" ", String.Empty); }
            if (input.LastName.Contains(" ")) { input.LastName.Replace(" ", String.Empty); }

            Person person;
            try
            {
                //Find persons person with current email
                List<Person> persons = GetPersonsFromEmail(new GetPersonsByEmailInput() { EmailAddress = input.EmailAddress });
                person = persons.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return Ok("The server is down. Please wait an hour or two and contact us if the problem persists");
                //return Ok(ex.Message + ex.InnerException.Message);
            }

            if (person == null)
            {
                return Ok("Could not find person to update");
            }


            //Find user info
            var updateInput = new UpdateUserInput()
            {
                CurrentEmail = input.EmailAddress,
                NewEmail = null,
                PhoneNumber = input.CellPhoneNumber ?? input.HomePhoneNumber ?? input.AltPhoneNumber
            };
            //Update user info
            var userResult = _userAppService.UpdateUser(updateInput);

            if (!userResult.Result)
                return Ok("Error updating user");

            bool result;

            using (var repo = new PersonRepository())
            {
                var app = new PersonAppService(repo);

                //update person
                person.EmailAddresses.FirstOrDefault().EmailAddress = input.EmailAddress;
                if (person.MailingAddresses.FirstOrDefault() != null)
                {
                    person.MailingAddresses.FirstOrDefault().MailingAddress1 = input.MailingAddress;
                    person.MailingAddresses.FirstOrDefault().MailingAddress2 = input.MailingAddress2;
                    person.MailingAddresses.FirstOrDefault().City = input.City;
                    person.MailingAddresses.FirstOrDefault().ZipCode = input.ZipCode;
                    person.MailingAddresses.FirstOrDefault().State = input.State;
                }
                else
                {
                    var address = new MailingAddress()
                    {
                        City = input.City,
                        ZipCode = input.ZipCode,
                        State = input.State,
                        MailingAddress1 = input.MailingAddress,
                        MailingAddress2 = input.MailingAddress2
                    };
                    person.MailingAddresses.Add(address);
                }
                person.PhoneNumbers.CellPhoneNumber = input.CellPhoneNumber;
                person.PhoneNumbers.HomePhoneNumber = input.HomePhoneNumber;
                person.PhoneNumbers.AltPhoneNumber = input.AltPhoneNumber;
                person.FirstName = input.FirstName;
                person.LastName = input.LastName;

                try
                {
                    app.UpdatePerson(new UpdatePersonInput() { PersonDto = new PersonDto(person) });
                    result = true;
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            }

            if (result)
                return Ok("success");

            return Ok("Error updating record");

        }



        //// GET api/Account/ExternalLogin
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        //[AllowAnonymous]
        //[Route("ExternalLogin", Name = "ExternalLogin")]
        //public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        //{
        //    string redirectUri = string.Empty;

        //    if (error != null)
        //    {
        //        return BadRequest(Uri.EscapeDataString(error));
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return new ChallengeResult(provider, this);
        //    }

        //    var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

        //    if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
        //    {
        //        return BadRequest(redirectUriValidationResult);
        //    }

        //    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        //    if (externalLogin == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (externalLogin.LoginProvider != provider)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        return new ChallengeResult(provider, this);
        //    }

        //    IdentityUser user = await _userAppService.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

        //    bool hasRegistered = user != null;

        //    redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
        //                                    redirectUri,
        //                                    externalLogin.ExternalAccessToken,
        //                                    externalLogin.LoginProvider,
        //                                    hasRegistered.ToString(),
        //                                    externalLogin.UserName);

        //    return Redirect(redirectUri);

        //}

        //// POST api/Account/RegisterExternal
        //[AllowAnonymous]
        //[Route("RegisterExternal")]
        //public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
        //    if (verifiedAccessToken == null)
        //    {
        //        return BadRequest("Invalid Provider or External Access Token");
        //    }

        //    IdentityUser user = await _userAppService.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

        //    bool hasRegistered = user != null;

        //    if (hasRegistered)
        //    {
        //        return BadRequest("External user is already registered");
        //    }

        //    var input = new RegisterExternalUserInput()
        //    {
        //        User = new User()
        //        {
        //           AccessFailedCount = user.AccessFailedCount,
        //           //Claims = user.Claims,
        //           Email = user.Email,
        //           EmailConfirmed = user.EmailConfirmed,
        //           Id = user.Id,
        //           LockoutEnabled = user.LockoutEnabled,
        //           LockoutEndDateUtc = user.LockoutEndDateUtc,
        //           //Logins = user.Logins,
        //           PasswordHash = user.PasswordHash,
        //           PhoneNumber = user.PhoneNumber,
        //           PhoneNumberConfirmed = user.PhoneNumberConfirmed,
        //           //Roles = user.Roles,
        //           SecurityStamp = user.SecurityStamp,
        //           TwoFactorEnabled = user.TwoFactorEnabled,
        //           UserName = user.UserName
        //        }   //Todo: Get PersonUser info if there is any
        //    };

        //    IdentityResult result = _userAppService.RegisterExternalUser(input).Result;
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    var info = new ExternalLoginInfo()
        //    {
        //        DefaultUserName = model.UserName,
        //        Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
        //    };

        //    result = await _userAppService.AddLoginAsync(user.Id, info.Login);
        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    //generate access token response
        //    var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName);

        //    return Ok(accessTokenResponse);
        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("ObtainLocalAccessToken")]
        //public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        //{

        //    if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
        //    {
        //        return BadRequest("Provider or external access token is not sent");
        //    }

        //    var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
        //    if (verifiedAccessToken == null)
        //    {
        //        return BadRequest("Invalid Provider or External Access Token");
        //    }

        //    IdentityUser user = await _userAppService.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

        //    bool hasRegistered = user != null;

        //    if (!hasRegistered)
        //    {
        //        return BadRequest("External user is not registered");
        //    }

        //    //generate access token response
        //    var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

        //    return Ok(accessTokenResponse);

        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _userAppService.Dispose();
        //    }

        //    base.Dispose(disposing);
        //}

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            List<string> errors = new List<string>();

            if (result == null)
            {
                return InternalServerError();
            }

            //if (!result.Succeeded)
            //{
            //    if (result.Errors != null)
            //    {
            //        int count = 0;
            //        foreach (string error in result.Errors)
            //        {
            //            ModelState.AddModelError(count.ToString(), error);
            //            errors.Add(error);
            //            count++;
            //        }
            //    }

            //    if (ModelState.IsValid)
            //    {
            //        // No ModelState errors are available to send, so just return an empty BadRequest.
            //        return BadRequest();
            //    }


            //    return BadRequest(ModelState);
            //}

            if (result.Errors != null)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }
            return null;
        }

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            var client = _userAppService.GetUser(new GetUserInput() { UserId = clientId }).User.ConvertToUser();

            if (client == null)
            {
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            }

            //if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            //{
            //    return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            //}

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        private List<Person> GetPersonsFromEmail(GetPersonsByEmailInput input)
        {
            using (var repo = new EmailRepository())
            {
                var app = new EmailAppService(repo);
                return app.GetPersonsByEmail(input).EmailPersons
                    .Select(p => p.ConvertToPerson()).ToList();
            }
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = "xxxxxx";
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                //if (provider == "Facebook")
                //{
                //    parsedToken.user_id = jObj["data"]["user_id"];
                //    parsedToken.app_id = jObj["data"]["app_id"];

                //    if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                //    {
                //        return null;
                //    }
                //}
                //else if (provider == "Google")
                //{
                //    parsedToken.user_id = jObj["user_id"];
                //    parsedToken.app_id = jObj["audience"];

                //    if (!string.Equals(Startup.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                //    {
                //        return null;
                //    }

                //}

            }

            return parsedToken;
        }

        private JObject GenerateLocalAccessTokenResponse(string userName)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = ApiStartup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
        );

            return tokenResponse;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }

        #endregion
    }
}
