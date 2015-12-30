using Abp.Web.Mvc.Controllers;

namespace PcdWeb.Controllers
{
    public abstract class PcdWebControllerBase : AbpController
    {
        protected PcdWebControllerBase()
        {
            LocalizationSourceName = PcdWebConsts.WebLocalizationSourceName;
        }


    }
}