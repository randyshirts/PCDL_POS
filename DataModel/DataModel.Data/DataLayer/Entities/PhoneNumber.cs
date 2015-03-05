using System.ComponentModel.DataAnnotations;
using DataModel.Data.DataLayer.Repositories;

namespace DataModel.Data.DataLayer.Entities
{
    public class PhoneNumber : Entity
    {

        #region Constructors
        public PhoneNumber()
        { }

        public PhoneNumber(int id, string firstName, string lastName, string email,
                       string homePhoneNumber, string cellPhoneNumber, string altPhoneNumber, 
                        string mailingAddress, string zipCode, string city, string state)
        {
            Id = id;
            HomePhoneNumber = homePhoneNumber;
            CellPhoneNumber = cellPhoneNumber;
            AltPhoneNumber = altPhoneNumber;
        }
        #endregion

        #region Define Members
        //public int Id { get; set; }
        [Phone]
        public string HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string AltPhoneNumber { get; set; }
        public virtual Person PhoneNumber_Person { get; set; }
       
        #endregion
    }
}


    

