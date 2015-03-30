using System.Collections.Generic;

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

    public class ResetPasswordModel
    {
        public string Password;
        public string PasswordRepeat;
        public string EmailAddress;
        public string Code;
    }
}
