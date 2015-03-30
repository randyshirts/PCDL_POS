using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class EmailRepository : PcdRepositoryBase<Email>, IEmailRepository
    {
        public IEnumerable<Email> GetAllEmails()
        {
            var query = (from e in Context.Emails
                         where (e.EmailAddress_Person != null)
                         select e);
            return query.ToList();
        }

        public IEnumerable<Person> GetPersonsByEmail(string email)
        {
            var query = (from e in Context.Emails
                         where (e.EmailAddress == email)
                         select e.EmailAddress_Person);

            var list = new List<Person>();
            foreach (var personList in query)
            {
                list.AddRange(personList);
            }

            
            return list;
        }
    }
}
