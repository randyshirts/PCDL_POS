using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.TransactionalLayer.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using PcdWeb.Models;


namespace PcdWeb.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {

        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        //private AuthRepository _repo = null;
        private readonly IPcdWebUserAppService _userAppService;
        public AccountController()
        {
            _userAppService = new PcdWebUserAppService();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterUserInput input)
        {

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


            var result = _userAppService.RegisterUser(input).Result;
            //if (result == null) return BadRequest("Registration Failed - Try Again");

            if (result.Result.Succeeded)
            {
                await _userAppService.SendConfirmation(new SendConfirmationInput() {EmailAddress = input.EmailAddress});
                output.message = "success";
            }

            if (result.Result.Errors.Any())
            {
                var alreadyRegisteredError =
                    result.Result.Errors.FirstOrDefault(e => e.StartsWith("You registered with"));
                if (alreadyRegisteredError != null)
                {
                    await
                        _userAppService.SendConfirmation(new SendConfirmationInput() {EmailAddress = input.EmailAddress});
                    output.message = alreadyRegisteredError;
                }
                else
                    output.message = result.Result.Errors.FirstOrDefault();
            }
            //IHttpActionResult errorResult = GetErrorResult(result.Result); 
            //if (errorResult != null)
            //{
            //    return Ok(errorResult);
            //}



            return Ok(output);
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("UpdateRegistration")]
        public async Task<IHttpActionResult> UpdateRegistration(UpdateRegisterUserInput input)
        {
            //define output
            var output = new RegisterApiOutputModel();

            //Get matching person
            var persons = new List<Person>();
            try
            {
                persons = GetPersonsFromEmail(new GetPersonsByEmailInput() {EmailAddress = input.EmailAddress});
            }
            catch (Exception ex)
            {
                output.message = "The server is down, wait an hour or two and if the problem persists call us";
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

        [AcceptVerbs("GET", "POST")]
        [Route("ConfirmEmail")]
        public HttpResponseMessage ConfirmEmail(string userId, string confirmationCode)
        {
            var input = new ConfirmEmailInput { ConfirmationCode = confirmationCode, UserId = userId };

            if (input.UserId == null || input.ConfirmationCode == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var result = _userAppService.ConfirmEmail(input);

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri("http://localhost:61754/#/emailConfirmed");
            return response;


            //return result.Result;
            //return result.Result.Succeeded ? RedirectToAction("Login", new { loginMessage = "Congratulations! Your account is activated. Enter your email address and password to login" })
            //                                : RedirectToAction("Login", new { loginMessage = "Sorry there was an error. Please contact 'Play Create Discover' to resolve" });
        }

        [AcceptVerbs("GET", "POST")]
        [Route("SendConfirmation")]
        public bool SendConfirmation(SendConfirmationInput input)
        {

            if (input.EmailAddress == null)
            {
                return false;
            }

            var result = _userAppService.SendConfirmation(input);

            return result.Result.Result;
            //return result.Result.Succeeded ? RedirectToAction("Login", new { loginMessage = "Congratulations! Your account is activated. Enter your email address and password to login" })
            //                                : RedirectToAction("Login", new { loginMessage = "Sorry there was an error. Please contact 'Play Create Discover' to resolve" });
        }

        // POST api/Account/Register
        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        [Route("GetUsers")]
        public IHttpActionResult GetUsers(GetPersonsByEmailInput input)
        {

            var output = new GetUsersApiOutput();

            //Check for existing user
            var user = new GetUserByUsernameOutput();
            try
            {
                user = _userAppService.GetUserByUsername(new GetUserByUsernameInput() {Email = input.EmailAddress});
            }
            catch (Exception ex)
            {
                output.message = "The server is down, wait an hour or two and if the problem persists call us";
            }

            if (user.User != null)
                output.message = "Email already registered";                      

            if(output.message != null)
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

            var client = _userAppService.GetUser(new GetUserInput() { UserId = Int32.Parse(clientId) }).User.ConvertToUser();

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
