using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataModel.Data.DataLayer;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class ConsignorRepository : PcdRepositoryBase<Consignor>, IConsignorRepository
    {

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public int AddNewConsignor(Consignor consignor)
        {
            //if (person.Consignor != null)
            //{
            //    throw new ArgumentNullException("Consignor", "Person member Consignor must be null when adding item or SQL will throw exception saying violated relationship property");
            //}
            Consignor oldConsignor = null;

            //Validation if a record already exists for this consignor
            if (consignor != null)
            {
                oldConsignor = Get(consignor.Id);
            }
            if (oldConsignor != null)
            {
                Validation.ConsignorPersonDuplicate(consignor, oldConsignor);
            }
            //Consignor doesn't exist so add it
            else
            {
                Validation.StringRequire(consignor.Consignor_Person.FirstName);
                Validation.StringRequire(consignor.Consignor_Person.LastName);
                Validation.NameLength(consignor.Consignor_Person.FirstName);
                Validation.NameLength(consignor.Consignor_Person.LastName);
                Validation.DateRequire(consignor.DateAdded);
                Validation.StringRequire(consignor.Consignor_Person.EmailAddresses.FirstOrDefault().EmailAddress);
                Validation.EmailLength(consignor.Consignor_Person.EmailAddresses.FirstOrDefault().EmailAddress);
                Validation.StringRequire(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().MailingAddress1);
                Validation.AddressLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().MailingAddress1);
                Validation.AddressLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().MailingAddress2);
                Validation.StringRequire(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().City);
                Validation.CityLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().City);
                Validation.StringRequire(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().State);
                Validation.StateLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().State);
                Validation.StringRequire(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().ZipCode);
                Validation.ZipLength(consignor.Consignor_Person.MailingAddresses.FirstOrDefault().ZipCode);
                Validation.StringRequire(consignor.Consignor_Person.PhoneNumbers.CellPhoneNumber);
                Validation.PhoneLength(consignor.Consignor_Person.PhoneNumbers.CellPhoneNumber);
                Validation.PhoneLength(consignor.Consignor_Person.PhoneNumbers.HomePhoneNumber);
                Validation.PhoneLength(consignor.Consignor_Person.PhoneNumbers.AltPhoneNumber);
                Context.Consignors.Add(consignor);
            }
            Context.SaveChanges();

            //Link Consignor to Person
            consignor.Consignor_Person.Consignor = consignor;

            using (var persons = new PersonRepository())
            {
                persons.UpdatePerson(consignor.Consignor_Person);
            }

            return consignor.Id;
        }

        public void DeleteConsignerById(int id)
        {

            var tempPerson = new Person();
            var consigner = Context.Consignors.Find(id);
            if (consigner != null)
            {
                tempPerson = consigner.Consignor_Person;
                Context.Entry(consigner).State = EntityState.Deleted;
                Context.SaveChanges();
            }
            var person = Context.Persons.Find(tempPerson.Id);
            if (person != null)
            {
                var phoneNumber = person.PhoneNumbers;
                Context.Entry(phoneNumber).State = EntityState.Deleted;
                Context.SaveChanges();
                Context.Entry(person).State = EntityState.Deleted;
                Context.SaveChanges();
            }
        }

        public void UpdateConsignor(Consignor updatedConsignor)
        {

            var original = Context.Consignors.Find(updatedConsignor.Id);

            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(updatedConsignor);
                Context.SaveChanges();
            }
        }

        

        public Consignor GetConsignorByName(string firstName, string lastName)
        {
            Consignor consignor = (from c in Context.Consignors
                                   where ((c.Consignor_Person.FirstName == firstName) &&
                                            (c.Consignor_Person.LastName == lastName))
                                   select c).FirstOrDefault<Consignor>();

            return consignor;
        }

        public IQueryable<Person> QueryPersonsThatAreConsigners()
        {
            var persons = Context.Persons
                           .Where(x => x.Consignor != null);
            return persons;
        }

        public List<string> GetConsignorNames()
        {
            var consignors = QueryPersonsThatAreConsigners().ToList();
            return consignors.Select(person => person.FirstName + " " + person.LastName).ToList();
        }

        public List<string> GetConsignorNamesLastnameFirst()
        {
            var consignors = QueryPersonsThatAreConsigners().ToList();
            return consignors.Select(person => person.LastName + " " + person.FirstName).ToList();
        }

        public Consignor GetConsignorByFullName(string fullName)
        {
            var names = fullName.Split(' ');
            var firstName = names[0];
            var lastName = names[1];
            var consignor = GetConsignorByName(firstName, lastName);

            return consignor;
        }

        public double GetConsignorCreditBalance(string name)
        {
            //Split full name
            if (String.IsNullOrEmpty(name)) return 0;
            var names = name.Split(' ');
            var lastName = names[0];
            var firstName = names[1];

            //Find consignor
            var consignor = GetConsignorByName(firstName, lastName);

            //Sum StoreCreditPmts which are store credit then subtract the sum of StoreCreditTransactions
            var scps = new StoreCreditPmtRepository();
            var pmts = scps.GetStoreCreditPmtsByConsignorId(consignor.Id); 
            if(pmts == null) return 0;

            var scts = new StoreCreditTransactionRepository();
            var purchases = scts.GetStoreCreditTransactionsByConsignorId(consignor.Id);
            if (purchases == null) return pmts.Sum(f => f.StoreCreditPmtAmount);

            return pmts.Sum(f => f.StoreCreditPmtAmount) -
                        purchases.Sum(f => f.StoreCreditTransactionAmount);
        }
    }
}
