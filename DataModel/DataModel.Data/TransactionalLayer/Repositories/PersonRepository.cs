using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DataModel.Data.ApplicationLayer.Identity;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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

                if (original.MailingAddresses.Count > 0)
                {
                    i = 0;
                    foreach (var address in original.MailingAddresses)
                    {
                        Context.Entry(address)
                            .CurrentValues.SetValues(updatedPerson.MailingAddresses.ElementAtOrDefault(i));
                        Context.SaveChanges();
                        i++;
                    }
                }
                else
                {
                    if (updatedPerson.MailingAddresses != null)
                    {
                        original.MailingAddresses.Add(updatedPerson.MailingAddresses.FirstOrDefault());
                        Context.SaveChanges();
                    }
    
                }

                Context.Entry(original.PhoneNumbers).CurrentValues.SetValues(updatedPerson.PhoneNumbers);
                Context.SaveChanges();

                //Context.Entry(original.User).CurrentValues.SetValues(updatedPerson.User);
                //Context.SaveChanges();

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
