using System.Data.Entity;
using Abp.EntityFramework;
using MySql.Data.Entity;

namespace PcdWeb.EntityFramework
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class PcdWebDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...

        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public PcdWebDbContext()
            : base("Default")
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in PcdWebDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of PcdWebDbContext since ABP automatically handles it.
         */
        public PcdWebDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
        }
    }
}
