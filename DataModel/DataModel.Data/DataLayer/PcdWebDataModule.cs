using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using DataModel.Data.ApplicationLayer.Identity;
using DataModel.Data.TransactionalLayer.Repositories;
using Owin;
using PcdWeb;

namespace DataModel.Data.DataLayer
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(PcdWebCoreModule))]
    public class PcdWebDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            //Database.SetInitializer<PcdWebDbContext>(null);
        }

    }
}