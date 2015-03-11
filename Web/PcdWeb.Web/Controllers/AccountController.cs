//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Http;
//using System.Web.Http.Results;
////using System.Web.Http.Results;
//using System.Net.Http;
//using System.Web.Mvc;
//using Abp.Runtime.Security;
//using Abp.UI;
//using Abp.Web.Mvc.Authorization;
//using Abp.Web.Mvc.Models;
//using DataModel.Data.ApplicationLayer.DTO;
//using DataModel.Data.ApplicationLayer.Identity;
//using DataModel.Data.ApplicationLayer.Services;
//using DataModel.Data.DataLayer.Entities;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OAuth;
//using PcdWeb.Web.Models;
//using PcdWeb.Web.Providers;
//using PcdWeb.Web.Results;
//using Recaptcha.Web;
//using Recaptcha.Web.Mvc;
//using RedirectResult = System.Web.Http.Results.RedirectResult;

//namespace PcdWeb.Web.Controllers
//{
//    [System.Web.Mvc.Authorize]
//    [System.Web.Mvc.RoutePrefix("api/Account")]
//    public class AccountController : ApiController
//    {
//        private readonly IPcdWebUserAppService _userAppService;
//        private const string LocalLoginProvider = "Local";
//        private const string DefaultUserRole = "RegisteredUsers";

//        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

//        private IAuthenticationManager Authentication
//        {
//            get { return Request.GetOwinContext().Authentication; }
//        }

//        private PcdUserManager _userManager;
//        public PcdUserManager UserManager
//        {
//            get
//            {
//                return _userManager ?? Request.GetOwinContext().GetUserManager<PcdUserManager>();
//            }
//            private set
//            {
//                _userManager = value;
//            }
//        }

//        private readonly RoleManager<UserRole> _roleManager;

//        private PcdSignInManager _signInManager;
//        public PcdSignInManager SignInManager
//        {
//            get
//            {
//                return _signInManager ?? Request.GetOwinContext().Get<PcdSignInManager>();
//            }
//            private set { _signInManager = value; }
//        }

//        private IAuthenticationManager AuthenticationManager
//        {
//            get
//            {
//                return Request.GetOwinContext().Authentication;
//            }
//        }

//        //public AccountController()
//        //    : this(Startup.OAuthOptions.AccessTokenFormat)
//        //{
//        //}

//        public AccountController(ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
//        {
//            //
//            AccessTokenFormat = accessTokenFormat;
//        }

//        public AccountController(PcdUserManager userManager, PcdSignInManager signInManager)
//        {
//            UserManager = userManager;
//            SignInManager = signInManager;
//        }

//        public AccountController()
//        {         
//            _userManager = UserManager;
//            _signInManager = SignInManager;

//            var userAppService = new PcdWebUserAppService(_userManager);
//            _userAppService = userAppService;
//        }

//        // GET api/Account/UserInfo
//        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
//        [System.Web.Mvc.Route("UserInfo")]
//        public UserInfoViewModel GetUserInfo()
//        {
//            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

//            var roleClaimValues = ((ClaimsIdentity)User.Identity).FindAll(ClaimTypes.Role).Select(c => c.Value);

//            var roles = string.Join(",", roleClaimValues);

//            return new UserInfoViewModel
//            {
//                UserName = User.Identity.GetUserName(),
//                Email = ((ClaimsIdentity)User.Identity).FindFirstValue(ClaimTypes.Email),
//                HasRegistered = externalLogin == null,
//                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null,
//                UserRoles = roles
//            };
//        }

        

//        //[System.Web.Mvc.AllowAnonymous]
//        //public virtual ActionResult Login(string returnUrl = "", string loginMessage = "")
//        //{
//        //    if (string.IsNullOrWhiteSpace(returnUrl))
//        //    {
//        //        returnUrl = Request.ApplicationPath;
//        //    }

//        //    ViewBag.ReturnUrl = returnUrl;
//        //    ViewBag.LoginMessage = loginMessage;
//        //    return View();
//        //}

//        //[System.Web.Mvc.HttpPost]
//        ////public virtual async Task<JsonResult> Login(LoginModel loginModel, string returnUrl = "")
//        //public virtual async Task<ActionResult> Login(LoginModel loginModel, string returnUrl = "")
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        throw new UserFriendlyException("Your form is invalid!");
//        //    }

//        //    if (!ModelState.IsValid)
//        //    {
//        //        return View(loginModel);
//        //    }

