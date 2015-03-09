using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities
{
    [JsonObject(IsReference = true)]
    public class UserRole : IdentityRole, IRole
    {
        public UserRole()
        { }

        public UserRole(string roleName, string description) : base(roleName)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
