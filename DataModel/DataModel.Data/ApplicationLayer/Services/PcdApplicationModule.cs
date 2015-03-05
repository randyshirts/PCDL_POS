using System.Reflection;
using Abp.Modules;
using PcdWeb;

namespace DataModel.Data.ApplicationLayer.Services
{
    [DependsOn(typeof(PcdWebCoreModule))]
    public class PcdApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
