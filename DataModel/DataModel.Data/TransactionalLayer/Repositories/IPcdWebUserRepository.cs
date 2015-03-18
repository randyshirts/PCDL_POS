using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DataModel.Data.DataLayer.Entities;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public interface IPcdWebUserRepository : IRepository<User, string>
    {
        bool UpdateConfirmationCode(User user);
        bool AddUserToPerson(Person person, User user);
        bool ConfirmEmail(string userId, string code);
    }

}
