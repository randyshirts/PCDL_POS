using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PcdWeb.Controllers;

namespace PcdWeb.Models.AccountModels
{
    public class RegisterApiOutputModel
    {
        public string message;
    }

    public class GetUsersApiOutput
    {
        public string message;
        public IEnumerable<ExistingUserInfo> users;
    }

    public class ResetPasswordModel : ModelBase
    {
        [RegularExpression(PREVENT_SQL_INJECTION, ErrorMessage = "Invalid Input in Password")]
        public string Password;

        [RegularExpression(PREVENT_SQL_INJECTION, ErrorMessage = "Invalid Input in Password")]
        public string PasswordRepeat;

        [EmailAddress]
        public string EmailAddress;

        [RegularExpression(PREVENT_SQL_INJECTION, ErrorMessage = "Invalid Input in Code")]
        public string Code;
    }
}
