using System.Reflection;
using Abp.Modules;

namespace PcdWeb
{
    public class PcdWebCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
