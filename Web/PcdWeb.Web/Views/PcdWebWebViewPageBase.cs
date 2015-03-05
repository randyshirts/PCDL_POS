using Abp.Web.Mvc.Views;

namespace PcdWeb.Web.Views
{
    public abstract class PcdWebWebViewPageBase : PcdWebWebViewPageBase<dynamic>
    {

    }

    public abstract class PcdWebWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected PcdWebWebViewPageBase()
        {
            LocalizationSourceName = PcdWebConsts.WebLocalizationSourceName;
        }
    }
}