//        //    // This doen't count login failures towards lockout only two factor authentication
//        //    // To enable password failures to trigger lockout, change to shouldLockout: true
//        //    var result = await SignInManager.PasswordSignInAsync(loginModel.EmailAddress, loginModel.Password, loginModel.RememberMe, shouldLockout: false);
//        //    switch (result)
//        //    {
//        //        case SignInStatus.Success:
//        //            return RedirectToLocal(returnUrl);
//        //        case SignInStatus.LockedOut:
//        //            return View("Lockout");
//        //        case SignInStatus.RequiresVerification:
//        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
//        //        case SignInStatus.Failure:
//        //        default:
//        //            ModelState.AddModelError("", "Invalid login attempt.");
//        //            return View(loginModel);
//        //    }
//        //    //var user = await _userManager.FindAsync(loginModel.EmailAddress, loginModel.Password);
//        //    //if (user == null)
//        //    //{
//        //    //    throw new UserFriendlyException("Invalid user name or password!");
//        //    //}

//        //    //await SignInAsync(user, loginModel.RememberMe);

//        //    //if (string.IsNullOrWhiteSpace(returnUrl))
//        //    //{
//        //    //    returnUrl = Request.ApplicationPath;
//        //    //}

//        //    //return Json(new MvcAjaxResponse { TargetUrl = returnUrl });
//        //}

//        //
//        //// GET: /Account/VerifyCode
//        //[System.Web.Mvc.AllowAnonymous]
//        //public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
//        //{
//        //    // Require that the user has already logged in via username/password or external login
//        //    if (!await SignInManager.HasBeenVerifiedAsync())
//        //    {
//        //        return View("Error");
//        //    }
//        //    var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
//        //    if (user != null)
//        //    {
//        //        ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
//        //    }
            
//        //    //return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
//        //    return null;
//        //}

//        //private async Task SignInAsync(User user, bool isPersistent)
//        //{
//        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
//        //    var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
//        //    identity.AddClaim(new Claim(AbpClaimTypes.TenantId, "42"));
//        //    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
//        //}

//        //public ActionResult ConfirmEmail(ConfirmEmailInput input)
//        //{
            
//        //    if (input.UserId == 0 || input.ConfirmationCode == null)
//        //    {
//        //        return View("Error");
//        //    }

//        //    var result = _userAppService.ConfirmEmail(input);

//        //    return result.Result.Succeeded ? RedirectToAction("Login", new { loginMessage = "Congratulations! Your account is activated. Enter your email address and password to login" })
//        //                                    : RedirectToAction("Login", new { loginMessage = "Sorry there was an error. Please contact 'Play Create Discover' to resolve" });
//        //}

//        //[AbpAuthorize]
//        //public virtual ActionResult Logout()
//        //{
//        //    AuthenticationManager.SignOut();
//        //    return RedirectToAction("Login");
//        //}

//        // POST api/Account/Logout
//        [System.Web.Http.Route("Logout")]
//        public IHttpActionResult Logout()
//        {
//            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
//            return this.Ok();
//        }

//        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
//        [System.Web.Http.Route("ManageInfo")]
//        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
//        {
//            User user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

//            if (user == null)
//            {
//                return null;
//            }

//            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

//            foreach (IdentityUserLogin linkedAccount in user.Logins)
//            {
//                logins.Add(new UserLoginInfoViewModel
//                {
//                    LoginProvider = linkedAccount.LoginProvider,
//                    ProviderKey = linkedAccount.ProviderKey
//                });
//            }

//            if (user.PasswordHash != null)
//            {
//                logins.Add(new UserLoginInfoViewModel
//                {
//                    LoginProvider = LocalLoginProvider,
//                    ProviderKey = user.UserName,
//                });
//            }

//            return new ManageInfoViewModel
//            {
//                LocalLoginProvider = LocalLoginProvider,
//                Email = user.Email,
//                UserName = user.UserName,
//                Logins = logins,
//                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
//            };
//        }

//        // POST api/Account/ChangePassword
//        [System.Web.Http.Route("ChangePassword")]
//        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
//                model.NewPassword);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }

//        // POST api/Account/SetPassword
//        [System.Web.Http.Route("SetPassword")]
//        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }

//        // POST api/Account/AddExternalLogin
//        [System.Web.Http.Route("AddExternalLogin")]
//        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

//            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

//            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
//                && ticket.Properties.ExpiresUtc.HasValue
//                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
//            {
//                return BadRequest("External login failure.");
//            }

//            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

//            if (externalData == null)
//            {
//                return BadRequest("The external login is already associated with an account.");
//            }

//            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
//                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }

