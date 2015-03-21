using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcdWeb.Models
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
}
