using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataModel.Data.DataLayer.Entities;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.TransactionalLayer.Repositories
{
    public class MemberRepository : PcdRepositoryBase<Member>, IMemberRepository
    {

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public int AddNewMember(Member member)
        {
            //if (person.Member != null)
            //{
            //    throw new ArgumentNullException("Member", "Person member Member must be null when adding item or SQL will throw exception saying violated relationship property");
            //}
            Member oldMember = null;

            //Validation if a record already exists for this member
            if (member != null)
            {
                oldMember = Get(member.Id);
            }
            if (oldMember != null)
            {
                Validation.MemberPersonDuplicate(member, oldMember);
            }

            //Member doesn't exist so add it
            else
            {
                Validation.StringRequire(member.Member_Person.FirstName);
                Validation.StringRequire(member.Member_Person.LastName);
                Validation.NameLength(member.Member_Person.FirstName);
                Validation.NameLength(member.Member_Person.LastName);
                Validation.DateRequire(member.DateAdded);
                Validation.StringRequire(member.Member_Person.EmailAddresses.FirstOrDefault().EmailAddress);
                Validation.EmailLength(member.Member_Person.EmailAddresses.FirstOrDefault().EmailAddress);
                Validation.StringRequire(member.Member_Person.MailingAddresses.FirstOrDefault().MailingAddress1);
                Validation.AddressLength(member.Member_Person.MailingAddresses.FirstOrDefault().MailingAddress1);
                Validation.AddressLength(member.Member_Person.MailingAddresses.FirstOrDefault().MailingAddress2);
                Validation.StringRequire(member.Member_Person.MailingAddresses.FirstOrDefault().City);
                Validation.CityLength(member.Member_Person.MailingAddresses.FirstOrDefault().City);
                Validation.StringRequire(member.Member_Person.MailingAddresses.FirstOrDefault().State);
                Validation.StateLength(member.Member_Person.MailingAddresses.FirstOrDefault().State);
                Validation.StringRequire(member.Member_Person.MailingAddresses.FirstOrDefault().ZipCode);
                Validation.ZipLength(member.Member_Person.MailingAddresses.FirstOrDefault().ZipCode);
                Validation.StringRequire(member.Member_Person.PhoneNumbers.CellPhoneNumber);
                Validation.PhoneLength(member.Member_Person.PhoneNumbers.CellPhoneNumber);
                Validation.PhoneLength(member.Member_Person.PhoneNumbers.HomePhoneNumber);
                Validation.PhoneLength(member.Member_Person.PhoneNumbers.AltPhoneNumber);
                Context.Members.Add(member);
            }
            Context.SaveChanges();

            //Link Member to Person
            member.Member_Person.Member = member;

            using (var persons = new PersonRepository())
            {
                persons.UpdatePerson(member.Member_Person);
            }

            return member.Id;
        }

        public bool AddMemberToPerson(Person person, Member member)
        {
            var thisPerson = (from p in Context.Persons
                              where (p.Id == person.Id)
                              select p).FirstOrDefault();

            if (thisPerson == null) return false;
            member.Member_Person = thisPerson;
            thisPerson.Member = member;
            return Context.SaveChanges() > 0;
            

        }

        public void DeleteMemberById(int id)
        {

            var tempPerson = new Person();
            var member = Context.Members.Find(id);
            if (member != null)
            {
                tempPerson = member.Member_Person;
                Context.Entry(member).State = EntityState.Deleted;
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

        public void UpdateMember(Member updatedMember)
        {

            var original = Context.Members.Find(updatedMember.Id);

            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(updatedMember);
                Context.SaveChanges();
            }
        }

        

        public Member GetMemberByName(string firstName, string lastName)
        {
            Member member = (from m in Context.Members
                                   where ((m.Member_Person.FirstName == firstName) &&
                                            (m.Member_Person.LastName == lastName))
                                   select m).FirstOrDefault<Member>();

            return member;
        }

        public IQueryable<Person> QueryPersonsThatAreMembers()
        {
            var persons = Context.Persons
                           .Where(x => x.Member != null);
            return persons;
        }

        public List<string> GetMemberNames()
        {
            var members = QueryPersonsThatAreMembers().ToList();
            return members.Select(person => person.FirstName + " " + person.LastName).ToList();
        }

        public List<string> GetMemberNamesLastnameFirst()
        {
            var members = QueryPersonsThatAreMembers().ToList();
            return members.Select(person => person.LastName + " " + person.FirstName).ToList();
        }

        public Member GetMemberByFullName(string fullName)
        {
            var names = fullName.Split(' ');
            var firstName = names[0];
            var lastName = names[1];
            var member = GetMemberByName(firstName, lastName);

            return member;
        }

        public double GetMemberCreditBalance(string name)
        {
        //    //Split full name
        //    if (String.IsNullOrEmpty(name)) return 0;
        //    var names = name.Split(' ');
        //    var lastName = names[0];
        //    var firstName = names[1];

        //    //Find member
        //    var member = GetMemberByName(firstName, lastName);

        //    //Sum StoreCreditPmts which are store credit then subtract the sum of StoreCreditTransactions
        //    var scps = new StoreCreditPmtRepository();
        //    var pmts = scps.GetStoreCreditPmtsByMemberId(member.Id); 
        //    if(pmts == null) return 0;

        //    var scts = new StoreCreditTransactionRepository();
        //    var purchases = scts.GetStoreCreditTransactionsByMemberId(member.Id);
        //    if (purchases == null) return pmts.Sum(f => f.StoreCreditPmtAmount);

        //    return pmts.Sum(f => f.StoreCreditPmtAmount) -
        //                purchases.Sum(f => f.StoreCreditTransactionAmount);
            return 0;
        }

        public Member GetMemberByEmail(string email)
        {
          
            var member = (from c in Context.Members
                                   where (c.Member_Person.EmailAddresses.Any(p => p.EmailAddress == email))
                                   select c).FirstOrDefault();

            return member;
        }
    }
}
