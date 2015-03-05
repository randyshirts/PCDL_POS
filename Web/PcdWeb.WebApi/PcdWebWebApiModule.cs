using System.Reflection;
using Abp.Application.Services;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;
using DataModel.Data.ApplicationLayer.Services;

namespace PcdWeb
{
    [DependsOn(typeof(AbpWebApiModule), typeof(PcdApplicationModule))]
    public class PcdWebWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(PcdApplicationModule).Assembly, "pcdWebApi")
                .Build();
        }
    }
}
