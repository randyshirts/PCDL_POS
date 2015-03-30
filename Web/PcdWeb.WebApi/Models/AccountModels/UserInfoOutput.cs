using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataModel.Data.DataLayer.Entities;

namespace PcdWeb.Models.AccountModels
{
    public class UserInfoOutput
    {
        public UserInfoOutput(Person p)
        {
            EmailAddress = p.EmailAddresses.FirstOrDefault() != null ? p.EmailAddresses.FirstOrDefault().EmailAddress : null;
            MailingAddress = p.MailingAddresses.FirstOrDefault() != null
                ? p.MailingAddresses.FirstOrDefault().MailingAddress1
                : null;
            MailingAddress2 = p.MailingAddresses.FirstOrDefault() != null
                ? p.MailingAddresses.FirstOrDefault().MailingAddress2
                : null;
            City = p.MailingAddresses.FirstOrDefault() != null
                ? p.MailingAddresses.FirstOrDefault().City
                : null;
            ZipCode = p.MailingAddresses.FirstOrDefault() != null
                ? p.MailingAddresses.FirstOrDefault().ZipCode
                : null;
            State = p.MailingAddresses.FirstOrDefault() != null
                ? p.MailingAddresses.FirstOrDefault().State
                : null;
            FirstName = p.FirstName;
            LastName = p.LastName;
            HomePhoneNumber = p.PhoneNumbers.HomePhoneNumber;
            CellPhoneNumber = p.PhoneNumbers.CellPhoneNumber;
            AltPhoneNumber = p.PhoneNumbers.AltPhoneNumber;
        }

        public UserInfoOutput()
        {
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string HomePhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string CellPhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string AltPhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        public string MailingAddress { get; set; }

        public string MailingAddress2 { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string State { get; set; }

        public string Message { get; set; }
    }
}
