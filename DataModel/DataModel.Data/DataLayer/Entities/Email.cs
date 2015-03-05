using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class Email : Entity
    {

        #region Constructors
        public Email()
        { }

        public Email(int id, string email)
        {
            Id = id;
            EmailAddress = email;
        }
        #endregion

        #region Define Members
        //public int Id { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public virtual ICollection<Person> EmailAddress_Person { get; set; }
       
        #endregion
    }
}


    

