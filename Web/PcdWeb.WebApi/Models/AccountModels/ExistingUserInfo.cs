using System.ComponentModel.DataAnnotations;
using DataModel.Data.DataLayer.Entities;

namespace PcdWeb.Models.AccountModels
{
    public class ExistingUserInfo
    {
        public ExistingUserInfo(Person p)
        {
            IsSelected = false;
            FirstName = p.FirstName;
            LastName = p.LastName;
            PhoneNumber = p.PhoneNumbers.CellPhoneNumber ?? p.PhoneNumbers.HomePhoneNumber;
        }

        [Display(Name = "Select")]
        public bool IsSelected { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
