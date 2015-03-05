using System.Collections.Generic;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class MailingAddress : Entity
    {

        #region Constructors
        public MailingAddress()
        { }

        public MailingAddress(int id, string mailingAddress1, string mailingAddress2, 
                        string zipCode, string city, string state)
        {
            Id = id;
            MailingAddress1 = mailingAddress1;
            MailingAddress2 = mailingAddress2;
            ZipCode = zipCode;
            City = city;
            State = state;
        }
        #endregion

        #region Define Members
        public string MailingAddress1 { get; set; }
        public string MailingAddress2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public virtual ICollection<Person> MailingAddress_Person { get; set; }
        
       
        #endregion
    }
}


    

