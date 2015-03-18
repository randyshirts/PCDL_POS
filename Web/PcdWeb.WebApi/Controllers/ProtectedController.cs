﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace PcdWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/protected")]
    public class ProtectedController : ApiController
    {
        [Route("")]
        public IEnumerable<object> Get()
        {
            //string token = "";
            //Microsoft.Owin.Security.AuthenticationTicket ticket = Startup.OAuthBearerOptions.AccessTokenFormat.Unprotect(token);

            var identity = User.Identity as ClaimsIdentity;
           
            return identity.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });
        }
    }
}
