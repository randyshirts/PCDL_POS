using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace PcdWeb.Web
{
    public partial class Startup
    {
        static Startup()
        {
            PublicClientId = "self";
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    
    
        public static string PublicClientId { get; private set; }
    }


}