using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Runtime.Security;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using Abp.Web.Mvc.Models;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Identity;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PcdWeb.Web.Models;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;

namespace PcdWeb.Web.Controllers
{
    public class AccountController : PcdWebControllerBase
    {
        private readonly IPcdWebUserAppService _userAppService;

        private PcdUserManager _userManager;
        public PcdUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<PcdUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private readonly RoleManager<UserRole> _roleManager;

        private PcdSignInManager _signInManager;
        public PcdSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<PcdSignInManager>();
            }
            private set { _signInManager = value; }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public AccountController(PcdUserManager userManager, PcdSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public AccountController()
        {         
            _userManager = UserManager;
            _signInManager = SignInManager;

            var userAppService = new PcdWebUserAppService(_userManager);
            _userAppService = userAppService;
        }

        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl = "", string loginMessage = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginMessage = loginMessage;
            return View();
        }

        [HttpPost]
        //public virtual async Task<JsonResult> Login(LoginModel loginModel, string returnUrl = "")
        public virtual async Task<ActionResult> Login(LoginModel loginModel, string returnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Your form is invalid!");
            }

            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(loginModel.EmailAddress, loginModel.Password, loginModel.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(loginModel);
            }
            //var user = await _userManager.FindAsync(loginModel.EmailAddress, loginModel.Password);
            //if (user == null)
            //{
            //    throw new UserFriendlyException("Invalid user name or password!");
            //}

            //await SignInAsync(user, loginModel.RememberMe);

            //if (string.IsNullOrWhiteSpace(returnUrl))
            //{
            //    returnUrl = Request.ApplicationPath;
            //}

            //return Json(new MvcAjaxResponse { TargetUrl = returnUrl });
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            
            //return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
            return null;
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(AbpClaimTypes.TenantId, "42"));
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }

        public ActionResult ConfirmEmail(ConfirmEmailInput input)
        {
            
            if (input.UserId == 0 || input.ConfirmationCode == null)
            {
                return View("Error");
            }

            var result = _userAppService.ConfirmEmail(input);

            return result.Result.Succeeded ? RedirectToAction("Login", new { loginMessage = "Congratulations! Your account is activated. Enter your email address and password to login" })
                                            : RedirectToAction("Login", new { loginMessage = "Sorry there was an error. Please contact 'Play Create Discover' to resolve" });
        }

        [AbpAuthorize]
        public virtual ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult ActivationInfo()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Register(RegisterUserInput input)
        {
            //TODO: Return better exception messages!
            //TODO: Show captcha after filling register form, not on startup!

            //if (!ModelState.IsValid)
            //{
            //    throw new UserFriendlyException("Your form is invalid!");
            //}

            //var recaptchaHelper = this.GetRecaptchaVerificationHelper(this.ToString(), recaptchaPrivateKey);
            var recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (String.IsNullOrEmpty(recaptchaHelper.Response))
            {
                throw new UserFriendlyException("Captcha answer cannot be empty.");
            }

            var recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
                throw new UserFriendlyException("Incorrect captcha answer.");
            }

            //input.ProfileImage = ProfileImageHelper.GenerateRandomProfileImage();

            _userAppService.RegisterUser(input);

            return Json(new MvcAjaxResponse { TargetUrl = Url.Action("ActivationInfo") });
        }

        public JsonResult SendPasswordResetLink(SendPasswordResetLinkInput input)
        {
            _userAppService.SendPasswordResetLink(input);

            return Json(new MvcAjaxResponse());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(int userId, string resetCode, string email, string password)
        {

            if (!ModelState.IsValid)
            {
                return View(new ResetPasswordViewModel { UserId = userId, ResetCode = resetCode });
            }
            
            var user = await UserManager.FindByNameAsync(email);
            
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPassword", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, resetCode, password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPassword", "Account");
            }
            AddErrors(result);

            return View(new ResetPasswordViewModel { UserId = userId, ResetCode = resetCode });

        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ResetPassword(ResetPasswordInput input)
        {
            var recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (String.IsNullOrEmpty(recaptchaHelper.Response))
            {
                throw new UserFriendlyException("Captcha answer cannot be empty.");
            }

            var recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
                throw new UserFriendlyException("Incorrect captcha answer.");
            }

            _userAppService.ResetPassword(input);

            return Json(new MvcAjaxResponse { TargetUrl = Url.Action("Login") });
        }

        [Authorize]
        public JsonResult KeepSessionOpen()
        {
            return Json(new MvcAjaxResponse());
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Link = callbackUrl;
                return View("ForgotPasswordConfirmation");
                
            }
            return null;
            // If we got this far, something failed, redisplay form
            //return View(model);
        }

        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCode { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCode model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        // POST: /Account/VerifyCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(model.ReturnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid code.");
        //            return View(model);
        //    }
        //}

        // POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<JsonResult> Register(RegisterUserInput input)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new User { UserName = input.EmailAddress, Email = input.EmailAddress};
        //        var result = await UserManager.CreateAsync(user, input.Password);
        //        if (result.Succeeded)
        //        {
        //            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //            await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
        //            ViewBag.Link = callbackUrl;
        //            return View("DisplayEmail");
        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}


    }
}
