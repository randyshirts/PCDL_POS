using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using PcdWeb.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace PcdWeb.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {          
            //// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Views/Durandal/Index")
            //});

            //// Use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}
