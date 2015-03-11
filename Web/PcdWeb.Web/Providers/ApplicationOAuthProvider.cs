using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataModel.Data.ApplicationLayer.DTO;
using DataModel.Data.ApplicationLayer.Identity;
using DataModel.Data.ApplicationLayer.Services;
using DataModel.Data.DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace PcdWeb.Web.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<PcdUserManager>();

            User user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var app = new PcdWebUserAppService(userManager);
            var input = new GenerateUserIdentityAsyncInput {User = new UserDto(user)};
            ClaimsIdentity oAuthIdentity = app.GenerateUserOauthIdentityAsync(input).Task.Result;
            ClaimsIdentity cookiesIdentity = app.GenerateUserIdentityAsync(input).Task.Result;  
            AuthenticationProperties properties = CreateProperties(oAuthIdentity);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(ClaimsIdentity identity)
        {
            var roleClaimValues = ((ClaimsIdentity)identity).FindAll(ClaimTypes.Role).Select(c => c.Value);

            var roles = string.Join(",", roleClaimValues);

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", ((ClaimsIdentity) identity).FindFirstValue(ClaimTypes.Name) },
                { "userRoles", roles }
            };
            return new AuthenticationProperties(data);
        }
    }
}
