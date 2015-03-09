using System.Collections.Generic;
using DataModel.Data.DataLayer.Repositories;
using Newtonsoft.Json;

namespace DataModel.Data.DataLayer.Entities
{
    [JsonObject(IsReference = true)]
    public class Person : Entity
    {

        #region Constructors

        //public Person(int id, string firstName, string lastName)
        //{
        //    Id = id;
        //    FirstName = firstName;
        //    LastName = lastName;       
        //}
        #endregion

        #region Define Members
        //public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Consignor Consignor { get; set; }
        public virtual Member Member { get; set; }
        public virtual Volunteer Volunteer { get; set; }
        public virtual ICollection<MailingAddress> MailingAddresses { get; set; }
        public virtual ICollection<Email> EmailAddresses { get; set; }
        public virtual PhoneNumber PhoneNumbers { get; set; }
        public virtual User User { get; set; }
        #endregion
    }
}


    

