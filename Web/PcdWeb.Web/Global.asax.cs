using System;
using System.Web.Mvc;
using Abp.Dependency;
using Abp.Web;
using Castle.Facilities.Logging;

namespace PcdWeb.Web
{
    public class MvcApplication : AbpWebApplication
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            IocManager.Instance.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            base.Application_Start(sender, e);
        }
    }
}
