using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;
using DataModel.Data.DataLayer.Entities;
using PcdWeb.Controllers;

namespace PcdWeb.Models.AccountModels
{
    public class ExistingUserInfo : ModelBase
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
        [RegularExpression(NAME_REGEX, ErrorMessage = "Invalid Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(NAME_REGEX, ErrorMessage = "Invalid Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
