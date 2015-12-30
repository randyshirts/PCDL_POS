using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcdWeb.Models
{
    public class ModelBase
    {
        public const string PREVENT_SQL_INJECTION = @"/\w*((\%27)|(\'))((\%6F)|o|(\%4F))((\%72)|r|(\%52))/ix";
        public const string NAME_REGEX = @"^[a-zA-Z\s]+$";
    }
}