//        // POST api/Account/RemoveLogin
//        [System.Web.Http.Route("RemoveLogin")]
//        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            IdentityResult result;

//            if (model.LoginProvider == LocalLoginProvider)
//            {
//                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
//            }
//            else
//            {
//                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
//                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
//            }

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }

//        // GET api/Account/ExternalLogin
//        [System.Web.Http.OverrideAuthentication]
//        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
//        [System.Web.Http.AllowAnonymous]
//        [System.Web.Http.Route("ExternalLogin", Name = "ExternalLogin")]
//        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
//        {
//            if (error != null)
//            {
//                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
//            }

//            if (!User.Identity.IsAuthenticated)
//            {
//                return new ChallengeResult(provider, this);
//            }

//            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

//            if (externalLogin == null)
//            {
//                return InternalServerError();
//            }

//            if (externalLogin.LoginProvider != provider)
//            {
//                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
//                return new ChallengeResult(provider, this);
//            }

//            User user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
//                externalLogin.ProviderKey));

//            bool hasRegistered = user != null;

//            if (hasRegistered)
//            {
//                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

//                var input = new GenerateUserIdentityAsyncInput {User = new UserDto(user)};
//                ClaimsIdentity oAuthIdentity = _userAppService.GenerateUserOauthIdentityAsync(input).Task.Result;
//                ClaimsIdentity cookieIdentity = _userAppService.GenerateUserIdentityAsync(input).Task.Result;

//                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(oAuthIdentity);
//                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
//            }
//            else
//            {
//                IEnumerable<Claim> claims = externalLogin.GetClaims();
//                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
//                Authentication.SignIn(identity);
//            }

//            return Ok();
//        }

//        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
//        [System.Web.Http.AllowAnonymous]
//        [System.Web.Http.Route("ExternalLogins")]
//        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
//        {
//            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
//            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

//            string state;

//            if (generateState)
//            {
//                const int strengthInBits = 256;
//                state = RandomOAuthStateGenerator.Generate(strengthInBits);
//            }
//            else
//            {
//                state = null;
//            }

//            foreach (AuthenticationDescription description in descriptions)
//            {
//                ExternalLoginViewModel login = new ExternalLoginViewModel
//                {
//                    Name = description.Caption,
//                    Url = Url.Route("ExternalLogin", new
//                    {
//                        provider = description.AuthenticationType,
//                        response_type = "token",
//                        client_id = Startup.PublicClientId,
//                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
//                        state = state
//                    }),
//                    State = state
//                };
//                logins.Add(login);
//            }

//            return logins;
//        }

//        // POST api/Account/Register
//        [System.Web.Http.AllowAnonymous]
//        [System.Web.Http.Route("Register")]
//        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var user = new User() { UserName = model.UserName, Email = model.Email };

//            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            result = await UserManager.AddToRoleAsync(user.Id, DefaultUserRole);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }

//        // POST api/Account/RegisterExternal
//        [System.Web.Http.OverrideAuthentication]
//        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
//        [System.Web.Http.Route("RegisterExternal")]
//        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var info = await Authentication.GetExternalLoginInfoAsync();
//            if (info == null)
//            {
//                return InternalServerError();
//            }

//            var user = new User() { UserName = model.UserName, Email = model.Email != null ? model.Email : "" };

//            IdentityResult result = await UserManager.CreateAsync(user);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            result = await UserManager.AddLoginAsync(user.Id, info.Login);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            result = await UserManager.AddToRoleAsync(user.Id, DefaultUserRole);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                UserManager.Dispose();
//            }

//            base.Dispose(disposing);
//        }



//        //public ActionResult ActivationInfo()
//        //{
//        //    return View();
//        //}

//        //[System.Web.Mvc.AllowAnonymous]
//        //[System.Web.Mvc.HttpPost]
//        //public JsonResult Register(RegisterUserInput input)
//        //{
//        //    //TODO: Return better exception messages!
//        //    //TODO: Show captcha after filling register form, not on startup!

//        //    //if (!ModelState.IsValid)
//        //    //{
//        //    //    throw new UserFriendlyException("Your form is invalid!");
//        //    //}

//        //    //var recaptchaHelper = this.GetRecaptchaVerificationHelper(this.ToString(), recaptchaPrivateKey);
//        //    var recaptchaHelper = this.GetRecaptchaVerificationHelper();
//        //    if (String.IsNullOrEmpty(recaptchaHelper.Response))
//        //    {
//        //        throw new UserFriendlyException("Captcha answer cannot be empty.");
//        //    }

