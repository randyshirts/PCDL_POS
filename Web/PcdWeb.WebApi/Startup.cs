using System;
using System.Web.Http;
using AngularJSAuthentication.API;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PcdWeb;
using PcdWeb.Providers;
using DataModel.Data;
using DataModel.Data.ApplicationLayer.Identity;
using DataModel.Data.DataLayer;
using DataModel.Data.TransactionalLayer.Repositories;

//using Microsoft.Owin.Security.Facebook;
//using Microsoft.Owin.Security.Google;

[assembly: OwinStartup(typeof(ApiStartup))]
namespace PcdWeb
{
    public class ApiStartup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        //public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        //public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);
            ConfigureIdentityManagers(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, AngularJSAuthentication.API.Migrations.Configuration>());

        }

        public void ConfigureIdentityManagers(IAppBuilder app)
        {
            //ConfigureAuth(app);
            // Configure the db context, user manager and role manager to use a single instance per request
            app.CreatePerOwinContext(DataContext.Create);
            app.CreatePerOwinContext<PcdUserManager>(PcdUserManager.Create);
            app.CreatePerOwinContext<PcdRoleManager>(PcdRoleManager.Create);
            app.CreatePerOwinContext<PcdSignInManager>(PcdSignInManager.Create);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions() {
            
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(),
                //RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            ////Configure Google External Login
            //googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "xxxxxx",
            //    ClientSecret = "xxxxxx",
            //    Provider = new GoogleAuthProvider()
            //};
            //app.UseGoogleAuthentication(googleAuthOptions);

            ////Configure Facebook External Login
            //facebookAuthOptions = new FacebookAuthenticationOptions()
            //{
            //    AppId = "xxxxxx",
            //    AppSecret = "xxxxxx",
            //    Provider = new FacebookAuthProvider()
            //};
            //app.UseFacebookAuthentication(facebookAuthOptions);

        }
    }

}