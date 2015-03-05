using Abp.Application.Services;
using PcdWeb;

namespace DataModel.Data.ApplicationLayer.Services
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class PcdAppServiceBase : ApplicationService
    {
        protected PcdAppServiceBase()
        {
            LocalizationSourceName = PcdWebConsts.WebLocalizationSourceName;
        }
    }
}