//        //    var recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
//        //    if (recaptchaResult != RecaptchaVerificationResult.Success)
//        //    {
//        //        throw new UserFriendlyException("Incorrect captcha answer.");
//        //    }

//        //    //input.ProfileImage = ProfileImageHelper.GenerateRandomProfileImage();

//        //    _userAppService.RegisterUser(input);

//        //    return Json(new MvcAjaxResponse { TargetUrl = Url.Action("ActivationInfo") });
//        //}


//        //public JsonResult SendPasswordResetLink(SendPasswordResetLinkInput input)
//        //{
//        //    _userAppService.SendPasswordResetLink(input);

//        //    return Json(new MvcAjaxResponse());
//        //}

//        //[System.Web.Mvc.HttpPost]
//        //[System.Web.Mvc.AllowAnonymous]
//        //[ValidateAntiForgeryToken]
//        //public async Task<ActionResult> ResetPassword(int userId, string resetCode, string email, string password)
//        //{

//        //    if (!ModelState.IsValid)
//        //    {
//        //        return View(new ResetPasswordViewModel { UserId = userId, ResetCode = resetCode });
//        //    }
            
//        //    var user = await UserManager.FindByNameAsync(email);
            
//        //    if (user == null)
//        //    {
//        //        // Don't reveal that the user does not exist
//        //        return RedirectToAction("ResetPassword", "Account");
//        //    }
//        //    var result = await UserManager.ResetPasswordAsync(user.Id, resetCode, password);
//        //    if (result.Succeeded)
//        //    {
//        //        return RedirectToAction("ResetPassword", "Account");
//        //    }
//        //    AddErrors(result);

//        //    return View(new ResetPasswordViewModel { UserId = userId, ResetCode = resetCode });

//        //}

//        //// GET: /Account/ResetPasswordConfirmation
//        //[System.Web.Mvc.AllowAnonymous]
//        //public ActionResult ResetPassword()
//        //{
//        //    return View();
//        //}

//        //[System.Web.Mvc.HttpPost]
//        //public JsonResult ResetPassword(ResetPasswordInput input)
//        //{
//        //    var recaptchaHelper = this.GetRecaptchaVerificationHelper();
//        //    if (String.IsNullOrEmpty(recaptchaHelper.Response))
//        //    {
//        //        throw new UserFriendlyException("Captcha answer cannot be empty.");
//        //    }

//        //    var recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
//        //    if (recaptchaResult != RecaptchaVerificationResult.Success)
//        //    {
//        //        throw new UserFriendlyException("Incorrect captcha answer.");
//        //    }

//        //    _userAppService.ResetPassword(input);

//        //    return Json(new MvcAjaxResponse { TargetUrl = Url.Action("Login") });
//        //}

//        //[System.Web.Mvc.Authorize]
//        //public JsonResult KeepSessionOpen()
//        //{
//        //    return Json(new MvcAjaxResponse());
//        //}

//        //private ActionResult RedirectToLocal(string returnUrl)
//        //{
//        //    if (Url.IsLocalUrl(returnUrl))
//        //    {
//        //        return Redirect(returnUrl);
//        //    }
//        //    return RedirectToAction("Index", "Home");
//        //}

//        //[System.Web.Mvc.HttpPost]
//        //[System.Web.Mvc.AllowAnonymous]
//        //[ValidateAntiForgeryToken]
//        //public async Task<ActionResult> ForgotPassword(string email)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        var user = await UserManager.FindByNameAsync(email);
//        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
//        //        {
//        //            // Don't reveal that the user does not exist or is not confirmed
//        //            return View("ForgotPasswordConfirmation");
//        //        }

//        //        var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
//        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
//        //        await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
//        //        ViewBag.Link = callbackUrl;
//        //        return View("ForgotPasswordConfirmation");
                
//        //    }
//        //    return null;
//        //    // If we got this far, something failed, redisplay form
//        //    //return View(model);
//        //}

//        //// GET: /Account/SendCode
//        //[System.Web.Mvc.AllowAnonymous]
//        //public async Task<ActionResult> SendCode(string returnUrl)
//        //{
//        //    var userId = await SignInManager.GetVerifiedUserIdAsync();
//        //    if (userId == null)
//        //    {
//        //        return View("Error");
//        //    }
//        //    var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
//        //    var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
//        //    return View(new SendCode { Providers = factorOptions, ReturnUrl = returnUrl });
//        //}

//        //// POST: /Account/SendCode
//        //[System.Web.Mvc.HttpPost]
//        //[System.Web.Mvc.AllowAnonymous]
//        //[ValidateAntiForgeryToken]
//        //public async Task<ActionResult> SendCode(SendCode model)
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        return View();
//        //    }

