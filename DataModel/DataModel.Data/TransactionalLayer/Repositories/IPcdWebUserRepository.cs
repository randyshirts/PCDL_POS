using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DataModel.Data.DataLayer.Entities;
using Microsoft.AspNet.Identity;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public interface IPcdWebUserRepository : IRepository<User, string>
    {
        Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, User user);
    }

}
