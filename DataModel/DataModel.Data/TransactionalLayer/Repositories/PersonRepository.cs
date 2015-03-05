using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class PersonRepository : PcdRepositoryBase<Person>, IPersonRepository
    {
        public void UpdatePerson(Person updatedPerson)
        {
            var original = Context.Persons.Find(updatedPerson.Id);

            if (original != null)
            {
                int i = 0;
                foreach (var email in original.EmailAddresses) 
                { 
                    Context.Entry(email).CurrentValues.SetValues(updatedPerson.EmailAddresses.ElementAtOrDefault(i));
                    Context.SaveChanges();
                    i++;
                }
                i = 0;
                foreach (var address in original.MailingAddresses)
                {
                    Context.Entry(address).CurrentValues.SetValues(updatedPerson.MailingAddresses.ElementAtOrDefault(i));
                    Context.SaveChanges();
                    i++;
                }
 
                Context.Entry(original.PhoneNumbers).CurrentValues.SetValues(updatedPerson.PhoneNumbers);
                Context.SaveChanges();
                
                Context.Entry(original).CurrentValues.SetValues(updatedPerson);
                Context.SaveChanges();
            }
        }

        public IEnumerable<Person> QueryPersonsThatAreConsigners()
        {
            var query = (from p in Context.Persons
                         where (p.Consignor != null) 
                         select p);
            return query.ToList();
        }

        
    }
}