//        //    // Generate the token and send it
//        //    if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
//        //    {
//        //        return View("Error");
//        //    }
//        //    return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
//        //}

//        //// POST: /Account/LogOff
//        //[System.Web.Mvc.HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public ActionResult LogOff()
//        //{
//        //    AuthenticationManager.SignOut();
//        //    return RedirectToAction("Index", "Home");
//        //}

//        //// GET: /Account/ForgotPasswordConfirmation
//        //[System.Web.Mvc.AllowAnonymous]
//        //public ActionResult ForgotPasswordConfirmation()
//        //{
//        //    return View();
//        //}

//        //private void AddErrors(IdentityResult result)
//        //{
//        //    foreach (var error in result.Errors)
//        //    {
//        //        ModelState.AddModelError("", error);
//        //    }
//        //}

        

//        private class ExternalLoginData
//        {
//            public string LoginProvider { get; set; }
//            public string ProviderKey { get; set; }
//            public string UserName { get; set; }
//            public string Email { get; set; }

//            public IList<Claim> GetClaims()
//            {
//                IList<Claim> claims = new List<Claim>();
//                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

//                if (UserName != null)
//                {
//                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
//                }

//                if (Email != null)
//                {
//                    claims.Add(new Claim(ClaimTypes.Email, Email, null, LoginProvider));
//                }

//                return claims;
//            }

//            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
//            {
//                if (identity == null)
//                {
//                    return null;
//                }

//                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

//                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
//                    || String.IsNullOrEmpty(providerKeyClaim.Value))
//                {
//                    return null;
//                }

//                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
//                {
//                    return null;
//                }

//                return new ExternalLoginData
//                {
//                    LoginProvider = providerKeyClaim.Issuer,
//                    ProviderKey = providerKeyClaim.Value,
//                    UserName = identity.FindFirstValue(ClaimTypes.Name),
//                    Email = identity.FindFirstValue(ClaimTypes.Email)
//                };
//            }
//        }

//        private static class RandomOAuthStateGenerator
//        {
//            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

//            public static string Generate(int strengthInBits)
//            {
//                const int bitsPerByte = 8;

//                if (strengthInBits % bitsPerByte != 0)
//                {
//                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
//                }

//                int strengthInBytes = strengthInBits / bitsPerByte;

//                byte[] data = new byte[strengthInBytes];
//                _random.GetBytes(data);
//                return HttpServerUtility.UrlTokenEncode(data);
//            }
//        }

//        private IHttpActionResult GetErrorResult(IdentityResult result)
//        {
//            if (result == null)
//            {
//                return InternalServerError();
//            }

//            if (!result.Succeeded)
//            {
//                if (result.Errors != null)
//                {
//                    foreach (string error in result.Errors)
//                    {
//                        ModelState.AddModelError("", error);
//                    }
//                }

//                if (ModelState.IsValid)
//                {
//                    // No ModelState errors are available to send, so just return an empty BadRequest.
//                    return BadRequest();
//                }

//                return BadRequest(ModelState);
//            }

//            return null;
//        }
//        // POST: /Account/VerifyCode
//        //[HttpPost]
//        //[AllowAnonymous]
//        //[ValidateAntiForgeryToken]
//        //public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        return View(model);
//        //    }

//        //    var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
//        //    switch (result)
//        //    {
//        //        case SignInStatus.Success:
//        //            return RedirectToLocal(model.ReturnUrl);
//        //        case SignInStatus.LockedOut:
//        //            return View("Lockout");
//        //        case SignInStatus.Failure:
//        //        default:
//        //            ModelState.AddModelError("", "Invalid code.");
//        //            return View(model);
//        //    }
//        //}

//        // POST: /Account/Register
//        //[HttpPost]
//        //[AllowAnonymous]
//        //[ValidateAntiForgeryToken]
//        //public async Task<JsonResult> Register(RegisterUserInput input)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        var user = new User { UserName = input.EmailAddress, Email = input.EmailAddress};
//        //        var result = await UserManager.CreateAsync(user, input.Password);
//        //        if (result.Succeeded)
//        //        {
//        //            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
//        //            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
//        //            await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
//        //            ViewBag.Link = callbackUrl;
//        //            return View("DisplayEmail");
//        //        }
//        //        AddErrors(result);
//        //    }

//        //    // If we got this far, something failed, redisplay form
//        //    return View(model);
//        //}


//    }
//}
