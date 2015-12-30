using System.ComponentModel.DataAnnotations;
using PcdWeb.Controllers;

namespace PcdWeb.Models.AccountModels
{
    public class UserModel : ModelBase
    {
        [Required]
        [RegularExpression(NAME_REGEX, ErrorMessage = @"Invalid input for Username")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(PREVENT_SQL_INJECTION, ErrorMessage = "Invalid Input for Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [RegularExpression(PREVENT_SQL_INJECTION, ErrorMessage = "Invalid Input for Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

   
}