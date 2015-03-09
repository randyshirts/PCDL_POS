using DataModel.Data.DataLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class PcdRoleManager : RoleManager<IdentityRole>
    {
        public PcdRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static PcdRoleManager Create(IdentityFactoryOptions<PcdRoleManager> options, IOwinContext context)
        {
            return new PcdRoleManager(new RoleStore<IdentityRole>(context.Get<DataContext>()));
        }
    }
}
